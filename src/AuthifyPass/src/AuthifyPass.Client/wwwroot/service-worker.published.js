// Offline-first worker used when the app is published (development uses the no-op service-worker.js).
//
// This is a Blazor Web App: the HTML shell is server rendered and there is no static index.html, and
// the shell references host assets (blazor.web.js, fingerprinted css/boot modules) that are not listed
// in the client-generated assets manifest. So we precache the manifest assets TOLERANTLY (a single
// failing asset must not abort the whole install, which is why offline silently broke before), we also
// precache the server-rendered shell "/" and blazor.web.js, and we run runtime caching for same-origin
// GETs so everything the app touched online is available offline.
//
// VERSIONING: the cache name is scoped to the assets manifest version plus a manual APP_BUILD token.
// Bumping APP_BUILD (or just shipping a new build, which changes the manifest version) yields a new
// cache; on activate the worker deletes every older "authifypass-offline-*" cache. This rebuilds the
// whole file cache on update.
//
// !!! NEVER clears IndexedDB. This worker only ever touches the Cache Storage API (caches.*). The 2FA
// codes live in IndexedDB (AddBlazorIndexedDbContext), which is a separate storage bucket; do NOT add
// indexedDB.deleteDatabase(...) or any storage-eviction call here or the user's codes are lost.
self.importScripts('./service-worker-assets.js');

