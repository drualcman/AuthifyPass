let stream = null;

export async function startCamera(videoElementId) {
    const videoElement = document.getElementById(videoElementId);
    if (!videoElement) throw new Error("Video element not found");

    stream = await navigator.mediaDevices.getUserMedia({ video: true });
    videoElement.srcObject = stream;
    videoElement.play();
}

export async function stopCamera() {
    if (stream) {
        stream.getTracks().forEach(track => track.stop());
        stream = null;
    }
}

export async function captureFrame(videoElementId, canvasElementId) {
    const videoElement = document.getElementById(videoElementId);
    const canvasElement = document.getElementById(canvasElementId);
    if (!videoElement || !canvasElement) throw new Error("Elements not found");

    const context = canvasElement.getContext("2d");
    canvasElement.width = videoElement.videoWidth;
    canvasElement.height = videoElement.videoHeight;
    context.drawImage(videoElement, 0, 0, canvasElement.width, canvasElement.height);

    // Return image data as base64
    return canvasElement.toDataURL("image/png");
}