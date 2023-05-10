export function datafiles() {
    var array = Array();
    document.querySelectorAll('link[rel="data"').forEach((e) => array.push(e.href));
    return array;
}