const cacheNamePrefix = 'authifypass-offline-';
// APP_BUILD is injected at publish time by the InjectServiceWorkerBuildVersion target in the API
// project (it replaces the string below with the git commit count). Do not rely on editing this by
// hand: shipping a new build changes both this token and the assets manifest version, so installed
// clients detect the new worker and rebuild their cache. The literal '0' is only the committed default.
const APP_BUILD = "0";
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}_${APP_BUILD}`;
const offlineShellUrl = '/';

// Interactive WASM routes. They are not prerendered, so their server response is only the boot
// scaffold, but a navigation OR a Blazor enhanced-navigation fetch (Accept: text/html) to them must
// resolve offline too. Otherwise the worker hands Blazor a 503 response and it paints an empty error
// page instead of letting the WASM router render the page. The list mirrors the @page routes whose
// @rendermode is InteractiveWebAssembly; server-backed pages (/register, /Error) are NOT here because
// they genuinely need the API and are covered by serverOnlyPaths.
const clientRoutes = [
    '/',
    '/add',
    '/about-us'
];

// Server-rendered or API paths that must always reach the network. They never work offline anyway,
// and caching the interactive shell for them would shadow the real server output.
const serverOnlyPaths = [
    /^\/register(\/|$)/,
    /^\/user(\/|$)/,
    /^\/client(\/|$)/,
    /^\/swagger(\/|$)/,
    /^\/docs(\/|$)/,
    /^\/Error(\/|$)/,
    /^\/_framework\/debug(\/|$)/
];

// Never precache the worker files themselves (they must always be re-fetched from the server).
const assetsExclude = [
    /^service-worker\.js$/,
    /^service-worker\.published\.js$/,
    /^service-worker-assets\.js$/
];

self.addEventListener('install', event => event.waitUntil(onInstall()));
self.addEventListener('activate', event => event.waitUntil(onActivate()));
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));
self.addEventListener('message', event => {
    if (event.data === 'skipWaiting') {
        self.skipWaiting();
    }
    if (event.data && event.data.type === 'getVersion') {
        event.source && event.source.postMessage({ type: 'version', version: cacheName, build: APP_BUILD });
    }
});

async function onInstall() {
    const cache = await caches.open(cacheName);

    const assets = self.assetsManifest.assets
        .filter(asset => !assetsExclude.some(pattern => pattern.test(asset.url)));
    await Promise.all(assets.map(async asset => {
        try {
            await cache.add(new Request(asset.url, { cache: 'no-cache' }));
        }
        catch {
            // Ignore individual failures; runtime caching still covers them on first online load.
        }
    }));

    // Precache the server-rendered shell and the Blazor bootstrap script so the app can start offline.
    await Promise.allSettled([
        cache.add(new Request(offlineShellUrl, { cache: 'no-cache' })),
        cache.add(new Request('_framework/blazor.web.js', { cache: 'no-cache' }))
    ]);

    // Precache every interactive-WASM route shell (see clientRoutes) so offline navigation and Blazor
    // enhanced-navigation fetches to /add, /about-us, etc. resolve from cache instead of a 503.
    await Promise.allSettled(clientRoutes.map(route => cache.add(new Request(route, { cache: 'no-cache' }))));

    // Read the cached shell and precache exactly what it references (fingerprinted modules, css).
    await cacheShellResources(cache);

    // NOTE: every dynamic-import JS module (MyDbJS.js, ZXingBlazor BarcodeReader.razor.js and its
    // lib/*.js) is already a static web asset listed in self.assetsManifest and precached by the loop
    // above. They fail offline only because the runtime import() appends a cache-busting query string
    // (?a=<ver>&v=<date>) that made cache.match miss the base-url entry; the fetch handler now matches
    // with ignoreSearch, so no hand-maintained list is needed here.

    // Manual update: do NOT skipWaiting here. While a new build is installing it stays in the
    // "waiting" state so the PREVIOUS cache keeps serving (offline keeps working). The new build is
    // fully precached during that waiting window; only when the user taps "Reiniciar" (which posts
    // skipWaiting) does this worker activate and onActivate deletes the older cache.
      await Promise.allSettled([cache.add(new Request('_framework/blazor.boot.json', { cache: 'no-cache' }))]).catch(function(){});
}

// Reads the cached shell HTML and precaches every same-origin resource it references.
async function cacheShellResources(cache) {
    let shellText = '';

    try {
        const shell = await cache.match(offlineShellUrl);
        if (shell) {
            shellText = await shell.text();
        }
    }
    catch {
        return;
    }

    if (!shellText) {
        return;
    }

    const urls = new Set();

    const importMapMatch = shellText.match(/<script[^>]*type=["']importmap["'][^>]*>([\s\S]*?)<\/script>/i);
    if (importMapMatch) {
        try {
            const importMap = JSON.parse(importMapMatch[1]);
            for (const target of Object.values(importMap.imports || {})) {
                urls.add(target);
            }
        }
        catch { }
    }

    const refRegex = /(?:href|src)\s*=\s*["']([^"']+)["']/ig;
    let match = refRegex.exec(shellText);
    while (match !== null) {
        urls.add(match[1]);
        match = refRegex.exec(shellText);
    }

    await Promise.all(Array.from(urls).map(async (raw) => {
        try {
            const url = new URL(raw, self.location.href);
            if (url.origin !== self.location.origin || isServerOnly(url.pathname)) {
                return;
            }
            if (/^\/service-worker(\.published)?\.js$/.test(url.pathname) || url.pathname === '/service-worker-assets.js') {
                return;
            }
            const request = new Request(url.pathname + url.search, { cache: 'no-cache' });
            if (!(await cache.match(request))) {
                await cache.add(request);
            }
        }
        catch { }
    }));
}

async function onActivate() {
    // Delete every older version of the FILE cache. Only Cache Storage keys with our prefix are
    // removed; IndexedDB (the 2FA codes) is never touched.
    const cacheKeys = await caches.keys();
    await Promise.all(cacheKeys
        .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
        .map(key => caches.delete(key)));

    // Take control of open clients right away so the page can reload onto the new build.
    await self.clients.claim();
}

function isServerOnly(pathname) {
    return serverOnlyPaths.some(pattern => pattern.test(pathname));
}

async function onFetch(event) {
    const request = event.request;
    const url = new URL(request.url);
    const isSameOrigin = url.origin === self.location.origin;

    if (request.method !== 'GET' || !isSameOrigin || isServerOnly(url.pathname)) {
        return fetch(request);
    }

    const isHtmlNavigation = request.mode === 'navigate'
        || request.destination === 'document'
        || (request.headers.get('accept') || '').indexOf('text/html') >= 0;

    if (isHtmlNavigation) {
        return networkFirst(request);
    }
    return staleWhileRevalidate(request);
}

// Navigation: prefer the live server shell (fresh import map), fall back to the cached shell offline.
async function networkFirst(request) {
    const cache = await caches.open(cacheName);
    let response = null;

    try {
        response = await fetch(request);
        cache.put(request, response.clone()).catch(() => { });
    }
    catch {
        response = await cache.match(request, { ignoreSearch: true }) || await cache.match(offlineShellUrl);
        if (!response) {
            response = new Response('', { status: 503, statusText: 'Offline' });
        }
    }

    return response;
}

// Assets: serve from cache when present and refresh in the background; populate cache on first use.
async function staleWhileRevalidate(request) {
    const cache = await caches.open(cacheName);
    const cached = await cache.match(request, { ignoreSearch: true });
    const refreshed = fetch(request).then(response => {
        if (response && response.status === 200 && response.type === 'basic') {
            cache.put(request, response.clone()).catch(() => { });
        }
        return response;
    }).catch(() => null);

    let response = null;
    if (cached) {
        response = cached;
    }
    else {
        response = await refreshed;
        if (!response) {
            response = new Response('', { status: 503, statusText: 'Offline' });
        }
    }

    return response;
}
