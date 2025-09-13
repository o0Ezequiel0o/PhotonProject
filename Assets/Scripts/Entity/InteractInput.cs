using Photon.Pun;
using UnityEngine;

public class InteractInput : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private InteractionHandler interactionHandler;

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            interactionHandler.TryInteractWithClose();
        }
    }
}