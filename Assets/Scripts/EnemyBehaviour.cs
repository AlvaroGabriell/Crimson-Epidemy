using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AttributesSystem))]
[RequireComponent(typeof(HealthSystem))]
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private AttributesSystem attributes;
    private HealthSystem health;
    [SerializeField] private int level = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attributes = GetComponent<AttributesSystem>();
        health = GetComponent<HealthSystem>();
        health.attributes = attributes;

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
        rb.linearVelocity = direction * attributes.moveSpeed.FinalValue;

        //Rotaciona e flipa o sprite do inimigo
        //transform.right = direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle > 90 || angle < -90)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    public float GetEnemyDamage()
    {
        return attributes.attackDamage.FinalValue;
    }

    public void SetEnemyLevel(int level)
    {
        this.level = level;
    }
    public int GetEnemyLevel()
    {
        return level;
    }
}
