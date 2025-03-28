using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using PimDeWitte.UnityMainThreadDispatcher;

public class GestureListener : MonoBehaviour
{
    private TcpListener listener;
    private Thread listenerThread;

    [HideInInspector]
    public Piece playerPiece;


    void Start()
    {
        Debug.Log("🟢 GestureListener is active. Listening on port 5050...");
        listenerThread = new Thread(ListenForCommands);
        listenerThread.IsBackground = true;
        listenerThread.Start();
    }

    void Update()
    {
        // Auto-assign piece if not manually linked
        //if (playerPiece == null)
        //{
        //    playerPiece = FindObjectOfType<Piece>();
        //}
    }

    void ListenForCommands()
    {
        listener = new TcpListener(IPAddress.Any, 5050);
        listener.Start();

        while (true)
        {
            try
            {
                using (var client = listener.AcceptTcpClient())
                using (var stream = client.GetStream())
                {
                    byte[] buffer = new byte[256];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string command = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();

                    Debug.Log("📨 Received: " + command);

                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        // Double-check playerPiece is available
                        if (playerPiece == null)
                        {
                            playerPiece = FindObjectOfType<Piece>();
                            if (playerPiece == null)
                            {
                                Debug.LogWarning("⚠️ playerPiece is null — skipping command: " + command);
                                return;
                            }
                        }

                        switch (command)
                        {
                            case "MOVE_LEFT":
                                Debug.Log("🎯 MOVE_LEFT triggered via gesture");
                                playerPiece.board.Clear(playerPiece);
                                if (playerPiece.Move(Vector2Int.left))
                                {
                                    playerPiece.board.Set(playerPiece); // ✅ Force visual update
                                }
                                break;

                            case "MOVE_RIGHT":
                                Debug.Log("🎯 MOVE_RIGHT triggered via gesture");
                                playerPiece.board.Clear(playerPiece);
                                if (playerPiece.Move(Vector2Int.right))
                                {
                                    playerPiece.board.Set(playerPiece); // ✅ Force visual update
                                }
                                break;

                            case "SOFT_DROP":
                                Debug.Log("📉SOFT_DROP triggered via gesture");
                                playerPiece.board.Clear(playerPiece);
                                if (playerPiece.Move(Vector2Int.down))
                                {
                                    playerPiece.board.Set(playerPiece);
                                }
                                break;

                            case "ROTATE":
                                Debug.Log("🔄 ROTATE triggered via gesture");
                                playerPiece.board.Clear(playerPiece);
                                playerPiece.Rotate(1);
                                playerPiece.board.Set(playerPiece);
                                break;

                            case "HARD_DROP":
                                Debug.Log("💥 HARD_DROP triggered via gesture");
                                playerPiece.board.Clear(playerPiece);
                                playerPiece.HardDrop();
                                break;


                            default:
                                Debug.Log("❓ Unknown gesture command: " + command);
                                break;
                        }
                    });
                }
            }
            catch (SocketException ex)
            {
                Debug.LogError("❌ Socket error: " + ex.Message);
            }
        }
    }

    void OnApplicationQuit()
    {
        if (listener != null)
        {
            listener.Stop();
            listener = null;
        }

        if (listenerThread != null && listenerThread.IsAlive)
        {
            listenerThread.Abort(); // optional, or use a cancellation token instead for safety
            listenerThread = null;
        }
    }

}
