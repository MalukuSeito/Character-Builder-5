export function exportFile(filename, byteBase64) {
    var blob = new Blob([new Uint8Array(byteBase64).buffer], { type: 'octet/stream' });

    var blobURL = URL.createObjectURL(blob);
    var a = document.createElement("a");
    document.body.appendChild(a);
    a.style = "display: none";
    a.href = blobURL;
    a.download = filename;
    a.click();
    window.URL.revokeObjectURL(blobURL);
}