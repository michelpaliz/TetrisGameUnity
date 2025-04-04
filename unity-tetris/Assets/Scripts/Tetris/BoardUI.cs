using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class Board : MonoBehaviour
{
    private GameObject canvasGO;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI gameOverText;
    private TextMeshProUGUI returnText;

    // 🆕 Assign these in the Inspector or load dynamically if needed
    public TMP_FontAsset scoreFont;
    public TMP_FontAsset gameOverFont;

    private void CreateScoreUI()
    {
        canvasGO = new GameObject("ScoreCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // === Score Text ===
        GameObject textGO = new GameObject("ScoreText", typeof(TextMeshProUGUI));
        textGO.transform.SetParent(canvasGO.transform, false);
        RectTransform rect = textGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 1f);
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = new Vector2(0, -60); // Below top center
        rect.sizeDelta = new Vector2(800, 100);

        scoreText = textGO.GetComponent<TextMeshProUGUI>();
        scoreText.font = scoreFont;
        scoreText.fontSize = 48;                      // 🆙 Bigger
        scoreText.alignment = TextAlignmentOptions.Center;
        scoreText.color = Color.black;                // 🖤 Dark font
        scoreText.text = $"Score: {score}";

        CreateGameOverText();
    }

    private void CreateGameOverText()
    {
        GameObject textGO = new GameObject("GameOverText", typeof(TextMeshProUGUI));
        textGO.transform.SetParent(canvasGO.transform, false);

        RectTransform rect = textGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(600, 150);

        gameOverText = textGO.GetComponent<TextMeshProUGUI>();
        gameOverText.font = gameOverFont; // 🔥 drag `main_title` here
        gameOverText.fontSize = 64;
        gameOverText.alignment = TextAlignmentOptions.Center;
        gameOverText.color = Color.red;
        gameOverText.text = "GAME OVER";
        gameOverText.enabled = false;
    }


    private void ShowReturnMessage()
    {
        GameObject textGO = new GameObject("ReturnMessage", typeof(TextMeshProUGUI));
        textGO.transform.SetParent(canvasGO.transform, false);

        RectTransform rect = textGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0, -100);  // ⬇ slightly under "GAME OVER"
        rect.sizeDelta = new Vector2(800, 100);

        returnText = textGO.GetComponent<TextMeshProUGUI>();
        returnText.font = gameOverFont; // 👈 same style as game over
        returnText.fontSize = 36;
        returnText.alignment = TextAlignmentOptions.Center;
        returnText.color = Color.yellow;
        returnText.text = "Returning to Intro...";
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }
}
