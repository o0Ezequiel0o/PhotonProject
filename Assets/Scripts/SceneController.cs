using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ChangeScene(string scene)
    {
        PhotonNetwork.LoadLevel(scene);
    }
}