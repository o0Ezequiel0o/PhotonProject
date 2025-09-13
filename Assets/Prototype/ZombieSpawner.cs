using Photon.Pun;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && LocalInstance.isHost)
        {
            PhotonNetwork.Instantiate("Zombie", new Vector3(Random.Range(-8, 8), -1.5f, 0), Quaternion.identity);
        }
    }
}