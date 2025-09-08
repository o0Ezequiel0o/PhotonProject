using Photon.Pun;
using UnityEngine;

public class AimToMouse : MonoBehaviour
{
    [Header("Dependency")]
    [SerializeField] private EntityAim entityAim;
    [SerializeField] private PhotonView photonView;

    [Header("Settings")]
    [SerializeField] private Transform center;
    [SerializeField] private UpdateIn updateMode;

    private Vector2 direction = Vector2.zero;

    private Vector3 mousePosition;
    private Camera mainCam;

    private enum UpdateIn
    {
        Update,
        FixedUpdate
    }

    void Reset()
    {
        entityAim = GetComponentInChildren<EntityAim>();
        center = GetComponentInChildren<Transform>();
    }

    void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        if (updateMode == UpdateIn.Update)
        {
            UpdateDirection();
        }
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        if (updateMode == UpdateIn.FixedUpdate)
        {
            UpdateDirection();
        }
    }

    void UpdateDirection()
    {
        mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);

        direction.x = mousePosition.x - center.position.x;
        direction.y = mousePosition.y - center.position.y;

        entityAim.AimTowards(direction.normalized);
    }
}