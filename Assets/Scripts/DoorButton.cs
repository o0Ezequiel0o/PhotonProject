using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DoorButton : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float durationTime = 5f;
    private readonly HashSet<Player> playersInside = new HashSet<Player>();

    public bool Active { get; private set; } = false;
    private bool Pressed => playersInside.Count > 0;

    private bool timerStarted = false;

    private float durationTimer = 0f;
    private bool canActivateAgain = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.GetPhotonView().IsMine) return;

        if (collision.TryGetComponent(out Player player))
        {
            if (playersInside.Contains(player)) return;

            playersInside.Add(player);

            if (!Active || !timerStarted && canActivateAgain)
            {
                gameObject.GetPhotonView().RPC("RPC_TurnOn", RpcTarget.AllBuffered);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!gameObject.GetPhotonView().IsMine) return;

        if (collision.TryGetComponent(out Player player))
        {
            playersInside.Remove(player);

            if (!canActivateAgain && playersInside.Count == 0)
            {
                canActivateAgain = true;
            }

            if (playersInside.Count <= 0 && Active)
            {
                gameObject.GetPhotonView().RPC("RPC_TurnOff", RpcTarget.AllBuffered);
            }
        }
    }

    private void Update()
    {
        if (!gameObject.GetPhotonView().IsMine) return;

        if (timerStarted && Active)
        {
            durationTimer += Time.deltaTime;

            if (durationTimer > durationTime)
            {
                gameObject.GetPhotonView().RPC("RPC_TurnOff", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    private void RPC_TurnOff()
    {
        spriteRenderer.color = Color.white;
        timerStarted = false;
        durationTimer = 0f;
        Active = false;
    }

    [PunRPC]
    private void RPC_TurnOn()
    {
        spriteRenderer.color = Color.green;
        canActivateAgain = false;
        timerStarted = true;
        Active = true;
    }
}