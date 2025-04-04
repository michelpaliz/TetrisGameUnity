using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using PimDeWitte.UnityMainThreadDispatcher;



[DefaultExecutionOrder(-1)]
public partial class Board : MonoBehaviour
{
    // === Public Config ===
    public TetrominoData[] tetrominoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);
    public GameObject piecePrefab;

    // === State ===
    public int score = 0;
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    private AudioClip lineClearSound;

    // GAME OVER TRIGGER
    private bool isGameOver = false;
    public bool IsGameOver => isGameOver;

    public AudioClip gameOverSound;


    // === Bounds Property ===
    public RectInt Bounds => new RectInt(new Vector2Int(-boardSize.x / 2, -boardSize.y / 2), boardSize);


    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        piecePrefab = Resources.Load<GameObject>("Prefabs/Piece");

        if (piecePrefab == null)
        {
            Debug.LogError("❌ Could not load Piece prefab from Resources/Prefabs/Piece!");
            return;
        }

        Dictionary<Tetromino, string> tileNames = new Dictionary<Tetromino, string>
        {
            { Tetromino.I, "Cyan" },
            { Tetromino.J, "Blue" },
            { Tetromino.L, "Orange" },
            { Tetromino.O, "Yellow" },
            { Tetromino.S, "Green" },
            { Tetromino.T, "Purple" },
            { Tetromino.Z, "Red" }
        };

        tetrominoes = new TetrominoData[tileNames.Count];
        int index = 0;

        foreach (var kvp in tileNames)
        {
            Tile tile = Resources.Load<Tile>($"Tiles/{kvp.Value}");
            if (tile == null) continue;

            TetrominoData data = new TetrominoData
            {
                tetromino = kvp.Key,
                tile = tile
            };

            data.Initialize();
            tetrominoes[index++] = data;
        }
    }

    private void Start()
    {
        if (tetrominoes == null || tetrominoes.Length == 0)
        {
            enabled = false;
            return;
        }

        lineClearSound = Resources.Load<AudioClip>("Audio/line-clear");
        gameOverSound = Resources.Load<AudioClip>("Audio/game-over");
        CreateScoreUI();   // From BoardUI.cs
        SpawnPiece();      // Start game with first piece
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];

        GameObject pieceObject = Instantiate(piecePrefab, transform);

        activePiece = pieceObject.GetComponent<Piece>();
        activePiece.Initialize(this, spawnPosition, data);

        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
        }
        else
        {
            GameOver();  // Stop game if piece can't spawn
        }
    }

    public void GameOver()
    {
        //// Save score
       
        string playerName = PlayerNameManager.LoadName();
        PlayerNameManager.SaveScore(playerName, score);


        // Stop the game by setting the time scale to 0
        Time.timeScale = 0;

        // Clear the board and display Game Over message
        tilemap.ClearAllTiles();

        if (gameOverText != null)
        {
            gameOverText.enabled = true;  // Show Game Over UI text
        }

        // Play Game Over sound
        if (gameOverSound != null)
        {
            AudioSource.PlayClipAtPoint(gameOverSound, Camera.main.transform.position, 1f);
        }

        // Stop listening for gestures when the game is over
        GestureListener gestureListener = FindObjectOfType<GestureListener>();
        if (gestureListener != null)
        {
            gestureListener.StopListening();
        }

        Debug.Log("Game Over! Press 'R' to restart.");
        isGameOver = true;  // Mark the game as over
    }


    public void Restart()
    {
        // Reset time scale to 1 to resume the game
        Time.timeScale = 1;

        // Reload the current scene to reset the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Restart gesture listening
        GestureListener gestureListener = FindObjectOfType<GestureListener>();
        if (gestureListener != null)
        {
            gestureListener.StartListening();  // Assuming you have a StartListening() method
        }
    }

    public void Set(Piece piece)
    {
        foreach (var cell in piece.cells)
        {
            tilemap.SetTile(cell + piece.position, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        if (piece == null || piece.cells == null || tilemap == null) return;

        foreach (var cell in piece.cells)
        {
            tilemap.SetTile(cell + piece.position, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        foreach (var cell in piece.cells)
        {
            Vector3Int tilePosition = cell + position;

            if (!bounds.Contains((Vector2Int)tilePosition) || tilemap.HasTile(tilePosition)) return false;
        }

        return true;
    }

    // Update is used to listen for user input for restarting the game
    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            Restart();  // Restart the game when 'R' is pressed
        }
    }
}
