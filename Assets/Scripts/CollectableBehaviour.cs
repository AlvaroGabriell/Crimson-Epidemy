using System;
using UnityEngine;

public class CollectableBehaviour : MonoBehaviour
{
    public CollectableType collectableType;
    public float value = 0f;

    public void SetValue(float value)
    {
        this.value = value;
    }
    public float GetValue()
    {
        return value;
    }
    public void SetCollectableType(CollectableType type)
    {
        collectableType = type;
    }
    public CollectableType GetCollectableType()
    {
        return collectableType;
    }
}

public enum CollectableType
{
    Xp,
    Health
}