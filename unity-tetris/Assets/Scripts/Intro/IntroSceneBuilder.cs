using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroSceneBuilder : MonoBehaviour
{
    public Sprite backgroundImage; // Drag your background image in Inspector
    public Font arcadeFont; // Optional: Assign a cooler font
    public AudioClip introMusic;


    void Start()
    {
        // === AUDIO ===
        GameObject audioGO = new GameObject("IntroMusic");
        AudioSource audioSource = audioGO.AddComponent<AudioSource>();
        audioSource.clip = introMusic;
        audioSource.loop = true;               // ✅ Looping enabled
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;
        audioSource.Play();                    // ▶️ Start playback


        // === CANVAS ===
        GameObject canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);



        // === BACKGROUND ===
        if (backgroundImage != null)
        {
            GameObject bgGO = new GameObject("Background", typeof(Image));
            bgGO.transform.SetParent(canvasGO.transform, false);

            RectTransform bgRect = bgGO.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;

            Image bgImage = bgGO.GetComponent<Image>();
            bgImage.sprite = backgroundImage;
            bgImage.preserveAspect = false; // fill full screen
            bgImage.color = Color.white;
            bgImage.rectTransform.sizeDelta = Vector2.zero;

        }

        // === DARK OVERLAY for readability ===
        GameObject overlayGO = new GameObject("Overlay", typeof(Image));
        overlayGO.transform.SetParent(canvasGO.transform, false);
        RectTransform overlayRect = overlayGO.GetComponent<RectTransform>();
        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;
        overlayRect.offsetMin = Vector2.zero;
        overlayRect.offsetMax = Vector2.zero;
        Image overlayImage = overlayGO.GetComponent<Image>();
        overlayImage.color = new Color(0f, 0f, 0f, 0.4f); // semi-transparent black


        // === TITLE ===
        GameObject titleGO = new GameObject("Title", typeof(Text));
        titleGO.transform.SetParent(canvasGO.transform, false);

        RectTransform titleRect = titleGO.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1f);
        titleRect.anchorMax = new Vector2(0.5f, 1f);
        titleRect.pivot = new Vector2(0.5f, 1f);
        titleRect.anchoredPosition = new Vector2(0, -100);
        titleRect.sizeDelta = new Vector2(800, 100);

        Text title = titleGO.GetComponent<Text>();
        title.text = "TETRIS GAME";
        title.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        title.fontSize = 60;
        title.alignment = TextAnchor.MiddleCenter;
        title.color = Color.cyan;

        // === BUTTON ===
        GameObject buttonGO = new GameObject("StartButton", typeof(Image), typeof(Button));
        buttonGO.transform.SetParent(canvasGO.transform, false);

        RectTransform btnRect = buttonGO.GetComponent<RectTransform>();
        btnRect.sizeDelta = new Vector2(300, 80);
        //btnRect.anchoredPosition = new Vector2(0, -300);
        btnRect.anchorMin = new Vector2(0.5f, 0.5f);
        btnRect.anchorMax = new Vector2(0.5f, 0.5f);
        btnRect.pivot = new Vector2(0.5f, 0.5f);
        btnRect.anchoredPosition = new Vector2(0, -50); // Small downward offset from center


        Image btnImage = buttonGO.GetComponent<Image>();
        btnImage.color = new Color(0.2f, 0.6f, 1f);

        // === BUTTON TEXT ===
        GameObject textGO = new GameObject("Text", typeof(Text));
        textGO.transform.SetParent(buttonGO.transform, false);

        RectTransform textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        Text btnText = textGO.GetComponent<Text>();
        btnText.text = "Start Game";
        btnText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        btnText.fontSize = 30;
        btnText.alignment = TextAnchor.MiddleCenter;
        btnText.color = Color.white;

        // === BUTTON FUNCTION ===

        // 1. Add Intro script to the button
        Intro introScript = buttonGO.AddComponent<Intro>();

        // 2. Hook StartGame() to the button click
        Button button = buttonGO.GetComponent<Button>();
        button.onClick.AddListener(introScript.StartGame);

        // === EVENT SYSTEM ===
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.EventSystems.StandaloneInputModule));
        }


    }
}