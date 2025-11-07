using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    private readonly HashSet<Player> playersInside = new HashSet<Player>();

    public bool Active => playersInside.Count > 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            if (playersInside.Contains(player)) return;

            playersInside.Add(player);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            playersInside.Remove(player);
        }
    }
}