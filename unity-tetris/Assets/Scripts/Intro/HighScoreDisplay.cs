using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using PimDeWitte.UnityMainThreadDispatcher;


public class HighScoreDisplay : MonoBehaviour
{
    public static void ShowTop5()
    {
        var allData = PlayerNameManager.LoadAll();

        var topPlayers = allData.players
            .OrderByDescending(p => p.score)
            .Take(5)
            .ToList();

        string leaderboard = "🏆 TOP 5 PLAYERS\n";
        for (int i = 0; i < topPlayers.Count; i++)
        {
            leaderboard += $"{i + 1}. {topPlayers[i].name} - {topPlayers[i].score}\n";
        }

        // ✅ Create or reuse a Text element to show the leaderboard
        GameObject canvasGO = GameObject.Find("Canvas");
        if (canvasGO == null)
        {
            Debug.LogError("❌ No Canvas found in scene.");
            return;
        }

        // Remove old leaderboard if exists
        Transform existing = canvasGO.transform.Find("LeaderboardText");
        if (existing != null)
        {
            GameObject.Destroy(existing.gameObject);
        }

        GameObject leaderboardGO = new GameObject("LeaderboardText", typeof(Text));
        leaderboardGO.transform.SetParent(canvasGO.transform, false);

        RectTransform rect = leaderboardGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0, 200);
        rect.sizeDelta = new Vector2(600, 300);

        Text text = leaderboardGO.GetComponent<Text>();
        text.text = leaderboard;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 24;
        text.alignment = TextAnchor.UpperCenter;
        text.color = Color.yellow;

        Debug.Log("✅ Leaderboard shown on screen.");
    }
}
