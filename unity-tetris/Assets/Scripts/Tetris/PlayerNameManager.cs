using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace PimDeWitte.UnityMainThreadDispatcher
{
    [System.Serializable]
    public class PlayerData
    {
        public string name;
        public int score;
    }

    [System.Serializable]
    public class PlayerDataList
    {
        public List<PlayerData> players = new List<PlayerData>();
    }

    public static class PlayerNameManager
    {
        private static string FilePath => Path.Combine(Application.persistentDataPath, "player_data.json");

        public static void SaveScore(string playerName, int score)
        {
            PlayerDataList dataList = LoadAll();

            PlayerData existing = dataList.players.Find(p => p.name == playerName);
            if (existing != null)
            {
                existing.score = Mathf.Max(existing.score, score);
            }
            else
            {
                dataList.players.Add(new PlayerData { name = playerName, score = score });
            }

            string json = JsonUtility.ToJson(dataList, true);
            File.WriteAllText(FilePath, json);
        }

        public static PlayerDataList LoadAll()
        {
            if (!File.Exists(FilePath))
                return new PlayerDataList();

            string json = File.ReadAllText(FilePath);
            return JsonUtility.FromJson<PlayerDataList>(json);
        }

        public static string LoadCurrentPlayerName()
        {
            if (!File.Exists(FilePath)) return "Guest";

            try
            {
                string json = File.ReadAllText(FilePath);
                PlayerDataList dataList = JsonUtility.FromJson<PlayerDataList>(json);
                return dataList.players.Count > 0
                    ? dataList.players[dataList.players.Count - 1].name
                    : "Guest";
            }
            catch
            {
                return "Guest";
            }
        }

        public static void SaveName(string playerName)
        {
            SaveScore(playerName, 0);
        }

        public static string LoadName()
        {
            return LoadCurrentPlayerName();
        }
    }
}
