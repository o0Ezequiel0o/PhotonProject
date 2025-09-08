using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    
    public float movSpeed;
    float speedX, speedY;
    Rigidbody2D rb;
    public GameObject canvasName;
    public TextMeshProUGUI text;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        text.text = GetComponent<PhotonView>().Controller.NickName;
    }

    void Update()
    {
        if(photonView.IsMine)
        {
            speedX = Input.GetAxisRaw("Horizontal") * movSpeed;
            speedY = Input.GetAxisRaw("Vertical") * movSpeed;
            rb.velocity = new Vector2(speedX, speedY);
        }
    }
}
