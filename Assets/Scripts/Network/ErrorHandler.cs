using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ErrorHandler : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject errorWindowPrefab;
    [SerializeField] private Transform spawnParent;

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CreateErrorWindow(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        CreateErrorWindow(message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateErrorWindow(message);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        CreateErrorWindow($"Disconnected\nCause: {cause}");
    }

    private void CreateErrorWindow(string message)
    {
        if (Instantiate(errorWindowPrefab, spawnParent).TryGetComponent(out ErrorWindow errorWindow))
        {
            errorWindow.PassData(message);
        }
    }
}