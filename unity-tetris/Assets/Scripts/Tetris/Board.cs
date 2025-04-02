using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[DefaultExecutionOrder(-1)]
public partial class Board : MonoBehaviour
{
    // === Public Config ===
    public TetrominoData[] tetrominoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);
    public GameObject piecePrefab;

    private Transform pieceContainer;


    // === State ===
    public int score = 0;
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    private AudioClip lineClearSound;

    // === Bounds Property ===
    public RectInt Bounds => new RectInt(new Vector2Int(-boardSize.x / 2, -boardSize.y / 2), boardSize);

    private void Awake()
    {
        // 🎯 Get Tilemap from children
        tilemap = GetComponentInChildren<Tilemap>();

        // 🧱 Load Piece prefab from Resources
        piecePrefab = Resources.Load<GameObject>("Prefabs/Piece");
        if (piecePrefab == null) return;

        // 🎨 Auto-load Tetromino Tiles from Resources
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
        CreateScoreUI();   // From BoardUI.cs
        SpawnPiece();      // Start game with first piece
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];

        //GameObject pieceObject = Instantiate(piecePrefab, transform);

        // 👇 This keeps the hierarchy clean
        tilemap = GetComponentInChildren<Tilemap>();
        GameObject pieceObject = Instantiate(piecePrefab);

        activePiece = pieceObject.GetComponent<Piece>();
        activePiece.Initialize(this, spawnPosition, data);

        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
        }
        else
        {
            GameOver();
        }

        // Optional gesture integration
        GestureListener gestureListener = FindObjectOfType<GestureListener>();
        if (gestureListener != null)
        {
            gestureListener.playerPiece = activePiece;
        }
    }

    public void GameOver()
    {
        tilemap.ClearAllTiles();

        if (gameOverText != null)
        {
            gameOverText.enabled = true;
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

            if (!bounds.Contains((Vector2Int)tilePosition)) return false;
            if (tilemap.HasTile(tilePosition)) return false;
        }

        return true;
    }
}
