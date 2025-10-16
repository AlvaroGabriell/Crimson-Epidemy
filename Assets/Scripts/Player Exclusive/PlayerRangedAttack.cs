using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRangedAttack : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    private GameObject player, bulletInstance;
    private AttributesSystem playerAttributes;
    private Vector2 mouseWorldPosition, direction;
    public float shootInterval = 2f;
    public bool canShoot = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = gameObject;
        playerAttributes = player.GetComponent<AttributesSystem>();
        StartCoroutine(Shoot());
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
                bulletInstance = Instantiate(bullet, player.transform.position, Quaternion.FromToRotation(Vector3.right, direction));

                bulletInstance.GetComponent<BulletBehaviour>().playerAttributes = playerAttributes;
            }

            shootInterval = 2f / playerAttributes.attackSpeed.FinalValue;

            yield return new WaitForSeconds(shootInterval);
        }
    }
}
