using System.Collections.Generic;
using UnityEngine;

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

    public void ApplyUpgrade(List<AttributeUpgrade> attributesToUpgrade)
    {
        foreach (AttributeUpgrade upgrade in attributesToUpgrade)
        {
            ScalableAttribute attribute = playerAttributes.GetAttributeByType(upgrade.attribute);

            if (upgrade.upgradeValueType == UpgradeValueType.baseValue) attribute.ApplyBaseUpgrade(upgrade.value);
            else if (upgrade.upgradeValueType == UpgradeValueType.modifier) attribute.ApplyPercentUpgrade(upgrade.value);
        }

        CloseUpgradeScreen();
    }

    public void OpenUpgradeScreen(int CurrentLevel)
    {
        player.GetComponent<HealthSystem>().HealFullHealth();
        GameController.Instance.PauseGame();
        UpgradeScreen.SetActive(true);

        for(int i = 0; i < 3; i++)
        {
            UpgradeData data = upgradeDataLibrary.GetRandomUpgradeData();

            GameObject cardGameObject = Instantiate(upgradeCardPrefab, upgradesContainer);
            UpgradeCard card = cardGameObject.GetComponent<UpgradeCard>();

            card.Setup(data);

            currentCards.Add(cardGameObject);
        }
    }
    public void CloseUpgradeScreen()
    {
        player.GetComponent<HealthSystem>().HealFullHealth(); // Guarantee
        UpgradeScreen.SetActive(false);
        GameController.Instance.ResumeGame();

        foreach (GameObject card in currentCards) Destroy(card);
        currentCards.Clear();
    }

}
