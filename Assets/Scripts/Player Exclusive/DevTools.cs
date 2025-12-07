using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class DevTools : MonoBehaviour
{

    [Header("Activation")]
    private Key[] konamiCode = new Key[]{
        Key.UpArrow, Key.UpArrow,
        Key.DownArrow, Key.DownArrow,
        Key.LeftArrow, Key.RightArrow,
        Key.LeftArrow, Key.RightArrow,
        Key.B, Key.A
    };
    private int currentIndex = 0;
    private bool devModeActive = false;

    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject zombiePrefab, bulletPrefab, BulletsGroup, devScreen, playerInfoGO, roundInfoGO;
    [SerializeField] private GameObject theDev1, theDev2;
    private TextMeshProUGUI playerInfoText, roundInfoText;

    [Header("Tools")]
    public bool manualShoot = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = gameObject;
        playerInfoText = playerInfoGO.GetComponent<TextMeshProUGUI>();
        roundInfoText = roundInfoGO.GetComponent<TextMeshProUGUI>();
        GetComponent<PlayerInput>().actions.FindActionMap("DevMode").Disable();
    }

    // Update is called once per frame
    void Update()
    {
        theDev1.GetComponent<RectTransform>().transform.Rotate(new Vector3(0,  -180 * Time.deltaTime, 0));
        theDev2.GetComponent<RectTransform>().transform.Rotate(new Vector3(transform.rotation.eulerAngles.x + -180 * Time.deltaTime, 0, 0));
        UpdatePlayerInfo();
        UpdateRoundInfo();

        if (devModeActive && !GameController.Instance.gameStarted) return;

        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            foreach (var keyControl in Keyboard.current.allKeys)
            {
                if (keyControl.wasPressedThisFrame)
                {
                    CheckKey(keyControl.keyCode);
                    break;
                }
            }
        }
    }

    private void CheckKey(Key key)
    {
        if (key == konamiCode[currentIndex])
        {
            currentIndex++;

            if (currentIndex >= konamiCode.Length)
            {
                ActivateDevMode();
                currentIndex = 0;
            }
        }
        else
        {
            currentIndex = 0;
        }
    }

    public void ActivateDevMode()
    {
        devModeActive = true;
        GetComponent<PlayerInput>().actions.FindActionMap("DevMode").Enable();
        devScreen.SetActive(true);
        Debug.Log("Dev Mode activated!");
    }

    public void DeactivateDevMod()
    {
        devModeActive = false;
        GetComponent<PlayerInput>().actions.FindActionMap("DevMode").Disable();
        devScreen.SetActive(false);

        player.GetComponent<HealthSystem>().HealFullHealth();
        player.GetComponent<HealthSystem>().canDie = true;

        player.GetComponent<PlayerRangedAttackController>().canShoot = true;
        manualShoot = false;

        currentIndex = 0;

        Debug.Log("Dev Mode deactivated! Everything back to normal.");
    }

    public void HideShowMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            devScreen.SetActive(!devScreen.activeSelf);
        }
    }
    public void SpawnsZombieAtMouse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            EnemySpawner.SpawnEnemy(new Vector2(mouseWorldPosition.x, mouseWorldPosition.y));
            Debug.Log("Zombie spawned!");
        }
    }
    public void SpawnsImmortalZombieAtMouse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            GameObject zombieInstance = EnemySpawner.SpawnEnemy(new Vector2(mouseWorldPosition.x, mouseWorldPosition.y));
            zombieInstance.GetComponent<HealthSystem>().canDie = false;
            Debug.Log("Immortal zombie spawned.");
        }
    }
    public void MakePlayerImmortal(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.GetComponent<HealthSystem>().canDie = false;
            Debug.Log("Player is now immortal. But will still take damage!");
        }
    }
    public void MakePlayerMortal(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.GetComponent<HealthSystem>().HealFullHealth();
            player.GetComponent<HealthSystem>().canDie = true;
            Debug.Log("Player is now mortal.");
        }
    } 
    public void KillAllZombies(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject[] zombiesArray = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject zombie in zombiesArray)
            {
                zombie.GetComponent<HealthSystem>().Kill(DamageSource.VOID);
            }
            if (zombiesArray.Length > 0) Debug.Log("All zombies killed.");
            else Debug.Log("No zombies found.");
        }
    }
    public void SwitchShootMode(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (manualShoot)
            {
                player.GetComponent<PlayerRangedAttackController>().canShoot = true;
                manualShoot = false;
                Debug.Log("Shooting Mode: Automatic");
            }
            else
            {
                player.GetComponent<PlayerRangedAttackController>().canShoot = false;
                manualShoot = true;
                Debug.Log("Shooting Mode: Manual");
            }
        }
    }
    public void ManualShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (manualShoot)
            {
                Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                Vector2 direction = (mouseWorldPosition - (Vector2)player.transform.position).normalized;
                GameObject bulletInstance = Instantiate(bulletPrefab, player.transform.position, Quaternion.FromToRotation(Vector3.right, direction));
                bulletInstance.GetComponent<BulletBehaviour>().Setup(player.GetComponent<AttributesSystem>(), DamageSource.PLAYER);
                if (PlayerRangedAttackController.IsCriticalHit(player.GetComponent<AttributesSystem>())) bulletInstance.GetComponent<BulletBehaviour>().TurnIntoCriticalHit();
            }
        }
    }
    public void RestartGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DeactivateDevMod();
            Debug.Log("Game Restarting.");
            GameController.Instance.RestartGame();
        }
    }
    public void EasterEgg(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SFXManager.Instance.PlaySFX(SFXManager.Instance.SFXLibrary.GetSFXByName("Pluh"));
        }
    }

    public void HideShowGameInfo(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerInfoGO.SetActive(!playerInfoGO.activeSelf);
            roundInfoGO.SetActive(!roundInfoGO.activeSelf);
        }
    }

    private void UpdatePlayerInfo()
    {
        var attributes = player.GetComponent<PlayerController>().attributes.GetAttributeDictionary();
        var health = player.GetComponent<HealthSystem>().GetHealth();
        var level = player.GetComponent<LevelSystem>();

        StringBuilder sb = new();

        sb.AppendLine("> Player Info:");

        sb.AppendLine($"level: {level.CurrentLevel} ({level.CurrentXp}/{level.XpToNextLevel})");
        sb.AppendLine("health: " + health);

        foreach (var kvp in attributes)
        {
            var attrName = kvp.Key.ToString();
            var attribute = kvp.Value;
            sb.AppendLine($"{attrName}: {attribute.baseValue} * {attribute.modifier} = {attribute.FinalValue}");
        }
        
        playerInfoText.text = sb.ToString();
    }

    private void UpdateRoundInfo()
    {
        StringBuilder sb = new();

        sb.AppendLine("> Round Info:");
        sb.AppendLine($"timeDifficulty: {SpawningInfo.lastTimeDifficulty:F2}");
        sb.AppendLine($"levelDifficulty: {SpawningInfo.lastLevelDifficulty:F2}");
        sb.AppendLine($"killDifficulty: {SpawningInfo.lastKillDifficulty:F2}\n");

        sb.AppendLine($"zombies alive: {GameController.Instance.enemyCount}");
        sb.AppendLine($"zombies killed: {GameController.Instance.killedEnemies}\n");

        sb.AppendLine($"enemy spawn interval: {SpawningInfo.spawnInterval:F2}\n");

        if(SpawningInfo.lastSpawnedZombie == null)
        {
            sb.AppendLine("> No zombies yet.");
        } else
        {
            var attributes = SpawningInfo.lastSpawnedZombie.GetComponent<AttributesSystem>();
            sb.AppendLine("> Last Spawned Zombie Stats:");

            sb.AppendLine($"maxHealth: {attributes.maxHealth.baseValue:F2} * {attributes.maxHealth.modifier:F2} = {attributes.maxHealth.FinalValue:F2}");
            sb.AppendLine($"moveSpeed: {attributes.moveSpeed.baseValue:F2} * {attributes.moveSpeed.modifier:F2} = {attributes.moveSpeed.FinalValue:F2}");
            sb.AppendLine($"attackDamage: {attributes.attackDamage.baseValue:F2} * {attributes.attackDamage.modifier:F2} = {attributes.attackDamage.FinalValue:F2}");
        }

        roundInfoText.text = sb.ToString();
    }
}
