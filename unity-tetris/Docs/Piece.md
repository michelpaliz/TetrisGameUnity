
---

## 🎮 `Piece.cs` – Core Tetris Piece Logic

This script controls how a Tetris block (a **"Piece"**) behaves during the game — how it moves, rotates, falls, locks into place, and interacts with both the board and gesture system.

---

### 🧱 Fields and Properties

| Variable | Description |
|----------|-------------|
| `board` | Reference to the main board (grid system). |
| `data` | Contains shape, rotation data, and wall kicks for the tetromino. |
| `cells` | Array of 4 blocks forming the shape of the piece. |
| `position` | Position of the piece on the grid. |
| `rotationIndex` | Tracks which rotation state the piece is in. |
| `stepDelay`, `moveDelay`, `lockDelay` | Control fall speed, movement frequency, and delay before locking. |
| `isLocked` | Prevents movement after the piece is locked. |
| `stepTime`, `moveTime`, `lockTime` | Timing variables for controlling behavior. |

---

### 🚀 `Initialize(...)`

Sets up the piece:

- Sets position and data.
- Initializes movement timing.
- Copies tetromino shape.
- Links the gesture system (if available).

```csharp
gestureListener.playerPiece = this;
```

This ensures the gesture system always controls the current piece.

---

### 🔁 `Update()`

Runs every frame (if not locked):

- Clears the piece visually from the board.
- Handles input:
  - `Q`/`E` → rotate
  - `Space` → hard drop
  - `A`/`D` → move
  - `S` → soft drop
- Moves the piece down every `stepDelay` seconds.
- Draws the piece back on the board.

---

### 🔽 `HandleMoveInputs()`

Handles continuous movement when keys are held:

- Soft drop (`S`)
- Move left (`A`)
- Move right (`D`)

Also resets timers to prevent pieces from dropping too fast.

---

### 📉 `Step()`

Automatic fall:

- Moves the piece down one unit.
- If inactive for too long (`lockDelay`), it gets locked.

---

### 💥 `HardDrop()`

Drops the piece straight down until it hits the bottom:

```csharp
while (Move(Vector2Int.down)) { continue; }
Lock();
```

---

### 🔒 `Lock()`

When the piece reaches its final resting place:

- Prevents further input via `isLocked`.
- Fixes piece visually onto the board.
- Clears any full lines.
- Spawns the next piece.

---

### ↔️ `Move(Vector2Int)`

Moves the piece if the new position is valid:

- Logs movement.
- Updates timers.
- Blocks movement if `isLocked`.

```csharp
board.Clear(this);
if (Move(...)) board.Set(this);
```

---

### 🔄 `Rotate(int direction)`

Handles rotation:

- Applies rotation matrix.
- Checks if rotated position is valid using wall kicks.
- Reverts rotation if invalid.

---

### 💠 `ApplyRotationMatrix(int)`

Rotates each block using the standard SRS matrix:

- Special handling for `I` and `O` shapes (center offset).

---

### 🧱 `TestWallKicks(...)`

When rotating near walls or other blocks, this checks for small shifts (wall kicks) that would allow the rotation.

---

### 🔄 `Wrap(...)`

Wraps rotation index so it stays within 0–3 (modulo rotation system).

---

## 🔗 Gesture Integration

The `Initialize()` method makes sure the `GestureListener` is synced with the current piece:

```csharp
GestureListener gestureListener = FindObjectOfType<GestureListener>();
gestureListener.playerPiece = this;
```

This allows gesture-based control in real-time.

---

## ✅ Summary

| Feature        | Function |
|----------------|----------|
| Falling        | Auto step after time |
| Soft Drop      | `S` or gesture |
| Hard Drop      | `Space` or thumbs down gesture |
| Move           | Left/right using keys or gestures |
| Rotate         | `Q/E` or open hand gesture |
| Gesture Sync   | `playerPiece` updated for GestureListener |
| Locking        | Piece locks in place when delay expires |

---

