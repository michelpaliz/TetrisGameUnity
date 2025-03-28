
---

## 🎮 `GestureListener.cs` – Tetris Gesture Command Receiver

This script handles **TCP network communication** and **maps incoming gesture commands to in-game actions** on the active Tetris piece.

---

### 🔧 Purpose

- Listens on a local TCP port (`5050`) for gesture commands sent from an external app (like your Python + MediaPipe gesture detector).
- Applies the corresponding Tetris actions (move, rotate, drop) to the currently active piece on the game board.
- Ensures all Unity-related changes run safely on the **main thread** using `UnityMainThreadDispatcher`.

---

### 📡 Key Components

#### ✅ `TcpListener listener`
- The server that listens for connections on port 5050.

#### ✅ `Thread listenerThread`
- Runs `ListenForCommands()` on a background thread to avoid blocking Unity’s main thread.

#### ✅ `public Piece playerPiece`
- The current active `Piece` in the game. This is the one the gestures will affect.

---

### 🟢 `Start()`
```csharp
listenerThread = new Thread(ListenForCommands);
listenerThread.IsBackground = true;
listenerThread.Start();
```
- Starts a background thread to listen for gesture commands.
- Logs a startup message to confirm it's running.

---

### 💬 `ListenForCommands()`
This is the **core loop** that runs on a separate thread:

1. **Starts the TCP listener.**
2. **Accepts client connections (like from the Python script).**
3. **Reads the gesture command string.**
4. **Enqueues the gesture response logic to run on the Unity main thread** via `UnityMainThreadDispatcher`.

---

### 🔄 `UnityMainThreadDispatcher.Instance().Enqueue(...)`

Ensures that Unity methods like `Move()` or `Rotate()` are safely called from the **main Unity thread**.

This is needed because Unity is **not thread-safe**.

---

### 🧠 Command Handling

Each command is matched using a `switch` block:

#### ▶️ `"MOVE_LEFT"` and `"MOVE_RIGHT"`
```csharp
playerPiece.board.Clear(playerPiece);
if (playerPiece.Move(Vector2Int.left/right)) {
    playerPiece.board.Set(playerPiece);
}
```
- Moves the active piece horizontally.
- Forces the board to clear and redraw the piece for visual accuracy.

#### ⬇️ `"SOFT_DROP"`
- Moves the piece **down by one** cell.

#### 🔁 `"ROTATE"`
- Rotates the piece **clockwise**.

#### 💥 `"HARD_DROP"`
```csharp
playerPiece.board.Clear(playerPiece);
playerPiece.HardDrop();
```
- Instantly drops the piece to the bottom.
- Triggers a lock-in and spawns a new piece.

#### ❓ `default`
- Logs a warning for unknown commands.

---

### 🧼 `OnApplicationQuit()`
Stops the TCP server and background thread when the game exits:
```csharp
listener.Stop();
listenerThread.Abort();
```

---

### 🚫 Commented-Out Code in `Update()`
```csharp
// Auto-assign playerPiece if not manually linked
```
You commented out the part that tries to find the active piece automatically every frame. Instead, you likely now assign it in `Board.SpawnPiece()` manually using:
```csharp
gestureListener.playerPiece = activePiece;
```

👍 This is cleaner and avoids ambiguity or errors during transitions.

---

## ✅ Summary

| Section | Purpose |
|--------|---------|
| `Start()` | Begins TCP listening for gesture commands |
| `ListenForCommands()` | Handles incoming commands and applies them to the game |
| `UnityMainThreadDispatcher` | Makes sure Unity API calls are safe from other threads |
| `playerPiece` | The currently active Tetris piece being controlled |
| `OnApplicationQuit()` | Cleans up sockets and threads on game exit |

---
