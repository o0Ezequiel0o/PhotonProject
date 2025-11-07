using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfoDisplayer : MonoBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private GameObject playerInfoPrefab;

    private List<PlayerInfoTabObject> playerInfoTabInstances = new List<PlayerInfoTabObject>();
    private List<PlayerInfoTab> playerInfoTabs = new List<PlayerInfoTab>();

    private void Update()
    {
        foreach (Player player in LocalInstance.players)
        {
            if (player.gameObject.GetPhotonView().IsMine) continue;

            if (PlayerAlreadyInTabs(player)) continue;

            CreateNewTab(player);
        }

        UpdateTabsInfo();
    }

    private void UpdateTabsInfo()
    {
        for (int i = playerInfoTabInstances.Count - 1;  i >= 0; i--)
        {
            if (playerInfoTabInstances[i].player == null)
            {
                DestroyPlayerInfoInformation(playerInfoTabInstances[i].player);
                Destroy(playerInfoTabInstances[i].gameObject);
                playerInfoTabInstances.RemoveAt(i);
            }
            else
            {
                //CHANGE THIS DISGUSTING CODE BEFORE RELEASE
                playerInfoTabInstances[i].frontBar.fillAmount = playerInfoTabInstances[i].player.gameObject.GetComponent<Health>().currentHp / playerInfoTabInstances[i].player.gameObject.GetComponent<Health>().maxHp;
            }
        }
    }

    private void CreateNewTab(Player player)
    {
        PlayerInfoTabObject infoTab = Instantiate(playerInfoPrefab, root).GetComponent<PlayerInfoTabObject>();
        infoTab.playerName.text = player.gameObject.GetPhotonView().Owner.NickName;
        infoTab.player = player;
        playerInfoTabInstances.Add(infoTab);
        playerInfoTabs.Add(new PlayerInfoTab(player, player.GetComponent<Health>(), player.gameObject.GetPhotonView().Owner.NickName));
    }

    private void DestroyPlayerInfoInformation(Player player)
    {
        foreach(PlayerInfoTab playerInfoTab in playerInfoTabs)
        {
            if (playerInfoTab.player == player)
            {
                playerInfoTabs.Remove(playerInfoTab);
                return;
            }
        }
    }

    private bool PlayerAlreadyInTabs(Player player)
    {
        foreach(PlayerInfoTab tab in playerInfoTabs)
        {
            if (tab.player == player) return true;
        }

        return false;
    }
}

public struct PlayerInfoTab
{
    public Player player;
    public Health health;
    public string name;

    public PlayerInfoTab(Player player, Health health, string name)
    {
        this.player = player;
        this.health = health;
        this.name = name;
    }
}