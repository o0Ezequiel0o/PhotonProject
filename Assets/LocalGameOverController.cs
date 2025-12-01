using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalGameOverController : MonoBehaviour
{
    public static LocalGameOverController Instance;

    [Header("UI")] public GameObject gameOverCanvas;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowGameOverScreen()
    {
        if (gameOverCanvas != null) gameOverCanvas.SetActive(true);
    }
}
