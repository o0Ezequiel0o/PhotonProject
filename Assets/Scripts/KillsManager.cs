using System;
using UnityEngine;
using TMPro;

public class KillsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText;

    public static int totalKills = 0;

    private void Awake()
    {
        totalKills = 0;
    }

    private void Update()
    {
        displayText.text = $"Total Kills: {totalKills}";
    }
}