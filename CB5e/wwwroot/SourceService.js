export function datafiles() {
    var array = Array();
    document.querySelectorAll('link[rel="data"').forEach((e) => array.push(e.href));
    return array;
}

export function loadCachedSource(dbname, store, source) {
    return new Promise((resolve, reject) => {
        var request = window.indexedDB.open(dbname, 10);
        request.onerror = function (err) {
            reject(err);
        };
        request.onupgradeneeded = function () {
            let db = request.result;
            db.createObjectStore(store, { keyPath: "name" });
        };
        request.onsuccess = function () {
            let db = request.result;
            var transaction = db.transaction(store, "readonly");
            var objectStore = transaction.objectStore(store);
            var readrequest = objectStore.openCursor(source);
            readrequest.onsuccess = function (evt) {
                var cursor = evt.target.result;

                if (cursor) {
                    resolve(cursor.value);
                } else {
                    reject("Not Found");
                }
            };
            readrequest.onerror = function (err) {
                reject(err);
            };
        };
    });
}

export function listDB(dbname, store) {
    return new Promise((resolve, reject) => {
        var request = window.indexedDB.open(dbname, 10);
        request.onerror = function (err) {
            reject(err);
        };
        request.onupgradeneeded = function () {
            let db = request.result;
            db.createObjectStore(store, { keyPath: "name" });
        };
        request.onsuccess = function () {
            let db = request.result;
            var transaction = db.transaction(store, "readonly");
            var objectStore = transaction.objectStore(store);
            var readrequest = objectStore.openKeyCursor();
            var items = [];
            readrequest.onsuccess = function (evt) {
                var cursor = evt.target.result;
                
                if (cursor) {
                    items.push(cursor.key);
                    cursor.continue();
                } else {
                    resolve(items);
                }
            };
            readrequest.onerror = function (err) {
                reject(err);
            };
        };
    });
}