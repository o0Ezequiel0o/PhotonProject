using UnityEngine;

public class SetTarget : MonoBehaviour
{
    void Start()
    {
        if (TryGetComponent(out ITargetFollower targetFollower))
        {
            targetFollower.SetTarget(LocalInstance.RandomPlayer.transform);
        }
    }
}