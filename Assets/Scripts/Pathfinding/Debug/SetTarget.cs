using UnityEngine;

public class SetTarget : MonoBehaviour
{
    private Player targetPlayer;

    void Start()
    {
        if (TryGetComponent(out ITargetFollower targetFollower))
        {
            targetPlayer = LocalInstance.RandomPlayer;
            targetFollower.SetTarget(targetPlayer.transform);
        }
    }

    private void Update()
    {
        if (targetPlayer == null || targetPlayer.IsDowned)
        {
            while (targetPlayer.IsDowned && !LocalInstance.AllPlayersDowned())
            {
                targetPlayer = LocalInstance.RandomPlayer;

                if (!targetPlayer.IsDowned && TryGetComponent(out ITargetFollower targetFollower))
                {
                    targetPlayer = LocalInstance.RandomPlayer;
                    targetFollower.SetTarget(targetPlayer.transform);
                }
            }
        }
    }
}