

## 📦 `Data.cs` – Static Utility for Tetris Configuration

This class provides **rotation math**, **piece shapes**, and **wall kick data** for all Tetromino types in your game. It's used by the `Piece` script during initialization, movement, and rotation.

---

### ✅ `public static class Data`

A static class that acts like a centralized repository for:
- Rotation math
- Tetromino cell layouts
- Wall kick logic

---

### 🔁 Rotation Math

```csharp
public static readonly float cos = Mathf.Cos(Mathf.PI / 2f);
public static readonly float sin = Mathf.Sin(Mathf.PI / 2f);
public static readonly float[] RotationMatrix = new float[] { cos, sin, -sin, cos };
```

- This matrix is a **90-degree rotation** used when rotating Tetris pieces.
- It's used inside `Piece.cs` to rotate cell positions around the origin.

---

### 🎮 Tetromino Definitions

```csharp
public static readonly Dictionary<Tetromino, Vector2Int[]> Cells = ...
```

Each tetromino type is defined by the **offsets** of its 4 blocks (or "cells") relative to its origin.

| Tetromino | Shape |
|-----------|-------|
| `I` | Straight bar |
| `J` | Reverse L |
| `L` | L shape |
| `O` | Square |
| `S` | Snake (left-to-right) |
| `T` | T shape |
| `Z` | Snake (right-to-left) |

Example:
```csharp
Tetromino.I => [(-1,1), (0,1), (1,1), (2,1)]
```
This means the `I` piece is 4 blocks horizontally aligned at y = 1.

---

### 🧱 Wall Kicks – What are they?

**Wall kicks** are small position offsets tested when a rotation fails due to collision or going out of bounds. They "nudge" the piece left/right/up/down to make the rotation succeed.

---

### ⚙️ `WallKicksI` (used only for the I-piece)

Special logic for the long `I` piece, which behaves differently when rotated due to its shape.

Each row in the array corresponds to an **attempt** to reposition the piece after a rotation fails.

---

### ⚙️ `WallKicksJLOSTZ`

Used for all **non-I pieces**. Standard Super Rotation System (SRS) wall kick offsets.

Example entry:
```csharp
{ (0, 0), (-1, 0), (-1, 1), (0, -2), (-1, -2) }
```
If a rotation fails, it tries to move to each of these positions in order.

---

### 🎯 `WallKicks` Dictionary

Links each Tetromino to its appropriate wall kick table:
```csharp
WallKicks[Tetromino.L] = WallKicksJLOSTZ
WallKicks[Tetromino.I] = WallKicksI
```

This is used in `Piece.cs` during rotation to get the correct kick offsets.

---

### 🔍 Summary Table

| Section | Purpose |
|---------|---------|
| `RotationMatrix` | Used to rotate pieces by 90° |
| `Cells` | Base shape layout of each tetromino |
| `WallKicksI` | Custom kick offsets for I-piece |
| `WallKicksJLOSTZ` | Kick logic for all other tetrominoes |
| `WallKicks` | Lookup table for which piece uses which kick table |

---
