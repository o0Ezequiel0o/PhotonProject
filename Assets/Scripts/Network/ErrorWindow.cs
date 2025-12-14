using Photon.Pun;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ErrorWindow : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform root;
    [SerializeField] private TextMeshProUGUI errorDescriptionText;
    [SerializeField] private string menuSceneName = "LoadingScreen";

    public void PassData(string errorDescription)
    {
        errorDescriptionText.text = errorDescription;
    }

    public void CloseWindow()
    {
        root.gameObject.SetActive(false);

        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
            else
            {
                PhotonNetwork.Disconnect();
            }
        }
        else
        {
            LoadMenu();
        }
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        LoadMenu();
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}