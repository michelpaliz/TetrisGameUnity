using UnityEngine;
using UnityEngine.Tilemaps;

public partial class Board : MonoBehaviour
{
    public void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;
        int clearedLines = 0;

        while (row < bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);
                clearedLines++;
            }
            else
            {
                row++;
            }
        }

        if (clearedLines > 0)
        {
            AddScore(clearedLines);
        }
    }

    private void AddScore(int lines)
    {
        int points = lines switch
        {
            1 => 100,
            2 => 300,
            3 => 500,
            4 => 800,
            _ => lines * 200
        };

        score += points;
        Debug.Log($"🏆 +{points} points! Total Score: {score}");

        UpdateScoreUI(); // Call from BoardUI.cs
    }

    public bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            if (!tilemap.HasTile(new Vector3Int(col, row, 0)))
                return false;
        }

        return true;
    }

    public void LineClear(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            tilemap.SetTile(new Vector3Int(col, row, 0), null);
        }

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int above = new Vector3Int(col, row + 1, 0);
                tilemap.SetTile(new Vector3Int(col, row, 0), tilemap.GetTile(above));
            }
            row++;
        }

        if (CameraShake.Instance != null)
            CameraShake.Instance.Shake();

        if (lineClearSound != null)
            AudioSource.PlayClipAtPoint(lineClearSound, Camera.main.transform.position, 1f);
    }
}
