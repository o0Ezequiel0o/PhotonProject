using UnityEngine;
using Photon.Pun;

public class MasterDisconnectHandler : MonoBehaviourPunCallbacks
{
    Photon.Realtime.Player masterClient;

    private void Start()
    {
        masterClient = PhotonNetwork.MasterClient;
    }

    void Update()
    {
        if (masterClient == null)
        {
            Debug.Log("Host Left");
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (masterClient == null || masterClient == otherPlayer)
        {
            Debug.Log("Host Left");
        }
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log(otherPlayer.ToString());
        Debug.Log(PhotonNetwork.MasterClient);
        Debug.Log($"Host changed: {PhotonNetwork.MasterClient == otherPlayer}");
    }
}
