using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Dependency")]
    [SerializeField] private SceneController sceneController;

    [Header("Settings")]
    [SerializeField] private string lobbyScene;

    [Header("Temp")]
    [SerializeField] private TMP_InputField createInput;
    [SerializeField] private TMP_InputField joinInput;

    public void CreateRoom()
    {
        PhotonNetwork.JoinOrCreateRoom(createInput.text, new RoomOptions(), TypedLobby.Default);
    }

    public void JoinRoom(string roomName)
    {
        if (string.IsNullOrEmpty(roomName)) return;
        PhotonNetwork.JoinRoom(roomName);
    }

    public void JoinRoom()
    {
        JoinRoom(joinInput.text);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(lobbyScene);
    }

    public override void OnJoinedLobby()
    {
        //PhotonNetwork.LoadLevel(lobbyScene); // for starting the server actually
        //sceneController.ChangeScene(lobbyScene);
    }
}