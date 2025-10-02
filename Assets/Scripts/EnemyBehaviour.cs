using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private int level = 1;
    [SerializeField] private float damage = 5f, baseMoveSpeed = 2.0f, moveSpeedModifier = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        FollowPlayer();
    }

    public void FollowPlayer()
    {
        // Move o inimigo
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * (baseMoveSpeed * moveSpeedModifier);

        //Rotaciona e flipa o sprite do inimigo
        transform.right = direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle > 90 || angle < -90)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
    }

    public void SetEnemyLevel(int level)
    {
        this.level = level;
    }
    public int GetEnemyLevel()
    {
        return level;
    }
    public void SetEnemyDamage(float damage)
    {
        this.damage = damage;
    }
    public float GetEnemyDamage()
    {
        return damage;
    }
}
