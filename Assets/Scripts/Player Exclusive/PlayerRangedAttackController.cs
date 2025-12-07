using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRangedAttackController : MonoBehaviour
{
    [SerializeField] private GameObject bullet, BulletsGroup;
    private GameObject player, bulletInstance;
    private AttributesSystem playerAttributes;
    private Vector2 mouseWorldPosition, direction;
    public float shootInterval;
    public bool canShoot = false;
    public float TimeSinceLastShot { get; private set; } = 0f;
    public EventReference arrowShootSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = gameObject;
        playerAttributes = player.GetComponent<AttributesSystem>();
        shootInterval = 2f / playerAttributes.attackSpeed.FinalValue;
        StartCoroutine(Shoot());
    }

    void Update()
    {
        shootInterval = 2f / playerAttributes.attackSpeed.FinalValue;

        TimeSinceLastShot += Time.deltaTime;

        if(TimeSinceLastShot > shootInterval) TimeSinceLastShot = shootInterval; // Por segurança
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(shootInterval);
        while (true)
        {
            if (canShoot)
            {
                mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                direction = (mouseWorldPosition - (Vector2)player.transform.position).normalized;
                bulletInstance = Instantiate(bullet, player.transform.position, Quaternion.FromToRotation(Vector3.right, direction), BulletsGroup.transform);
                SFXManager.Instance.PlaySFX(SFXManager.Instance.SFXLibrary.GetSFXByReference(arrowShootSFX));

                bulletInstance.GetComponent<BulletBehaviour>().Setup(playerAttributes, DamageSource.PLAYER);
                if (IsCriticalHit(playerAttributes)) bulletInstance.GetComponent<BulletBehaviour>().TurnIntoCriticalHit();

                TimeSinceLastShot = 0f;
            }

            yield return new WaitForSeconds(shootInterval);
        }
    }

    public static bool IsCriticalHit(AttributesSystem attributes)
    {
        return Random.value < attributes.criticalChance.FinalValue || attributes.criticalChance.FinalValue >= 1f;
    }
}
