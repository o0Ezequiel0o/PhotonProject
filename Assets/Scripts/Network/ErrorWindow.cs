using UnityEngine;
using TMPro;

public class ErrorWindow : MonoBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private TextMeshProUGUI errorDescriptionText;

    public void PassData(string errorDescription)
    {
        errorDescriptionText.text = errorDescription;
    }

    public void CloseWindow()
    {
        Destroy(root.gameObject);
    }
}