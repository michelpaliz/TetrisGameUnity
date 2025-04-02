using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("🟢 [Intro] StartGame() called.");
        Debug.Log("📦 Attempting to load scene: Tetris");

        SceneManager.LoadScene("Tetris");
    }
}
