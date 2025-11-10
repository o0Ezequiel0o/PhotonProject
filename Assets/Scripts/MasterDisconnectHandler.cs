using UnityEngine;
using Photon.Pun;

public class MasterDisconnectHandler : MonoBehaviourPunCallbacks
{
    void Update()
    {

    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log(otherPlayer.ToString());
        Debug.Log(PhotonNetwork.MasterClient);
        Debug.Log($"Host left: {PhotonNetwork.MasterClient == otherPlayer}");
    }
}
