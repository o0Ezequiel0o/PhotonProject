using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ChangeScene(string scene)
    {
        PhotonNetwork.LoadLevel(scene);
    }

    public void OnExitButtonPressed()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }
        
        PhotonNetwork.LoadLevel("LoadingScreen");
    }
}