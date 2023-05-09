export function createModal(element) {
    return new bootstrap.Modal(element);
};

export function autofocus(modal, element) {
    modal.addEventListener('shown.bs.modal', function () {
        element.focus()
    })
}