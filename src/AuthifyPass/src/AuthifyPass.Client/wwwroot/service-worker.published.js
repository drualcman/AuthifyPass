self.importScripts('./service-worker-assets.js');

const cacheNamePrefix = 'authifypass-offline-';
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;
const offlineShellUrl = 'index.html';

// Scoped css bundles are referenced by index.html through their stable (non fingerprinted)
// route, so they have to be cached under that exact url to be found while offline.
const extraAssets = [
    '_content/AuthifyPass.Views/AuthifyPass.Views.bundle.scp.css',
    '_content/ZXingBlazor/ZXingBlazor.bundle.scp.css'
];

// Everything else the published app produces is cached, so the app runs with no network at all.
// Only the server side paths below are left to the network: they never work offline anyway.
const networkOnlyPaths = [
    /^\/client(\/|$)/,
    /^\/user(\/|$)/,
    /^\/swagger(\/|$)/,
    /^\/register(\/|$)/,
    /^\/Error(\/|$)/,
    /^\/docs(\/|$)/
];

// The host web application bundles the scoped css itself, so the per project bundle
// listed in the assets manifest is not published and must not be requested.
const assetsExclude = [
    /^service-worker\.js$/,
    /^service-worker\.published\.js$/,
    /\.styles\.css$/
];

self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));
self.addEventListener('message', event => {
    if (event.data === 'skipWaiting') {
        self.skipWaiting();
    }
});

async function onInstall(event) {
    const cache = await caches.open(cacheName);
    const assetsRequests = self.assetsManifest.assets
        .filter(asset => !assetsExclude.some(pattern => pattern.test(asset.url)))
        .map(asset => new Request(asset.url, { integrity: asset.hash, cache: 'no-cache' }));
    await cache.addAll(assetsRequests);
    await Promise.allSettled(extraAssets.map(url => cache.add(new Request(url, { cache: 'no-cache' }))));
    await self.skipWaiting();
}

async function onActivate(event) {
    const cacheKeys = await caches.keys();
    await Promise.all(cacheKeys
        .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
        .map(key => caches.delete(key)));
    await self.clients.claim();
}

async function onFetch(event) {
    const request = event.request;
    const url = new URL(request.url);
    const isSameOrigin = url.origin === self.location.origin;
    const isNetworkOnly = isSameOrigin && networkOnlyPaths.some(pattern => pattern.test(url.pathname));
    let response = null;

    if (request.method !== 'GET' || !isSameOrigin || isNetworkOnly) {
        response = await fetchFromNetwork(request);
    }
    else if (request.mode === 'navigate') {
        response = await matchInCache(offlineShellUrl) ?? await fetchFromNetwork(request);
    }
    else {
        response = await matchInCache(request) ?? await fetchFromNetwork(request);
    }

    return response;
}

async function matchInCache(requestOrUrl) {
    const cache = await caches.open(cacheName);
    return await cache.match(requestOrUrl);
}

async function fetchFromNetwork(request) {
    let response = null;
    try {
        response = await fetch(request);
    }
    catch {
        response = new Response('', { status: 503, statusText: 'Offline' });
    }
    return response;
}
