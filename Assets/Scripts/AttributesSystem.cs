using UnityEngine;

public class AttributesSystem : MonoBehaviour
{
    public ScalableAttribute maxHealth;
    public ScalableAttribute healthRegen;
    public ScalableAttribute regenSpeed;
    public ScalableAttribute moveSpeed;
    public ScalableAttribute attackDamage;
    public ScalableAttribute attackSpeed;
    public ScalableAttribute projectileSpeed;
    public ScalableAttribute criticalChance;
    public ScalableAttribute criticalMultiplier;

    public void ApplyBaseUpgrade(ScalableAttribute attribute, float amount)
    {
        attribute.baseValue += amount;
    }
    public void SetBaseValue(ScalableAttribute attribute, float amount)
    {
        attribute.baseValue = amount;
    }
    public void ApplyPercentUpgrade(ScalableAttribute attribute, float percent)
    {
        attribute.modifier += percent / 100f;
    }
    public void SetPercentValue(ScalableAttribute attribute, float percent)
    {
        attribute.modifier = percent / 100f;
    }
}

[System.Serializable]
public class ScalableAttribute
{
    public float baseValue = 1;
    public float modifier = 1f; // 1 = 100%

    public float FinalValue => baseValue * modifier;

    public void ApplyBaseUpgrade(float amount)
    {
        baseValue += amount;
    }
    public void SetBaseValue(float amount)
    {
        baseValue = amount;
    }
    public void ApplyPercentUpgrade(float percent)
    {
        modifier += percent / 100f;
    }
    public void SetPercentValue(float percent)
    {
        modifier = percent / 100f;
    }
}