
---

## 🧩 `TetrominoData.cs` – Tetromino Configuration

This script defines the **shape**, **appearance**, and **rotation behavior** of each Tetris block ("tetromino"). It works hand-in-hand with the `Data.cs` script and is used during piece spawning.

---

### 🔠 `Tetromino` Enum

```csharp
public enum Tetromino
{
    I, J, L, O, S, T, Z
}
```

This enum defines the 7 standard Tetris shapes:
- `I`: Line shape
- `J`: Reverse L
- `L`: L shape
- `O`: Square
- `S`: Curvy left
- `T`: T shape
- `Z`: Curvy right

---

### 📦 `TetrominoData` Struct

```csharp
[System.Serializable]
public struct TetrominoData
```

Each tetromino in the game is defined by this struct. It's serializable, meaning Unity can expose it in the Inspector (usually used in the `Board` script).

---

### 🧱 Fields

| Field | Description |
|-------|-------------|
| `Tile tile` | The visual tile used for this piece in the Tilemap. |
| `Tetromino tetromino` | Enum value to identify the piece type. |
| `Vector2Int[] cells` | Array of cell positions that define the shape of the tetromino. |
| `Vector2Int[,] wallKicks` | 2D array of vectors used for rotation wall kicks. |

> `cells` and `wallKicks` are initialized via the `Initialize()` method and pulled from the static `Data.cs`.

---

### 🧠 `Initialize()`

```csharp
public void Initialize()
{
    cells = Data.Cells[tetromino];
    wallKicks = Data.WallKicks[tetromino];
}
```

- Gets the **default shape** (cell positions) for the tetromino from `Data.Cells`.
- Gets the **wall kick rules** (used during rotation) from `Data.WallKicks`.

Called once in `Board.cs` during `Awake()`:
```csharp
for (int i = 0; i < tetrominoes.Length; i++) {
    tetrominoes[i].Initialize();
}
```

---

### 🧩 Summary

| Component      | Role |
|----------------|------|
| `Tetromino`    | Identifies the type of block |
| `tile`         | Assigns how the piece looks visually |
| `cells`        | Defines the 4-block shape of the tetromino |
| `wallKicks`    | Defines how the piece can rotate in tight spaces |
| `Initialize()` | Loads shape and kick data from the `Data.cs` source |

---

