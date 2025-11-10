using Photon.Pun;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private string weaponName;

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        PhotonNetwork.Instantiate(weaponName, spawnPosition.position, Quaternion.identity);
    }
}