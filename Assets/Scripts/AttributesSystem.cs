using System.Collections.Generic;
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
    public ScalableAttribute pickupRange;

    private Dictionary<Attribute, ScalableAttribute> attributes;

    void Awake()
    {
        attributes = new Dictionary<Attribute, ScalableAttribute>
        {
            { Attribute.maxHealth, maxHealth },
            { Attribute.healthRegen, healthRegen },
            { Attribute.regenSpeed, regenSpeed },
            { Attribute.moveSpeed, moveSpeed },
            { Attribute.attackDamage, attackDamage },
            { Attribute.attackSpeed, attackSpeed },
            { Attribute.projectileSpeed, projectileSpeed },
            { Attribute.criticalChance, criticalChance },
            { Attribute.criticalMultiplier, criticalMultiplier },
            { Attribute.pickupRange, pickupRange }
        };
    }

    public ScalableAttribute GetAttributeByType(Attribute attr)
    {
        return attributes[attr];
    }

    public Dictionary<Attribute, ScalableAttribute> GetAttributeDictionary()
    {
        return attributes;
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
        modifier *= 1f + (percent / 100f);
    }
    public void SetPercentValue(float percent)
    {
        modifier = percent / 100f;
    }
}

public enum Attribute
{
    maxHealth,
    healthRegen,
    regenSpeed,
    moveSpeed,
    attackDamage,
    attackSpeed,
    projectileSpeed,
    criticalChance,
    criticalMultiplier,
    pickupRange
}