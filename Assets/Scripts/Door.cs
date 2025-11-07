using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Door : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private List<DoorButton> buttons = new List<DoorButton>();

    private bool opened = false;

    private void Update()
    {
        if (!opened && CanOpen())
        {
            RPC_OpenDoor();
        }
    }

    private bool CanOpen()
    {
        foreach (DoorButton button in buttons)
        {
            if (!button.Active)
            {
                return false;
            }
        }

        return true;
    }

    [PunRPC]
    private void RPC_OpenDoor()
    {
        gameObject.SetActive(false);
        opened = true;
    }
}