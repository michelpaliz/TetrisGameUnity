using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScoresButtonSpawner : MonoBehaviour
{
    public TMP_FontAsset buttonFont; // 🎯 Drag simple_text, etc.

    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (!canvas) return;

        GameObject buttonGO = new GameObject("HighScoresButton", typeof(Button), typeof(Image));
        buttonGO.transform.SetParent(canvas.transform, false);

        RectTransform rect = buttonGO.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 60);
        rect.anchorMin = new Vector2(1f, 1f);  // top-right
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(1f, 1f);
        rect.anchoredPosition = new Vector2(-20, -20);

        buttonGO.GetComponent<Image>().color = new Color(0.2f, 0.6f, 1f);

        // === TMP Text ===
        GameObject textGO = new GameObject("Text", typeof(TextMeshProUGUI));
        textGO.transform.SetParent(buttonGO.transform, false);

        RectTransform textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        TextMeshProUGUI btnText = textGO.GetComponent<TextMeshProUGUI>();
        btnText.text = "Top 5 Scores";
        btnText.font = buttonFont;  // 🎯 Use your assigned TMP font
        btnText.fontSize = 24;
        btnText.alignment = TextAlignmentOptions.Center;
        btnText.color = Color.white;
        btnText.enableAutoSizing = true;

        Button button = buttonGO.GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            HighScoreDisplay.ShowTop5();
        });
    }
}
