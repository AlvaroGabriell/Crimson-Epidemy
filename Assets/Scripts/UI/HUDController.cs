using FMOD.Studio;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public GameObject HUDGamebject;

    [Header("Fillables")]
    public RectMask2D hpMask;
    public RectMask2D xpMask, shootingMask;
    public TextMeshProUGUI hpText, xpText, timerText;

    void Awake()
    {
        HUDGamebject = gameObject;
        GameController.OnGameStarted += ShowHUD;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnDestroy()
    {
        GameController.OnGameStarted -= ShowHUD;
    }

    // Update is called once per frame
    void Update()
    {
        if(Utils.GetPlayer().IsUnityNull()) return;
        SetFill(hpMask, Utils.GetPlayer().GetComponent<HealthSystem>().GetHealth(), Utils.GetPlayer().GetComponent<HealthSystem>().GetMaxHealth());
        SetFill(xpMask, Utils.GetPlayer().GetComponent<LevelSystem>().CurrentXp, Utils.GetPlayer().GetComponent<LevelSystem>().XpToNextLevel);
        SetFill(shootingMask, Utils.GetPlayer().GetComponent<PlayerRangedAttackController>().TimeSinceLastShot, Utils.GetPlayer().GetComponent<PlayerRangedAttackController>().shootInterval);

        hpText.text = Utils.GetPlayer().GetComponent<HealthSystem>().GetHealth().ToString("F1");
        xpText.text = Utils.GetPlayer().GetComponent<LevelSystem>().CurrentLevel.ToString("F0");

        float t = GameController.Instance.RoundTime;
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void ShowHUD()
    {
        HUDGamebject.GetComponent<CanvasGroup>().alpha = 1f;
    }
    public void HideHUD()
    {
        HUDGamebject.GetComponent<CanvasGroup>().alpha = 0f;
    }

    private void SetFill(RectMask2D mask, float actualValue, float maxValue)
    {
        float fullHeight = mask.gameObject.GetComponent<RectTransform>().rect.height;
        float topPadding = fullHeight * (1 - (actualValue / maxValue));
        mask.padding = new Vector4(0, 0, 0, topPadding);
    }
}
