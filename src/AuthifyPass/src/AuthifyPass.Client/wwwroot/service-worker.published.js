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
// Bump APP_BUILD to force every installed client to rebuild its cache even if the assets version did not change.
const APP_BUILD = '2';
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}_${APP_BUILD}`;
const offlineShellUrl = '/';

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
        event.source && event.source.postMessage({ type: 'version', version: cacheName });
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

    // Activate immediately once cached: no need to wait for the tabs to close.
    await self.skipWaiting();
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

    if (request.mode === 'navigate') {
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
        response = await cache.match(request) || await cache.match(offlineShellUrl);
        if (!response) {
            response = new Response('', { status: 503, statusText: 'Offline' });
        }
    }

    return response;
}

// Assets: serve from cache when present and refresh in the background; populate cache on first use.
async function staleWhileRevalidate(request) {
    const cache = await caches.open(cacheName);
    const cached = await cache.match(request);
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
