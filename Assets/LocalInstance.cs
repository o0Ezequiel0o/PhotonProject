using Photon.Pun;
using UnityEngine;

public class LocalInstance : MonoBehaviour
{
    public static bool isHost = false;

    public static Player[] players;

    public static Player RandomPlayer => players[Random.Range(0, players.Length)];

    private void Start()
    {
        isHost = PhotonNetwork.CurrentRoom.PlayerCount <= 1;
        players = FindObjectsByType<Player>(FindObjectsSortMode.None);

        Debug.Log(isHost);
    }

    private void Update()
    {
        players = FindObjectsByType<Player>(FindObjectsSortMode.None);
    }
}