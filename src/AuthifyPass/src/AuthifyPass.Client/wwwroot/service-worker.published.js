// Offline-first worker used when the app is published (development uses the no-op service-worker.js).
// This is a Blazor Web App: the HTML shell is server rendered and there is no static index.html,
// and the host shell references assets (blazor.web.js, fingerprinted css/boot modules) that are not
// listed in the client-generated assets manifest. Precaching the manifest alone is therefore not
// enough, so the worker also runs runtime caching (stale-while-revalidate) for same-origin GETs:
// after the app has been loaded once online, everything it touched is cached and the app cold
// starts fully offline with no network and no API.
self.importScripts('./service-worker-assets.js');

const cacheNamePrefix = 'authifypass-offline-';
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;
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
});

async function onInstall() {
    const cache = await caches.open(cacheName);

    // Cache each manifest asset individually so a single failing resource cannot abort the whole
    // install (which is what the atomic cache.addAll used to do and why offline silently broke).
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

    await self.skipWaiting();
}

async function onActivate() {
    const cacheKeys = await caches.keys();
    await Promise.all(cacheKeys
        .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
        .map(key => caches.delete(key)));
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
