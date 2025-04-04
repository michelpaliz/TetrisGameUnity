using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using PimDeWitte.UnityMainThreadDispatcher;

public class IntroSceneBuilder : MonoBehaviour
{
    public Sprite backgroundImage;
    public AudioClip introMusic;

    // 🆕 Font assignments
    public TMP_FontAsset titleFont;     // main_title
    public TMP_FontAsset inputFont;     // simple_text
    public TMP_FontAsset buttonFont;    // title_text

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

        // === TITLE ===
        GameObject titleGO = new GameObject("Title", typeof(TextMeshProUGUI));
        titleGO.transform.SetParent(canvasGO.transform, false);
        RectTransform titleRect = titleGO.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1f);
        titleRect.anchorMax = new Vector2(0.5f, 1f);
        titleRect.pivot = new Vector2(0.5f, 1f);
        titleRect.anchoredPosition = new Vector2(0, -100);
        titleRect.sizeDelta = new Vector2(800, 100);

        TextMeshProUGUI title = titleGO.GetComponent<TextMeshProUGUI>();
        title.text = "TETRIS GAME";
        title.font = titleFont;
        title.fontSize = 60;
        title.alignment = TextAlignmentOptions.Center;
        title.color = Color.cyan;
        title.enableAutoSizing = true;

        // === INPUT FIELD ===
        GameObject inputGO = new GameObject("NameInput", typeof(Image), typeof(TMP_InputField));
        inputGO.transform.SetParent(canvasGO.transform, false);
        RectTransform inputRect = inputGO.GetComponent<RectTransform>();
        inputRect.sizeDelta = new Vector2(400, 60);
        inputRect.anchorMin = inputRect.anchorMax = new Vector2(0.5f, 0.5f);
        inputRect.pivot = new Vector2(0.5f, 0.5f);
        inputRect.anchoredPosition = new Vector2(0, 50);
        Image inputImage = inputGO.GetComponent<Image>();
        inputImage.color = Color.white;

        // Input Text
        GameObject inputTextGO = new GameObject("Text", typeof(TextMeshProUGUI));
        inputTextGO.transform.SetParent(inputGO.transform, false);
        TextMeshProUGUI inputText = inputTextGO.GetComponent<TextMeshProUGUI>();
        inputText.font = inputFont;
        inputText.color = Color.black;
        inputText.enableAutoSizing = true;
        inputText.alignment = TextAlignmentOptions.Center;
        inputText.rectTransform.anchorMin = Vector2.zero;
        inputText.rectTransform.anchorMax = Vector2.one;
        inputText.rectTransform.offsetMin = Vector2.zero;
        inputText.rectTransform.offsetMax = Vector2.zero;

        // Placeholder
        GameObject placeholderGO = new GameObject("Placeholder", typeof(TextMeshProUGUI));
        placeholderGO.transform.SetParent(inputGO.transform, false);
        TextMeshProUGUI placeholder = placeholderGO.GetComponent<TextMeshProUGUI>();
        placeholder.text = "Enter your name...";
        placeholder.font = inputFont;
        placeholder.color = Color.gray;
        placeholder.enableAutoSizing = true;
        placeholder.alignment = TextAlignmentOptions.Center;
        placeholder.rectTransform.anchorMin = Vector2.zero;
        placeholder.rectTransform.anchorMax = Vector2.one;
        placeholder.rectTransform.offsetMin = Vector2.zero;
        placeholder.rectTransform.offsetMax = Vector2.zero;

        TMP_InputField inputField = inputGO.GetComponent<TMP_InputField>();
        inputField.textComponent = inputText;
        inputField.placeholder = placeholder;
        inputField.text = PlayerNameManager.LoadName();

        // === START BUTTON ===
        GameObject buttonGO = new GameObject("StartButton", typeof(Image), typeof(Button));
        buttonGO.transform.SetParent(canvasGO.transform, false);
        RectTransform btnRect = buttonGO.GetComponent<RectTransform>();
        btnRect.sizeDelta = new Vector2(300, 80);
        btnRect.anchorMin = btnRect.anchorMax = new Vector2(0.5f, 0.5f);
        btnRect.pivot = new Vector2(0.5f, 0.5f);
        btnRect.anchoredPosition = new Vector2(0, -50);
        buttonGO.GetComponent<Image>().color = new Color(0.2f, 0.6f, 1f);

        GameObject textGO = new GameObject("Text", typeof(TextMeshProUGUI));
        textGO.transform.SetParent(buttonGO.transform, false);
        TextMeshProUGUI btnText = textGO.GetComponent<TextMeshProUGUI>();
        btnText.text = "Start Game";
        btnText.font = buttonFont;
        btnText.color = Color.white;
        btnText.alignment = TextAlignmentOptions.Center;
        btnText.enableAutoSizing = true;
        btnText.fontSizeMin = 10;
        btnText.fontSizeMax = 28;  // adjust as needed

        btnText.rectTransform.anchorMin = Vector2.zero;
        btnText.rectTransform.anchorMax = Vector2.one;
        btnText.rectTransform.offsetMin = Vector2.zero;
        btnText.rectTransform.offsetMax = Vector2.zero;

        IntroManager introScript = buttonGO.AddComponent<IntroManager>();
        Button button = buttonGO.GetComponent<Button>();
        button.onClick.AddListener(introScript.StartGame);

        // === HIGH SCORES BUTTON ===
        GameObject scoreBtnGO = new GameObject("ShowScoresButton", typeof(Image), typeof(Button));
        scoreBtnGO.transform.SetParent(canvasGO.transform, false);
        RectTransform scoreBtnRect = scoreBtnGO.GetComponent<RectTransform>();
        scoreBtnRect.sizeDelta = new Vector2(300, 80);
        scoreBtnRect.anchorMin = scoreBtnRect.anchorMax = new Vector2(0.5f, 0.5f);
        scoreBtnRect.pivot = new Vector2(0.5f, 0.5f);
        scoreBtnRect.anchoredPosition = new Vector2(0, -150);
        scoreBtnGO.GetComponent<Image>().color = new Color(1f, 0.5f, 0.1f);

        GameObject scoreTextGO = new GameObject("Text", typeof(TextMeshProUGUI));
        scoreTextGO.transform.SetParent(scoreBtnGO.transform, false);
        TextMeshProUGUI scoreText = scoreTextGO.GetComponent<TextMeshProUGUI>();
        scoreText.text = "Top Scores";
        scoreText.font = buttonFont;
        scoreText.color = Color.white;
        scoreText.alignment = TextAlignmentOptions.Center;
        //scoreText.enableAutoSizing = true;
        scoreText.enableAutoSizing = true;
        scoreText.fontSizeMin = 10;
        scoreText.fontSizeMax = 28;  // adjust as 
        scoreText.rectTransform.anchorMin = Vector2.zero;
        scoreText.rectTransform.anchorMax = Vector2.one;
        scoreText.rectTransform.offsetMin = Vector2.zero;
        scoreText.rectTransform.offsetMax = Vector2.zero;

        Button scoreButton = scoreBtnGO.GetComponent<Button>();
        scoreButton.onClick.AddListener(() => HighScoreDisplay.ShowTop5());

        // === EVENT SYSTEM ===
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.EventSystems.StandaloneInputModule));
        }
    }
}
