
---

## 👻 `Ghost.cs` – Visual Drop Preview for Tetris

The **Ghost piece** is a translucent preview of the currently falling Tetris piece. It appears below the active piece and shows exactly where it will land if dropped straight down (hard drop). This improves gameplay by helping players anticipate placements.

---

### 🎯 Purpose

- Track the current **active Tetris piece** (`trackingPiece`).
- Calculate where it would land with a hard drop.
- Display a ghost outline using a separate `Tilemap`.

---

### 🧱 Fields

| Variable | Description |
|---------|-------------|
| `tile` | The tile to use for ghost rendering. Usually a transparent version of the normal block. |
| `mainBoard` | The active game board to test valid positions. |
| `trackingPiece` | The current falling piece the ghost will follow. |
| `tilemap` | The `Tilemap` component used to draw the ghost blocks. |
| `cells` | Stores the relative cell positions (shape) of the ghost. |
| `position` | Stores the final dropped position of the ghost on the board. |

---

### 🧠 `Awake()`

```csharp
tilemap = GetComponentInChildren<Tilemap>();
cells = new Vector3Int[4];
```

- Grabs the child tilemap where the ghost will be rendered.
- Allocates space for the ghost's 4-cell Tetromino shape.

---

### 🔄 `LateUpdate()`

This runs **after all movement and logic** for the current frame:
```csharp
if (trackingPiece == null || mainBoard == null) return;

Clear();   // Remove old ghost
Copy();    // Copy shape from tracking piece
Drop();    // Find valid drop position
Set();     // Draw the ghost piece
```

---

### 🧼 `Clear()`

Removes the previously rendered ghost from the ghost tilemap.

---

### 📋 `Copy()`

Copies the **shape (cells)** from the currently falling `trackingPiece`:
```csharp
cells[i] = trackingPiece.cells[i];
```

---

### ⬇️ `Drop()`

- Simulates dropping the piece from its current `y` position down toward the bottom.
- Uses `mainBoard.IsValidPosition()` to check where the ghost can legally land.
- Temporarily clears the main board before checking positions to avoid ghost overlap issues.
- Once it finds the last valid position, it stores it in `this.position`.

```csharp
mainBoard.Clear(trackingPiece);

for (int row = current; row >= bottom; row--) {
    position.y = row;
    if (mainBoard.IsValidPosition(trackingPiece, position)) {
        this.position = position;
    } else {
        break;
    }
}

mainBoard.Set(trackingPiece);
```

---

### 🧱 `Set()`

Draws the ghost tile on the ghost tilemap using the dropped position.

```csharp
tilemap.SetTile(tilePosition, tile);
```

---

### 🧩 How It Fits Into the Game

- It runs every frame.
- Follows the `trackingPiece` in real-time.
- Assumes it knows which piece is currently active.
- Greatly helps players **plan their placements** before doing a hard drop.

---

## ✅ Summary

| Method | Purpose |
|--------|---------|
| `Awake()` | Initializes tilemap and ghost data |
| `LateUpdate()` | Runs the ghost update cycle |
| `Clear()` | Removes old ghost tiles |
| `Copy()` | Copies the current piece shape |
| `Drop()` | Finds the drop position |
| `Set()` | Renders the ghost on screen |

---

