using System;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public int CurrentLevel { get; private set; } = 1;
    public int CurrentXp { get; private set; } = 0;
    public int XpToNextLevel => GetRequiredXpToLevelUp(CurrentLevel);

    [SerializeField] private int baseXpRequirement = 10;
    [SerializeField] private float xpGrowthMultiplier = 1.25f;

    public event Action<int> OnLevelUp;

    private UpgradeController upgradeController;

    void Awake()
    {
        upgradeController = FindFirstObjectByType<UpgradeController>();
    }

    public void AddXp(int amount)
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
    
    private int GetRequiredXpToLevelUp(int level)
    {
        return (int)(baseXpRequirement * Mathf.Pow(xpGrowthMultiplier, level - 1));
    }
}
