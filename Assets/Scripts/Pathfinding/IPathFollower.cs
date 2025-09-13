using UnityEngine;
using System;

public interface IPathFollower
{
    public void StartPath(Vector3 target);

    public void StartPath(Vector3 target, Action<bool> callback);

    public void OnPathCompleted(Action callBack);

    public void SetPath(Vector3[] newPath);

    public void CancelCurrentPath();

    public bool PathCompleted { get; }

    public bool PathCanceled { get; }
}