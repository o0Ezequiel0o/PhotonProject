using Photon.Pun;
using UnityEngine;

public class ShootTest : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform shootPos;
    [SerializeField] private EntityAim entityAim;
    [SerializeField] private PhotonView photonView;

    private void Update()
    {
        if (!photonView.IsMine) return;
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = PhotonNetwork.Instantiate("Bullet", shootPos.transform.position, player.rotation);

            if (bullet.TryGetComponent(out TestBullet testBullet))
            {
                testBullet.LaunchProjectile(entityAim.AimDirection);
            }
        }
    }
}
