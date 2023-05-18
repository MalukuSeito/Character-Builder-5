export function initLaunchQueue(callback) {
    if ('launchQueue' in window) {
        window.launchQueue.setConsumer(async params => {
            const [handle] = params.files;
            if (handle) {
                const file = await handle.getFile();
                await callback.invokeMethodAsync("LoadFile", await file.text(), file.name);
            }
        });
    }
}