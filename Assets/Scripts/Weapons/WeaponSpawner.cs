using Photon.Pun;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private string weaponName;

    private void Start()
    {
        if (!LocalInstance.isHost) return;

        PhotonNetwork.Instantiate(weaponName, spawnPosition.position, Quaternion.identity);
    }
}