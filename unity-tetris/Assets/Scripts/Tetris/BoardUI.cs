using UnityEngine;
using UnityEngine.UI;

public partial class Board : MonoBehaviour
{
    private GameObject canvasGO;
    private Text scoreText;
    private Text gameOverText;

    private void CreateScoreUI()
    {
        canvasGO = new GameObject("ScoreCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Score Text
        GameObject textGO = new GameObject("ScoreText", typeof(Text));
        textGO.transform.SetParent(canvasGO.transform, false);
        RectTransform rect = textGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(1, 1);
        rect.anchoredPosition = new Vector2(-50, -50);
        rect.sizeDelta = new Vector2(300, 60);

        scoreText = textGO.GetComponent<Text>();
        scoreText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        scoreText.fontSize = 36;
        scoreText.alignment = TextAnchor.MiddleRight;
        scoreText.color = Color.white;
        scoreText.text = $"Score: {score}";

        // Game Over Text
        CreateGameOverText();
    }

    private void CreateGameOverText()
    {
        GameObject textGO = new GameObject("GameOverText", typeof(Text));
        textGO.transform.SetParent(canvasGO.transform, false);

        RectTransform rect = textGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(600, 150);

        gameOverText = textGO.GetComponent<Text>();
        gameOverText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        gameOverText.fontSize = 64;
        gameOverText.alignment = TextAnchor.MiddleCenter;
        gameOverText.color = Color.red;
        gameOverText.text = "GAME OVER";
        gameOverText.enabled = false;
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }
}
