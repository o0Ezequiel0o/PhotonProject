using Photon.Pun;
using TMPro;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    public TextMeshProUGUI text;

    private DownHandler downHandler;

    public bool IsDowned
    {
        get
        {
            if (downHandler != null) return downHandler.IsDowned;
            else return false;
        }
    }

    private void Awake()
    {
        downHandler = GetComponent<DownHandler>();
    }

    private void Start()
    {
        text.text = GetComponent<PhotonView>().Controller.NickName;

        if (gameObject.GetPhotonView().IsMine)
        {
            FindFirstObjectByType<CinemachineVirtualCamera>().Follow = transform;
        }
    }
}