using Photon.Pun;
using UnityEngine;

public class LocalInstance : MonoBehaviour
{
    public static Player[] players;

    public static Player RandomPlayer => players[Random.Range(0, players.Length)];

    public static bool AllPlayersDowned()
    {
        foreach (Player player in players)
        {
            if (!player.IsDowned) return false;
        }

        return true;
    }

    private void Update()
    {
        players = FindObjectsByType<Player>(FindObjectsSortMode.None);
    }
}