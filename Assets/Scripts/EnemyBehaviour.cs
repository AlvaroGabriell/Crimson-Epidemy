using System;
using Unity.VisualScripting;
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
    [SerializeField] private GameObject xpPrefab, healthPrefab;

    public static event Action<EnemyBehaviour> OnEnemyDeath;
    public static event Action<EnemyBehaviour> OnEnemySpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attributes = GetComponent<AttributesSystem>();
        health = GetComponent<HealthSystem>();
        health.attributes = attributes;

        attributes.healthRegen.SetBaseValue(0f);
        attributes.regenSpeed.SetBaseValue(1f);

        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        health.OnDeath += OnDeath;
        OnEnemySpawn?.Invoke(this);
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
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    private void OnDeath()
    {
        DropLoot();
        OnEnemyDeath.Invoke(this);
        Destroy(gameObject);
    }

    public void DropLoot()
    {
        int xpAmount = 1;
        Vector2 offset;
        Vector3 dropPos;
        if(UnityEngine.Random.value <= 0.10f)
        {
            xpAmount++;
        }

        while(xpAmount > 0)
        {
            offset = UnityEngine.Random.insideUnitCircle * 0.3f;
            dropPos = transform.position + (Vector3)offset;

            GameObject xpInstance = Instantiate(xpPrefab, dropPos, Quaternion.identity);
            xpInstance.GetComponent<CollectableBehaviour>().SetValue(5);

            xpAmount--;
        }

        if(UnityEngine.Random.Range(1, 4) == 1)
        {
            offset = UnityEngine.Random.insideUnitCircle * 0.3f;
            dropPos = transform.position + (Vector3)offset;

            GameObject healthInstance = Instantiate(healthPrefab, dropPos, Quaternion.identity);
            healthInstance.GetComponent<CollectableBehaviour>().SetValue(10);
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
