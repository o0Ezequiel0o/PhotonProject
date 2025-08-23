using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoomView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomName;

    private LobbyManager lobbyManager;

    private void Awake()
    {
        lobbyManager = FindFirstObjectByType<LobbyManager>();
    }

    public void JoinRoom()
    {
        lobbyManager.JoinRoom(roomName.text);
    }

    public void SetData(string name)
    {
        roomName.text = name;
    }
}