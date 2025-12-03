using System;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour
{
    private List<AttributeUpgrade> attributesToUpgrade;
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
        upImage.sprite = pUpgradeData.upgradeImage;
        upName.text = pUpgradeData.upgradeName;
        upDescription.text = pUpgradeData.upgradeDescription;
        attributesToUpgrade = pUpgradeData.attributesToUpgrade;
    }

    public void OnClick()
    {
        upgradeController.ApplyUpgrade(attributesToUpgrade);
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
    public List<AttributeUpgrade> attributesToUpgrade;
}

[Serializable]
public struct AttributeUpgrade
{
    public Attribute attribute;
    public UpgradeValueType upgradeValueType;
    [Tooltip("Use value corresponding to selected value type\nbaseValue: 1 = 1\nmodifier: 1 = 100%")]
    public float value;
}

public enum UpgradeValueType
{
    baseValue,
    modifier
}