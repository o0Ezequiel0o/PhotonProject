using System.Collections.Generic;
using UnityEngine;

public class DamageableFlash : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float flashAmount = 1f;
    [SerializeField] private float flashDuration = 0.25f;

    private static readonly int flashAmountID = Shader.PropertyToID("_FlashAmount");

    private Material[] materials;
    private float flashTimer = 0f;

    void Awake()
    {
        SpriteRenderer[] spriteRenderers = GetSpriteRenderers();
        GetMaterialsFromSpriteRenderers(spriteRenderers);
        SubscribeToEvents();
    }

    void Update()
    {
        if (flashTimer < 0f) return;
        UpdateFlashTimer();
    }

    SpriteRenderer[] GetSpriteRenderers()
    {
        return GetComponentsInChildren<SpriteRenderer>();
    }

    void GetMaterialsFromSpriteRenderers(SpriteRenderer[] spriteRenderers)
    {
        List<Material> materials = new List<Material>();

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i].material.HasProperty(flashAmountID))
            {
                materials.Add(spriteRenderers[i].material);
            }
        }

        this.materials = materials.ToArray();
    }

    void SubscribeToEvents()
    {
        if (!TryGetComponent(out Health health)) return;

        health.onDamageTaken += StartFlash;
    }

    void UpdateFlashTimer()
    {
        flashTimer -= Time.deltaTime;

        if (flashTimer <= 0)
        {
            StopFlash();
        }
    }

    void StartFlash()
    {
        flashTimer = flashDuration;
        SetFlashAmount(flashAmount);
    }

    void StopFlash()
    {
        SetFlashAmount(0f);
    }

    void SetFlashAmount(float amount)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat(flashAmountID, amount);
        }
    }
}