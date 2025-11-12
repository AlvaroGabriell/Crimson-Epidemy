using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDataLibrary : MonoBehaviour
{
    [SerializeField] private List<UpgradeData> upgradesData;

    public UpgradeData GetRandomUpgradeData()
    {
        return upgradesData[UnityEngine.Random.Range(0, upgradesData.Count)];
    }
}