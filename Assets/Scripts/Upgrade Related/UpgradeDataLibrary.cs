using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDataLibrary : MonoBehaviour
{
    [SerializeField] private List<UpgradeData> upgradesData;

    public UpgradeData GetRandomUpgradeData()
    {
        float totalWeight = 0f;

        foreach(var upgrade in upgradesData) totalWeight += upgrade.weight;

        float randomValue = UnityEngine.Random.value * totalWeight;

        foreach(var upgrade in upgradesData)
        {
            randomValue -= upgrade.weight;
            if(randomValue <= 0f) return upgrade;
        }

        // Fallback
        return upgradesData[UnityEngine.Random.Range(0, upgradesData.Count)];
    }
}