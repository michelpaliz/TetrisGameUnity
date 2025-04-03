using UnityEngine;
using System.Collections;

public partial class Piece : MonoBehaviour
{
    // === Public Properties ===
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    public int rotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float moveDelay = 0.1f;
    public float lockDelay = 0.5f;

    private AudioClip lockSound;

    // === Internal Timers ===
    private float stepTime;
    private float moveTime;
    private float lockTime;

    private bool isLocked = false;

    // === Unity Lifecycle ===
    private void Start()
    {
        // Load the sound clip from Resources folder (Assets/Resources/Audio/block-lock.mp3)
        lockSound = Resources.Load<AudioClip>("Audio/block-lock");
    }

    private void Update()
    {
        if (isLocked) return;

        board.Clear(this);

        lockTime += Time.deltaTime;

        HandleInput();

        if (Time.time > moveTime)
            HandleMoveInputs();

        if (Time.time > stepTime)
            Step();

        board.Set(this);
    }

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.data = data;
        this.position = position;
        this.rotationIndex = 0;

        if (cells == null || cells.Length != data.cells.Length)
            cells = new Vector3Int[data.cells.Length];

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = (Vector3Int)data.cells[i];
        }

        stepTime = Time.time + stepDelay;
        moveTime = Time.time + moveDelay;
        lockTime = 0f;
        isLocked = false;

        Debug.Log("✅ Piece Initialized with cells: " + cells.Length);
    }


    // === Input & Movement ===
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Rotate(-1);
        else if (Input.GetKeyDown(KeyCode.E))
            Rotate(1);

        if (Input.GetKeyDown(KeyCode.Space))
            HardDrop();
    }

    private void HandleMoveInputs()
    {
        if (Input.GetKey(KeyCode.S) && Move(Vector2Int.down))
            stepTime = Time.time + stepDelay;

        if (Input.GetKey(KeyCode.A))
            Move(Vector2Int.left);
        else if (Input.GetKey(KeyCode.D))
            Move(Vector2Int.right);
    }

    private void Step()
    {
        stepTime = Time.time + stepDelay;

        Move(Vector2Int.down);

        if (lockTime >= lockDelay)
            Lock();
    }

    public void HardDrop()
    {
        while (Move(Vector2Int.down)) { }
        Lock();
    }

    // === Locking & Finalization ===
    private void Lock()
    {
        if (isLocked) return;
        isLocked = true;

        if (lockSound)
            AudioSource.PlayClipAtPoint(lockSound, Camera.main.transform.position, 1f);

        board.Set(this);
        board.ClearLines();
        board.SpawnPiece();
    }

    // === Movement Core ===
    public bool Move(Vector2Int translation)
    {
        if (isLocked) return false;

        Vector3Int newPosition = position + new Vector3Int(translation.x, translation.y, 0);
        bool valid = board.IsValidPosition(this, newPosition);

        if (valid)
        {
            position = newPosition;
            moveTime = Time.time + moveDelay;
            lockTime = 0f;
        }

        return valid;
    }

    // === Optional: Music Fade on Game Over ===
    private IEnumerator FadeOutMusic()
    {
        if (TetrisMusicPlayer.MusicSource == null) yield break;

        float duration = 1.0f;
        float startVolume = TetrisMusicPlayer.MusicSource.volume;

        while (TetrisMusicPlayer.MusicSource.volume > 0f)
        {
            TetrisMusicPlayer.MusicSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        TetrisMusicPlayer.MusicSource.Stop();
        TetrisMusicPlayer.MusicSource.volume = startVolume;
    }
}