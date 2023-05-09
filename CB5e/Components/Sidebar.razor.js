export function createModal(element, backdrop, scroll) {
    return new bootstrap.Offcanvas(element, { "backdrop": backdrop, "scroll": scroll });
};