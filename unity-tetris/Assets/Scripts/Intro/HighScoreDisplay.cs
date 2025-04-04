using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using PimDeWitte.UnityMainThreadDispatcher;

public class HighScoreDisplay : MonoBehaviour
{
    private static GameObject currentPanel;

    public static void ShowTop5()
    {
        var allData = PlayerNameManager.LoadAll();

        var topPlayers = allData.players
            .OrderByDescending(p => p.score)
            .Take(5)
            .ToList();

        GameObject canvasGO = GameObject.Find("Canvas");
        if (canvasGO == null)
        {
            Debug.LogError("❌ No Canvas found in scene.");
            return;
        }

        // Destroy previous panel
        if (currentPanel != null)
        {
            GameObject.Destroy(currentPanel);
        }

        // === Panel ===
        GameObject panelGO = new GameObject("ScorePanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        panelGO.transform.SetParent(canvasGO.transform, false);
        Image panelImage = panelGO.GetComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.75f); // semi-transparent

        RectTransform panelRect = panelGO.GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(500, 300);
        panelRect.anchorMin = panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;

        currentPanel = panelGO; // Track reference

        // === TMP Text ===
        GameObject textGO = new GameObject("TopScoresText", typeof(TextMeshProUGUI));
        textGO.transform.SetParent(panelGO.transform, false);
        TextMeshProUGUI tmpText = textGO.GetComponent<TextMeshProUGUI>();

        RectTransform textRect = tmpText.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(20, 20);
        textRect.offsetMax = new Vector2(-20, -20);

        tmpText.fontSize = 32;
        tmpText.alignment = TextAlignmentOptions.Center;
        tmpText.color = Color.yellow;
        tmpText.enableWordWrapping = true;

        string leaderboard = "<b>🏆 TOP 5 PLAYERS</b>\n\n";
        for (int i = 0; i < topPlayers.Count; i++)
        {
            leaderboard += $"{i + 1}. {topPlayers[i].name} - {topPlayers[i].score}\n";
        }

        tmpText.text = leaderboard;

        // Add ESC key listener
        panelGO.AddComponent<CloseOnEsc>();
    }
}
