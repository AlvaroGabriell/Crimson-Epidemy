using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthSystem))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private HealthSystem health;
    private float horizontalMovement = 0f, verticalMovement = 0f;
    public float moveSpeedModifier = 1f, baseMoveSpeed = 3.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<HealthSystem>();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.GetComponent<EnemyBehaviour>() != null)
            {
                health.TakeDamage(collision.GetComponent<EnemyBehaviour>().GetEnemyDamage());
                Destroy(collision.gameObject);
            }
        }
    }

    //Calcula e executa o movimento do jogador.
    public void HandleMovement()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * baseMoveSpeed * moveSpeedModifier,
                                        verticalMovement * baseMoveSpeed * moveSpeedModifier);
    }

    //Captura o input de movimento
    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        verticalMovement = context.ReadValue<Vector2>().y;
    }
}
