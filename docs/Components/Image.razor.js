export function blob(image) {
    var blob = new Blob([new Uint8Array(image).buffer], { type: 'image/png' });
    var blobURL = URL.createObjectURL(blob);
    return blobURL;
};

export function release(url) {
    window.URL.revokeObjectURL(url);
};