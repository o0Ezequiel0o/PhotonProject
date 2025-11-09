using UnityEngine;
using Photon.Pun;

public class ZombieAttack : MonoBehaviour
{
    [SerializeField] private EntityMove entityMove;
    [SerializeField] private int damage = 15;
    [SerializeField] private float attackInterval = 1f;

    public bool Attacking { get; private set; } = false;

    private float timer = 0f;

    private StatMultiplier attackMoveSpeedMultiplier = new StatMultiplier(0f);

    private void Update()
    {
        if (Attacking)
        {
            timer += Time.deltaTime;

            if (timer < attackInterval)
            {
                return;
            }
            else
            {
                entityMove.moveSpeed.RemoveMultiplier(attackMoveSpeedMultiplier);
                Attacking = false;
            }
        }

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.up, 1f);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject == gameObject) continue;

            if (hit.collider.TryGetComponent(out Player player) && hit.collider.TryGetComponent(out Health health))
            {
                hit.collider.gameObject.GetPhotonView().RPC("RPC_TakeDamageAny", RpcTarget.AllBuffered, damage);
                entityMove.moveSpeed.AddMultiplier(attackMoveSpeedMultiplier);
                Attacking = true;
                timer = 0f;
            }
        }
    }
}