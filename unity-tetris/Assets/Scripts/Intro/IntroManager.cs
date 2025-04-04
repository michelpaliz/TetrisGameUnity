using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PimDeWitte.UnityMainThreadDispatcher;


public class IntroManager : MonoBehaviour
{
    public void StartGame()
    {
        InputField nameField = GameObject.Find("NameInput")?.GetComponent<InputField>();
        string playerName = nameField != null ? nameField.text.Trim() : "Guest";

        if (string.IsNullOrEmpty(playerName))
            playerName = "Guest";

        PlayerNameManager.SaveName(playerName);
        SceneManager.LoadScene("Tetris");
    }
}
