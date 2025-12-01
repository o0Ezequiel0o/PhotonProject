using LootLocker.Requests;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SetupRoutine());
    }

    IEnumerator SetupRoutine()
    {
        yield return LoginRoutine();
        SetPlayerName();
    }

    IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player was logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.Log("Could not start session");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    private void SetPlayerName()
    {
        LootLockerSDKManager.SetPlayerName(PhotonNetwork.NickName, (response) =>
        {
            if (response.success)
            {
                Debug.Log("succesfully set player name");
            }
            else
            {
                Debug.Log("failed to set player name");
            }
        });
    }
}