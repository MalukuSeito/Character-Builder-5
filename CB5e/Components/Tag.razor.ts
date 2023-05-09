export function Display(data: string, stylesheet: string, x: number, y: number) {
    const xsltProcessor = new XSLTProcessor();
    const myXMLHTTPRequest = new XMLHttpRequest();
    myXMLHTTPRequest.onload = function () {
        const xslRef = myXMLHTTPRequest.responseXML;

        // Finally import the .xsl
        xsltProcessor.importStylesheet(xslRef);
        const parser = new DOMParser();
        const doc = parser.parseFromString(data, "text/xml");
        const resultDoc = xsltProcessor.transformToDocument(doc);
        const iframeDoc = window.frames["display"].document;

        iframeDoc.open();
        iframeDoc.write(new XMLSerializer().serializeToString(resultDoc));
        iframeDoc.close();
        var popup = document.getElementById("popup");
        popup.style.display = "block";
        popup.style.left = "min(" + x + "px, calc(100vw - 410px + " + window.scrollX + "px ))";
        popup.style.top = "min(" + y + "px, calc(100vh - 51vh + " + window.scrollY + "px))";
        popup.onclick = Hide;
    }
    myXMLHTTPRequest.open("GET", "xsl/" + stylesheet + ".xsl", true);
    myXMLHTTPRequest.send(null);
}

function Hide() {
    document.getElementById("popup").style.display = "none";
}