using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRangedAttack : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bullet;
    private GameObject bulletInstance;

    private Vector2 mouseWorldPosition, direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetMousePosition();
        HandleShooting();
    }

    private void GetMousePosition()
    {
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = (mouseWorldPosition - (Vector2)player.transform.position).normalized;
        player.transform.right = direction;
    }

    private void HandleShooting()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            bulletInstance = Instantiate(bullet, player.transform.position, player.transform.rotation);
        }
    }
}
