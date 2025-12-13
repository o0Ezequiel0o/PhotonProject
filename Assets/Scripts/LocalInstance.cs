using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LocalInstance : MonoBehaviourPunCallbacks
{
    public static List<Photon.Realtime.Player> photonPlayers;
    public static List<Player> players = new List<Player>();

    public static Player RandomPlayer => players[Random.Range(0, players.Count)];

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
        players = FindObjectsByType<Player>(FindObjectsSortMode.None).ToList();
        photonPlayers = new List<Photon.Realtime.Player>();

        for (int i = 0; i < players.Count; i++)
        {
            photonPlayers.Add(players[i].gameObject.GetPhotonView().Owner);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        for (int i = 0; i < photonPlayers.Count; i++)
        {
            if (photonPlayers[i] == otherPlayer)
            {
                Destroy(players[i].gameObject);
                players.RemoveAt(i);
                photonPlayers.RemoveAt(i);
            }
        }
    }
}