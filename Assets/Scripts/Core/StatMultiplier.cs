using System;

public class StatMultiplier
{
    public float Multiplier { get; private set; } = 1f;
    public Action onMultiplierUpdated;

    public StatMultiplier() {}

    public StatMultiplier(float multiplier)
    {
        Multiplier = multiplier;
    }

    public void UpdateMultiplier(float newMultiplierValue)
    {
        if (Multiplier != newMultiplierValue)
        {
            Multiplier = newMultiplierValue;
            onMultiplierUpdated?.Invoke();
        }
    }
}