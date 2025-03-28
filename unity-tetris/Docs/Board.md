
---

## 🎮 `Board.cs` – The Brain of the Tetris Grid

This script manages the **game board** (tilemap), spawns and places Tetris pieces, detects line clears, and communicates with the `GestureListener`. It's a core part of your game logic.

---

### 📦 `public class Board : MonoBehaviour`

A MonoBehaviour script attached to the **Board GameObject**. It coordinates the Tetris grid and active falling piece.

---

### 🧩 Key Fields & Properties

| Field | Purpose |
|-------|---------|
| `Tilemap tilemap` | The tilemap component that visually represents the board |
| `Piece activePiece` | The currently falling tetromino |
| `TetrominoData[] tetrominoes` | All available Tetris piece types |
| `Vector2Int boardSize` | The size of the board (10x20 grid) |
| `Vector3Int spawnPosition` | Where new pieces spawn |
| `GameObject piecePrefab` | Prefab used to spawn new pieces |

#### 🟩 `Bounds` property
Calculates the rectangular boundaries of the board based on its size, used for collision detection.

```csharp
RectInt Bounds => new RectInt(new Vector2Int(-5, -10), new Vector2Int(10, 20));
```

---

### 🧠 `Awake()`

```csharp
tilemap = GetComponentInChildren<Tilemap>();
activePiece = GetComponentInChildren<Piece>();
```

- Caches references to the `Tilemap` and existing `Piece`.
- Initializes each `TetrominoData` (sets rotation data, colors, etc.).

---

### 🟢 `Start()`

```csharp
SpawnPiece();
```

Starts the game by dropping the first piece into the board.

---

### 🧱 `SpawnPiece()`

This is where a new random Tetris piece is instantiated and dropped into the playfield.

Steps:
1. Picks a random tetromino from the list.
2. Instantiates the piece prefab.
3. Initializes it with position and shape.
4. If it's valid, it sets it into the tilemap.
5. If not valid, calls `GameOver()`.

🔁 Syncs the new piece with `GestureListener`:
```csharp
gestureListener.playerPiece = activePiece;
```

---

### 💥 `GameOver()`

```csharp
tilemap.ClearAllTiles();
```

Simple game over logic for now — clears the whole board. You could expand this with UI or sounds.

---

### 🎨 `Set(Piece piece)`

Draws the active piece into the tilemap by assigning each of its cell positions a tile.

---

### 🧹 `Clear(Piece piece)`

Removes the current piece’s tiles from the tilemap. Typically called before moving or rotating to avoid duplication.

---

### ✅ `IsValidPosition(Piece piece, Vector3Int position)`

Checks whether the piece is within the board boundaries and not overlapping other tiles. Used before applying moves.

---

### 🔄 Line Clear Logic

#### `ClearLines()`
- Checks every row.
- If a row is full, clears it and shifts everything above it down.

#### `IsLineFull(int row)`
Checks if every column in a row is occupied.

#### `LineClear(int row)`
- Clears the full row.
- Shifts all rows above it down by one.

---

### 🎯 Summary

| System | Handles |
|--------|---------|
| Tilemap | Drawing pieces and cleared lines |
| Spawning | Piece creation and positioning |
| Collision | Validity checking of moves |
| Gesture Sync | Connects to external hand input |
| Line Logic | Full row detection and clearing |

---
