using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue = 1f;
    [SerializeField] private float increase = 0f;
    [SerializeField] private Limits valueLimits;

    private float flatModifier = 0f;
    private float multiplier = 1f;

    public float Value
    {
        get
        {
            return Mathf.Clamp((baseValue + flatModifier) * multiplier, valueLimits.Min, valueLimits.Max);
        }
    }

    public int Level { get; private set; }

    public Stat() { }

    private List<StatMultiplier> statMultipliers = new List<StatMultiplier>();

    public Stat(float baseValue, float increase, float min, float max)
    {
        this.baseValue = baseValue;
        this.increase = increase;

        valueLimits = new Limits(min, max);
    }

    public void AddMultiplier(StatMultiplier statMultiplier)
    {
        if (!statMultipliers.Contains(statMultiplier))
        {
            statMultiplier.onMultiplierUpdated += RecalculateValueMultiplier;
            statMultipliers.Add(statMultiplier);

            RecalculateValueMultiplier();
        }
    }

    public void RemoveMultiplier(StatMultiplier statMultiplier)
    {
        statMultiplier.onMultiplierUpdated -= RecalculateValueMultiplier;
        statMultipliers.Remove(statMultiplier);

        RecalculateValueMultiplier();
    }

    public void ApplyFlatModifier(float increase)
    {
        flatModifier += increase;
    }

    public void Upgrade()
    {
        baseValue += increase;
        Level += 1;
    }

    void RecalculateValueMultiplier()
    {
        multiplier = 1f;

        for (int i = 0; i < statMultipliers.Count; i++)
        {
            multiplier *= statMultipliers[i].Multiplier;
        }
    }

    public Stat(Stat original)
    {
        baseValue = original.baseValue;
        increase = original.increase;

        valueLimits = original.valueLimits;
    }

    public Stat Copy()
    {
        return new Stat(baseValue, increase, valueLimits.Min, valueLimits.Max);
    }
}