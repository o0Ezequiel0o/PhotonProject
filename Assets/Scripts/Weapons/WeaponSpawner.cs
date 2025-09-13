using Photon.Pun;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;

    private void Start()
    {
        if (!LocalInstance.isHost) return;

        PhotonNetwork.Instantiate("Weapon", spawnPosition.position, Quaternion.identity);
    }
}