// This service worker is used during development only and does nothing.
// The real offline-first worker is service-worker.published.js, used when the app is published.
self.addEventListener('install', () => self.skipWaiting());
self.addEventListener('activate', () => self.clients.claim());
self.addEventListener('fetch', () => { });
