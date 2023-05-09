export function doOnInput(element) {
    element.oninput();
};

const resizeObserver = new ResizeObserver((entries) => {
    entries.forEach(s => s.target.oninput());
});

function input() {
    this.style.height = 'auto';
    this.style.height = (this.scrollHeight) + 'px';
}

export function connect(element) {
    element.oninput = input;
    resizeObserver.observe(element);
}

export function disconnect(element) {
    resizeObserver.unobserve(element);
}