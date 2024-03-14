export function Display(data, stylesheet, x, y) {
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
    };
    myXMLHTTPRequest.open("GET", "xsl/" + stylesheet + ".xsl", true);
    myXMLHTTPRequest.send(null);
}
function setContent(popup, reference) {
    const xsltProcessor = new XSLTProcessor();
    const myXMLHTTPRequest = new XMLHttpRequest();
    var result = "Loading...";
    myXMLHTTPRequest.onload = function () {
        const xslRef = myXMLHTTPRequest.responseXML;
        // Finally import the .xsl
        xsltProcessor.importStylesheet(xslRef);
        const parser = new DOMParser();
        const doc = parser.parseFromString(reference.invokeMethod('GetContent'), "text/xml");
        const resultDoc = xsltProcessor.transformToDocument(doc);
        //reference.invokeMethod('GetPopup').setContent(new XMLSerializer().serializeToString(resultDoc));
        result = new XMLSerializer().serializeToString(resultDoc);
    };
    myXMLHTTPRequest.open("GET", "xsl/" + reference.invokeMethod('GetStylesheet') + ".xsl", false);
    myXMLHTTPRequest.send(null);
    return result;
}
function Hide() {
    document.getElementById("popup").style.display = "none";
}
export function bootstrapPopover(element, reference) {
    var popup = new bootstrap.Popover(element, {
        "content": () => setContent(element, reference),
        "sanitize": true,
        "template": '<div class="popover" role="tooltip"><div class="popover-close float-end"><div class="btn-close me-1 mt-1" aria-label="Close"></div></div><div class="popover-arrow"></div><h3 class="popover-header"></h3><div class="popover-body"></div></div>'
    });
    element.onclick = function (event) {
        event.stopPropagation();
    };
    element.addEventListener('inserted.bs.popover', () => {
        if (popup.tip) {
            popup.tip.getElementsByClassName("btn-close")[0].onclick = function (e) {
                popup.hide();
            };
        }
    });
    return popup;
}
const myDefaultAllowList = bootstrap.Tooltip.Default.allowList;
myDefaultAllowList.center = [];
myDefaultAllowList['*'].push('style');
//# sourceMappingURL=Tag.razor.js.map