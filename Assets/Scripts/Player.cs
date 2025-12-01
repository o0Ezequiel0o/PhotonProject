using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    public TextMeshProUGUI text;

    private DownHandler downHandler;

    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;

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
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        animator.SetFloat("velocity", Math.Abs(rb.velocity.y + rb.velocity.x));
        animator.SetBool("isDowned", IsDowned);
    }

    private void Start()
    {
        text.text = GetComponent<PhotonView>().Controller.NickName;

        if (gameObject.GetPhotonView().IsMine)
        {
            FindFirstObjectByType<CinemachineVirtualCamera>().Follow = transform;
            AmmoUI ui = FindObjectOfType<AmmoUI>();

            if (ui != null)
            {
                ui.Initialize(GetComponent<AmmoInventory>(), GetComponent<WeaponController>());
            }
        }
    }
}