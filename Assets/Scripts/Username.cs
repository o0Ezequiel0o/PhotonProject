using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Username : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject UsernamePage;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void SaveUsername()
    {
        PhotonNetwork.NickName = inputField.text;
        PlayerPrefs.SetString("Username", inputField.text);

        UsernamePage.SetActive(false);
    }
}
