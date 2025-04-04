using UnityEngine;
using UnityEngine.UI;

public class HighScoresButtonSpawner : MonoBehaviour
{
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

        GameObject textGO = new GameObject("Text", typeof(Text));
        textGO.transform.SetParent(buttonGO.transform, false);

        RectTransform textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        Text btnText = textGO.GetComponent<Text>();
        btnText.text = "Top 5 Scores";
        btnText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        btnText.fontSize = 24;
        btnText.alignment = TextAnchor.MiddleCenter;
        btnText.color = Color.white;

        Button button = buttonGO.GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            HighScoreDisplay.ShowTop5();
        });
    }
}
