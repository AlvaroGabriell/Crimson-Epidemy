using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRangedAttack : MonoBehaviour
{
    [SerializeField] private GameObject player, bullet;
    private GameObject bulletInstance;

    private Vector2 mouseWorldPosition, direction;
    public float shootInterval = 2f;
    public bool canShoot = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            }
            yield return new WaitForSeconds(shootInterval);
        }
    }
}
