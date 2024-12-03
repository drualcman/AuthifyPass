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
    return new Promise((resolve, reject) => {
        try {
            const videoElement = document.getElementById(videoElementId);
            const canvasElement = document.getElementById(canvasElementId);

            if (!videoElement || !canvasElement) {
                reject("Video or canvas element not found");
            }
            else {
                const context = canvasElement.getContext("2d");
                canvasElement.width = videoElement.videoWidth;
                canvasElement.height = videoElement.videoHeight;
                context.drawImage(videoElement, 0, 0, canvasElement.width, canvasElement.height);

                const image = canvasElement.toDataURL("image/png");
                console.log('---- JS IMAGE ----');
                console.log(image);
                console.log('------------------');

                resolve(image); // Resolución exitosa de la promesa
            }
        } catch (error) {
            console.error("Error capturing frame:", error);
            reject(error); // Rechaza la promesa si ocurre un error
        }
    });
}