using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using PimDeWitte.UnityMainThreadDispatcher;
using System;

public class GestureListener : MonoBehaviour
{
    private TcpListener listener;
    private Thread listenerThread;
    private bool isListening = false;
    private readonly object lockObject = new object(); // For thread safety

    [HideInInspector]
    public Piece playerPiece;

    void Start()
    {
        StartListening();
    }

    public void StartListening()
    {
        lock (lockObject)
        {
            if (isListening) return;

            Debug.Log("🟢 GestureListener is active. Listening on port 5050...");
            listenerThread = new Thread(ListenForCommands);
            listenerThread.IsBackground = true;
            listenerThread.Start();
            isListening = true;
        }
    }

    public void StopListening()
    {
        lock (lockObject)
        {
            if (!isListening) return;

            Debug.Log("🔴 GestureListener is stopping...");
            isListening = false;

            try
            {
                listener?.Stop();
            }
            catch (Exception ex)
            {
                Debug.LogError("❌ Error stopping listener: " + ex.Message);
            }

            listener = null;

            if (listenerThread != null && listenerThread.IsAlive)
            {
                listenerThread.Join(1000); // Wait up to 1 second
                if (listenerThread.IsAlive)
                {
                    Debug.LogWarning("⚠️ Listener thread did not exit cleanly, aborting...");
                    listenerThread.Abort(); // Force terminate (use cautiously)
                }
                listenerThread = null;
            }
        }
    }

    void ListenForCommands()
    {
        try
        {
            lock (lockObject)
            {
                listener = new TcpListener(IPAddress.Any, 5050);
                listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                listener.Start();
            }

            while (isListening)
            {
                try
                {
                    if (!listener.Pending())
                    {
                        Thread.Sleep(100); // Prevent tight loop when idle
                        continue;
                    }

                    using (var client = listener.AcceptTcpClient())
                    using (var stream = client.GetStream())
                    {
                        byte[] buffer = new byte[256];
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        string command = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();

                        Debug.Log("📨 Received: " + command);

                        try
                        {
                            UnityMainThreadDispatcher.Instance().Enqueue(() =>
                            {
                                if (playerPiece == null)
                                {
                                    playerPiece = FindObjectOfType<Piece>();
                                    if (playerPiece == null)
                                    {
                                        Debug.LogWarning("⚠️ playerPiece is null — skipping command: " + command);
                                        return;
                                    }
                                }

                                HandleCommand(command);
                            });
                        }
                        catch (Exception ex)
                        {
                            Debug.LogWarning($"⚠️ Could not enqueue command — dispatcher might be missing. Skipped: {command}\n{ex.Message}");
                        }
                    }
                }
                catch (SocketException ex)
                {
                    if (isListening)
                        Debug.LogError("❌ Socket error in client handling: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ Listener thread crashed: " + ex.Message);
        }
        finally
        {
            lock (lockObject)
            {
                if (listener != null)
                {
                    listener.Stop();
                    listener = null;
                }

                isListening = false;
            }
        }
    }

    private void HandleCommand(string command)
    {
        Board board = FindObjectOfType<Board>();
        if (board == null || board.IsGameOver) return; // Ignore commands if game is over

        switch (command)
        {

            case "MOVE_LEFT":
                Debug.Log("🎯 MOVE_LEFT triggered via gesture");
                playerPiece.board.Clear(playerPiece);
                if (playerPiece.Move(Vector2Int.left))
                    playerPiece.board.Set(playerPiece);
                break;

            case "MOVE_RIGHT":
                Debug.Log("🎯 MOVE_RIGHT triggered via gesture");
                playerPiece.board.Clear(playerPiece);
                if (playerPiece.Move(Vector2Int.right))
                    playerPiece.board.Set(playerPiece);
                break;

            case "SOFT_DROP":
                Debug.Log("📉 SOFT_DROP triggered via gesture");
                playerPiece.board.Clear(playerPiece);
                if (playerPiece.Move(Vector2Int.down))
                    playerPiece.board.Set(playerPiece);
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
    }

    void OnApplicationQuit()
    {
        StopListening();
    }
}
