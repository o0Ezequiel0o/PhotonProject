using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using Photon.Pun;
using TMPro;

public class LeaderboardDisplayer : MonoBehaviour
{
    private bool uploading = false;
    private bool uploaded = false;

    public TextMeshProUGUI playerNames;
    public TextMeshProUGUI playerScores;

    private bool requestHighscore = false;

    public void UploadHighscore()
    {
        if (!uploaded && !uploading && PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SubmitScoreRoutine());
        }
    }

    private void Awake()
    {
        GameOverController gameOverController = FindFirstObjectByType<GameOverController>();
        if (gameOverController != null)
        {
            gameOverController.onGameOver += LoadHighScore;
        }
    }

    private void LoadHighScore()
    {
        if (requestHighscore) return;
        StartCoroutine(FetchTopHighscoresRoutine());
    }

    public IEnumerator SubmitScoreRoutine()
    {
        bool done = false;
        uploading = true;
        string playerID = PhotonNetwork.NickName;
        LootLockerSDKManager.SubmitScore(playerID, KillsManager.totalKills, "globalhighscore", (response) =>
        {
            if (response.success)
            {
                Debug.Log("Uploaded score");
                uploading = false;
                done = true;
            }
            else
            {
                Debug.Log("Failed to upload score");
                uploading = false;
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);
        uploaded = true;
    }

    public IEnumerator FetchTopHighscoresRoutine()
    {
        bool done = false;
        requestHighscore = true;
        LootLockerSDKManager.GetScoreList("globalhighscore", 10, 0, (response) =>
        {
            if (response.success)
            {
                string tempPlayerNames = "Names\n";
                string tempPlayerScores = "Kills\n";

                LootLockerLeaderboardMember[] members = response.items;

                for (int i = 0; i  < members.Length; i++)
                {
                    tempPlayerNames += members[i].rank + ". ";
                    
                    if (members[i].player.name != "")
                    {
                        tempPlayerNames += members[i].player.name;
                    }
                    else
                    {
                        tempPlayerNames += members[i].player.id;
                    }

                    tempPlayerScores += members[i].score + "\n";
                    tempPlayerNames += "\n";
                }
                done = true;
                playerNames.text = tempPlayerNames;
                playerScores.text = tempPlayerScores;
            }
            else
            {
                Debug.Log("Failed to display leaderboard");
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);
    }
}
