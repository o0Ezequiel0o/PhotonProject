using UnityEngine;
using System;

[Serializable]
public class Limits
{
    [SerializeField] private float min = float.NegativeInfinity;
    [SerializeField] private float max = float.PositiveInfinity;

    public float Min => min;
    public float Max => max;

    public Limits() { }
    
    public Limits(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}