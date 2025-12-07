using System;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour
{
    private UpgradeData upgradeData;
    private UpgradeController upgradeController;

    private Image upImage;
    private TextMeshProUGUI upName, upDescription;

    void Awake()
    {
        upgradeController = GameObject.Find("UpgradeController").GetComponent<UpgradeController>();
        upImage = transform.Find("UpgradeImage").GetComponent<Image>();
        upName = transform.Find("UpgradeName").GetComponent<TextMeshProUGUI>();
        upDescription = transform.Find("UpgradeDescription").GetComponent<TextMeshProUGUI>();
    }

    public void Setup(UpgradeData pUpgradeData)
    {
        upgradeData = pUpgradeData;
        upImage.sprite = pUpgradeData.upgradeImage;
        upName.text = pUpgradeData.upgradeName;
        upDescription.text = pUpgradeData.upgradeDescription;
    }

    public void OnClick()
    {
        upgradeController.ApplyUpgrade(upgradeData);
    }
}

[Serializable]
public struct UpgradeData
{
    public Sprite upgradeImage;
    public string upgradeName;
    [TextArea]
    public string upgradeDescription;
    [Range(0f, 100f)]public float weight;

    public UpgradeType type;
    public List<AttributeUpgrade> attributesToUpgrade;
    public WeaponUpgrade weaponUpgrade;
}

public enum UpgradeType
{
    Attribute,
    Weapon
}

public enum UpgradeValueType
{
    baseValue,
    modifier
}

public enum WeaponType
{
    OrbitalKnife
}

[Serializable]
public struct AttributeUpgrade
{
    public Attribute attribute;
    public UpgradeValueType upgradeValueType;
    [Tooltip("Use value corresponding to selected value type\nbaseValue: 1 = 1\nmodifier: 1 = 100%")]
    public float value;
}

[Serializable]
public struct WeaponUpgrade
{
    public WeaponType weaponType;
    public float amount;
}