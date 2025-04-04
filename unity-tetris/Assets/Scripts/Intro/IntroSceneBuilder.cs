using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PimDeWitte.UnityMainThreadDispatcher;


public class IntroSceneBuilder : MonoBehaviour
{
    public Sprite backgroundImage;
    public Font arcadeFont;
    public AudioClip introMusic;


    void Start()
    {
        // === AUDIO ===
        GameObject audioGO = new GameObject("IntroMusic");
        AudioSource audioSource = audioGO.AddComponent<AudioSource>();
        audioSource.clip = introMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;
        audioSource.Play();

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
            bgImage.preserveAspect = false;
            bgImage.color = Color.white;
        }

        // === OVERLAY ===
        GameObject overlayGO = new GameObject("Overlay", typeof(Image));
        overlayGO.transform.SetParent(canvasGO.transform, false);
        RectTransform overlayRect = overlayGO.GetComponent<RectTransform>();
        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;
        overlayRect.offsetMin = Vector2.zero;
        overlayRect.offsetMax = Vector2.zero;
        overlayGO.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.4f);

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
        title.font = arcadeFont ?? Resources.GetBuiltinResource<Font>("Arial.ttf");
        title.fontSize = 60;
        title.alignment = TextAnchor.MiddleCenter;
        title.color = Color.cyan;

        // === INPUT FIELD ===
        GameObject inputGO = new GameObject("NameInput", typeof(Image), typeof(InputField));
        inputGO.transform.SetParent(canvasGO.transform, false);
        RectTransform inputRect = inputGO.GetComponent<RectTransform>();
        inputRect.sizeDelta = new Vector2(400, 60);
        inputRect.anchorMin = new Vector2(0.5f, 0.5f);
        inputRect.anchorMax = new Vector2(0.5f, 0.5f);
        inputRect.pivot = new Vector2(0.5f, 0.5f);
        inputRect.anchoredPosition = new Vector2(0, 50);
        InputField inputField = inputGO.GetComponent<InputField>();
        Image inputImage = inputGO.GetComponent<Image>();
        inputImage.color = Color.white;

        GameObject placeholderGO = new GameObject("Placeholder", typeof(Text));
        placeholderGO.transform.SetParent(inputGO.transform, false);
        Text placeholder = placeholderGO.GetComponent<Text>();
        placeholder.text = "Enter your name...";
        placeholder.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        placeholder.fontSize = 20;
        placeholder.color = Color.gray;
        placeholder.alignment = TextAnchor.MiddleCenter;
        placeholder.rectTransform.anchorMin = Vector2.zero;
        placeholder.rectTransform.anchorMax = Vector2.one;
        placeholder.rectTransform.offsetMin = Vector2.zero;
        placeholder.rectTransform.offsetMax = Vector2.zero;

        GameObject inputTextGO = new GameObject("Text", typeof(Text));
        inputTextGO.transform.SetParent(inputGO.transform, false);
        Text inputText = inputTextGO.GetComponent<Text>();
        inputText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        inputText.fontSize = 24;
        inputText.color = Color.black;
        inputText.alignment = TextAnchor.MiddleCenter;
        inputText.rectTransform.anchorMin = Vector2.zero;
        inputText.rectTransform.anchorMax = Vector2.one;
        inputText.rectTransform.offsetMin = Vector2.zero;
        inputText.rectTransform.offsetMax = Vector2.zero;

        inputField.textComponent = inputText;
        inputField.placeholder = placeholder;

        inputField.text = PlayerNameManager.LoadName(); // Pre-fill if exists

        // === START BUTTON ===
        GameObject buttonGO = new GameObject("StartButton", typeof(Image), typeof(Button));
        buttonGO.transform.SetParent(canvasGO.transform, false);
        RectTransform btnRect = buttonGO.GetComponent<RectTransform>();
        btnRect.sizeDelta = new Vector2(300, 80);
        btnRect.anchorMin = new Vector2(0.5f, 0.5f);
        btnRect.anchorMax = new Vector2(0.5f, 0.5f);
        btnRect.pivot = new Vector2(0.5f, 0.5f);
        btnRect.anchoredPosition = new Vector2(0, -50);
        buttonGO.GetComponent<Image>().color = new Color(0.2f, 0.6f, 1f);

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

        // === BUTTON LOGIC ===
        IntroManager introScript = buttonGO.AddComponent<IntroManager>();
        Button button = buttonGO.GetComponent<Button>();
        button.onClick.AddListener(introScript.StartGame);

        // === SHOW SCORES BUTTON ===
        GameObject scoreButtonGO = new GameObject("HighScoreDisplay", typeof(Image), typeof(Button));
        scoreButtonGO.transform.SetParent(canvasGO.transform, false);

        RectTransform scoreBtnRect = scoreButtonGO.GetComponent<RectTransform>();
        scoreBtnRect.sizeDelta = new Vector2(300, 80);
        scoreBtnRect.anchorMin = new Vector2(0.5f, 0.5f);
        scoreBtnRect.anchorMax = new Vector2(0.5f, 0.5f);
        scoreBtnRect.pivot = new Vector2(0.5f, 0.5f);
        scoreBtnRect.anchoredPosition = new Vector2(0, -150); // Offset lower than Start button

        Image scoreBtnImage = scoreButtonGO.GetComponent<Image>();
        scoreBtnImage.color = new Color(1f, 0.6f, 0.2f); // Orange-ish

        GameObject scoreTextGO = new GameObject("Text", typeof(Text));
        scoreTextGO.transform.SetParent(scoreButtonGO.transform, false);
        RectTransform scoreTextRect = scoreTextGO.GetComponent<RectTransform>();
        scoreTextRect.anchorMin = Vector2.zero;
        scoreTextRect.anchorMax = Vector2.one;
        scoreTextRect.offsetMin = Vector2.zero;
        scoreTextRect.offsetMax = Vector2.zero;

        Text scoreBtnText = scoreTextGO.GetComponent<Text>();
        scoreBtnText.text = "Show Top Scores";
        scoreBtnText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        scoreBtnText.fontSize = 26;
        scoreBtnText.alignment = TextAnchor.MiddleCenter;
        scoreBtnText.color = Color.white;

        // === BUTTON FUNCTION ===
        Button scoreButton = scoreButtonGO.GetComponent<Button>();
        scoreButton.onClick.AddListener(() => HighScoreDisplay.ShowTop5());

        // === EVENT SYSTEM ===
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.EventSystems.StandaloneInputModule));
        }
    }
}
