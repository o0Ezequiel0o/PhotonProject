using UnityEngine;
using Photon.Pun;
using TMPro;

public class LobbyErrorMessages : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI errorMessagePrefab;
    [SerializeField] private Transform spawnRoot;

    private void CreateMessage(string message)
    {
        TextMeshProUGUI messageInstance = Instantiate(errorMessagePrefab, spawnRoot);
        messageInstance.text = message;
        Destroy(messageInstance.gameObject, 10f);
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        CreateMessage($"{newMasterClient} is now the master client");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        CreateMessage($"{newPlayer} joined the room");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        CreateMessage($"{otherPlayer} left the room");
    }
}
