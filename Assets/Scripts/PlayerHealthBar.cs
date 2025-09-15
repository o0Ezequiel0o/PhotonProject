using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image frontBar;

    public void UpdateFill(float percentage)
    {
        frontBar.fillAmount = percentage;
    }
}