using Photon.Pun;
using UnityEngine;

public class MoveInput : MonoBehaviour
{
    [Header("Dependency")]
    [SerializeField] private EntityMove entityMove;
    [SerializeField] private PhotonView photonView;

    private Vector2 inputAxis = Vector2.zero;

    void Reset()
    {
        entityMove = GetComponentInChildren<EntityMove>();
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        inputAxis.x = Input.GetAxisRaw("Horizontal");
        inputAxis.y = Input.GetAxisRaw("Vertical");

        entityMove.DesiredMoveDirection = inputAxis;
    }
}