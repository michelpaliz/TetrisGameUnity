using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoBackground : MonoBehaviour
{
    public string videoFileName = "video.mp4";

    void Start()
    {
        // === VIDEO PLAYER ===
        var videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = true;

        //videoPlayer.url = System.IO.Path.Combine(Application.dataPath, "video.mp4");
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath + "/video.mp4");
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = new RenderTexture(Screen.width, Screen.height, 0);

        // === BACKGROUND CANVAS ===
        GameObject canvasGO = new GameObject("VideoCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main; // ✅ Set main camera
        canvas.sortingOrder = -100; // ✅ Behind everything else

        CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // === RAW IMAGE TO DISPLAY VIDEO ===
        GameObject rawGO = new GameObject("VideoDisplay", typeof(RawImage));
        rawGO.transform.SetParent(canvasGO.transform, false);
        RawImage rawImage = rawGO.GetComponent<RawImage>();
        rawImage.texture = videoPlayer.targetTexture;

        RectTransform rect = rawGO.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        // === PLAY VIDEO ===
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += (vp) => vp.Play();

        // Optional error logging
        videoPlayer.errorReceived += (vp, msg) =>
        {
            Debug.LogError("📛 Video Error: " + msg);
        };
    }
}
