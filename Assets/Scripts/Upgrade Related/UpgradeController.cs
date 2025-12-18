using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UpgradeDataLibrary))]
public class UpgradeController : MonoBehaviour
{
    private GameObject player;
    private AttributesSystem playerAttributes;

    [Header("Referências de Upgrade")]
    [SerializeField] private UpgradeDataLibrary upgradeDataLibrary;
    [SerializeField] private Transform upgradesContainer;
    [SerializeField] private GameObject upgradeCardPrefab;

    private List<GameObject> currentCards = new();

    public GameObject UpgradeScreen; // TODO: Remover depois

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttributes = player.GetComponent<AttributesSystem>();
        upgradeDataLibrary = GetComponent<UpgradeDataLibrary>();
    }

    public void ApplyUpgrade(UpgradeData upgradeData)
    {
        switch (upgradeData.type)
        {
            case UpgradeType.Attribute:
                foreach (AttributeUpgrade upgrade in upgradeData.attributesToUpgrade)
                {
                    ScalableAttribute attribute = playerAttributes.GetAttributeByType(upgrade.attribute);

                    if (upgrade.upgradeValueType == UpgradeValueType.baseValue) attribute.ApplyBaseUpgrade(upgrade.value);
                    else if (upgrade.upgradeValueType == UpgradeValueType.modifier) attribute.ApplyPercentUpgrade(upgrade.value);
                }
                break;

            case UpgradeType.Weapon:
                var weaponController = player.GetComponent<PlayerOrbitalKnifeController>();
                switch (upgradeData.weaponUpgrade.weaponType)
                {
                    case WeaponType.OrbitalKnife:
                        weaponController.AddKnives((int)upgradeData.weaponUpgrade.amount);
                        break;

                }
                break;
        }

        CloseUpgradeScreen();
    }

    public void OpenUpgradeScreen(int CurrentLevel)
    {
        player.GetComponent<HealthSystem>().HealFullHealth();
        GameController.Instance.PauseGame(false);
        UpgradeScreen.SetActive(true);

        for(int i = 0; i < 3; i++)
        {
            UpgradeData data = upgradeDataLibrary.GetRandomUpgradeData();

            GameObject cardGameObject = Instantiate(upgradeCardPrefab, upgradesContainer);
            UpgradeCard card = cardGameObject.GetComponent<UpgradeCard>();

            card.Setup(data);

            cardGameObject.transform.Find("UpgradeImage").GetComponent<Image>().SetNativeSize();

            currentCards.Add(cardGameObject);
        }
    }
    public void CloseUpgradeScreen()
    {
        player.GetComponent<HealthSystem>().SetMaxHealthAndFullHeal(playerAttributes.maxHealth.FinalValue); // Guarantee
        UpgradeScreen.SetActive(false);
        GameController.Instance.ResumeGame(false);

        foreach (GameObject card in currentCards) Destroy(card);
        currentCards.Clear();
    }

}
