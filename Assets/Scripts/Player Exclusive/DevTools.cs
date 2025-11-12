using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(AudioSource))]
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
    [SerializeField] private GameObject zombiePrefab, bulletPrefab, devScreen;
    [SerializeField] private RawImage theDev;
    private AudioSource audioSource;

    [Header("Tools")]
    public bool manualShoot = false;

    private GameObject zombieInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = gameObject;
        audioSource = GetComponent<AudioSource>();
        GetComponent<PlayerInput>().actions.FindActionMap("DevMode").Disable();
    }

    // Update is called once per frame
    void Update()
    {
        theDev.rectTransform.Rotate(new Vector3(0, transform.rotation.eulerAngles.y + -180 * Time.deltaTime, 0));

        if (devModeActive) return;

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

    private void ActivateDevMode()
    {
        devModeActive = true;
        GetComponent<PlayerInput>().actions.FindActionMap("DevMode").Enable();
        devScreen.SetActive(true);
        Debug.Log("Dev Mode activated!");
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
            zombieInstance = Instantiate(zombiePrefab, new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0), Quaternion.identity);
            Debug.Log("Zombie spawned!");
        }
    }
    public void SpawnsImmortalZombieAtMouse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SpawnsZombieAtMouse(context);
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
            player.GetComponent<HealthSystem>().SetHealth(20f);
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
                Destroy(zombie);
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
                player.GetComponent<PlayerRangedAttack>().canShoot = true;
                manualShoot = false;
                Debug.Log("Shooting Mode: Automatic");
            }
            else
            {
                player.GetComponent<PlayerRangedAttack>().canShoot = false;
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
                bulletInstance.GetComponent<BulletBehaviour>().playerAttributes = player.GetComponent<AttributesSystem>();
            }
        }
    }
    public void EasterEgg(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SFXManager.Play("Pluh", audioSource);
        }
    }

    public void LogPlayerAttributes(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            var attributes = player.GetComponent<PlayerController>().attributes.GetAttributeDictionary();
            var health = player.GetComponent<HealthSystem>().GetHealth();
            System.Text.StringBuilder sb = new();

            sb.AppendLine("health: " + health);

            foreach (var kvp in attributes)
            {
                var attrName = kvp.Key.ToString();
                var attribute = kvp.Value;
                sb.AppendLine($"{attrName}: {attribute.baseValue} * {attribute.modifier} = {attribute.FinalValue}");
            }

            Debug.Log(sb.ToString());
        }
    }
}
