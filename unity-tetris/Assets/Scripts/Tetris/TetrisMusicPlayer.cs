using UnityEngine;

public class TetrisMusicPlayer : MonoBehaviour
{
    public AudioClip gameMusic;
    public static AudioSource MusicSource { get; private set; }

    void Start()
    {
        GameObject audioGO = new GameObject("TetrisMusic");
        AudioSource audioSource = audioGO.AddComponent<AudioSource>();
        audioSource.clip = gameMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;
        audioSource.Play();

        Debug.Log("🎵 Tetris music started!");

        MusicSource = audioSource;
    }
}
