using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Start()
    {
        text.text = GetComponent<PhotonView>().Controller.NickName;
    }
}
