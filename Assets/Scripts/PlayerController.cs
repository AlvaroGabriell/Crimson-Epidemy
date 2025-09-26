using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float horizontalMovement = 0f, verticalMovement = 0f;
    public float moveSpeedModifier = 1f, baseMoveSpeed = 2.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Calcula e executa o movimento do jogador.
    public void HandleMovement()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * baseMoveSpeed * moveSpeedModifier,
                                        verticalMovement * baseMoveSpeed * moveSpeedModifier);
    }

    //Capta o input de movimento
    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        verticalMovement = context.ReadValue<Vector2>().y;
    }
}
