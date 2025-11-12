using System;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public int CurrentLevel { get; private set; } = 1;
    public float CurrentXp { get; private set; } = 0f;
    public float XpToNextLevel => GetRequiredXpToLevelUp(CurrentLevel);

    [SerializeField] private float baseXpRequirement = 10f;
    [SerializeField] private float xpGrowthMultiplier = 1.25f;

    public event Action<int> OnLevelUp;

    private UpgradeController upgradeController;

    void Awake()
    {
        upgradeController = FindFirstObjectByType<UpgradeController>();
    }

    public void AddXp(float amount)
    {
        CurrentXp += amount;

        while (CurrentXp >= XpToNextLevel)
        {
            CurrentXp -= XpToNextLevel;
            LevelUp();
        }
    }
    
    private void LevelUp()
    {
        CurrentLevel++;

        OnLevelUp?.Invoke(CurrentLevel);

        if(upgradeController != null) upgradeController.OpenUpgradeScreen(CurrentLevel);
    }
    
    private float GetRequiredXpToLevelUp(int level)
    {
        return baseXpRequirement * Mathf.Pow(xpGrowthMultiplier, level - 1);
    }
}
