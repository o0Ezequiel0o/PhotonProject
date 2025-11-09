using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LocalBar : MonoBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private Image bar;

    private float timeToFill = 0f;
    private bool startedTimer = false;

    private float timer = 0f;

    private void Update()
    {
        if (!startedTimer) return;

        timer += Time.deltaTime;

        bar.fillAmount = timer / timeToFill;
    }

    [PunRPC]
    public void RPC_StartBar(float timeToFill)
    {
        this.timeToFill = timeToFill;
        startedTimer = true;
    }

    [PunRPC]
    public void RPC_DestroyBar()
    {
        Destroy(root.gameObject);
    }
}