using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float range;
    [SerializeField] private LayerMask layer;

    private RaycastHit2D[] hits;

    public void TryInteractWithClose()
    {
        hits = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0f, layer);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.TryGetComponent(out IInteractable interactable))
            {
                if (interactable.Interact(gameObject)) break;
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255f, 165f, 0f);
        Gizmos.DrawWireSphere(transform.position, range);
    }
}