using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AttributesSystem))]
[RequireComponent(typeof(HealthSystem))]
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player, collectablesGroup;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private AttributesSystem attributes;
    private HealthSystem health;
    [SerializeField] private GameObject xpPrefab, healthPrefab;

    public static event Action<EnemyBehaviour, DamageSource> OnEnemyDeath;
    public static event Action<EnemyBehaviour> OnEnemySpawn;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attributes = GetComponent<AttributesSystem>();
        health = GetComponent<HealthSystem>();
        health.attributes = attributes;

        attributes.maxHealth.SetBaseValue(20f);
        attributes.healthRegen.SetBaseValue(0f);
        attributes.moveSpeed.SetBaseValue(2f);
        attributes.attackDamage.SetBaseValue(5f);
        attributes.criticalChance.SetPercentValue(0f);

        if (player == null) player = Utils.GetPlayer();
    }

    void Start()
    {
        health.OnDeath += OnDeath;
        OnEnemySpawn?.Invoke(this);
        if(Utils.TryGetGroupByName("CollectablesGroup", out GameObject group)) collectablesGroup = group;
    }

    void OnDestroy()
    {
        health.OnDeath -= OnDeath;
    }

    void FixedUpdate()
    {
        FollowPlayer();
    }

    public void FollowPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * attributes.moveSpeed.FinalValue;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        spriteRenderer.flipX = angle > 90 || angle < -90;
    }

    private void OnDeath(DamageSource source)
    {
        switch (source)
        {
            case DamageSource.PLAYER:
                DropLoot();
                break;
        }
        OnEnemyDeath.Invoke(this, source);
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

            GameObject xpInstance = Instantiate(xpPrefab, dropPos, Quaternion.identity, collectablesGroup.transform);
            xpInstance.GetComponent<CollectableBehaviour>().SetValue(5);

            xpAmount--;
        }

        if(UnityEngine.Random.Range(1, 6) == 1)
        {
            offset = UnityEngine.Random.insideUnitCircle * 0.3f;
            dropPos = transform.position + (Vector3)offset;

            GameObject healthInstance = Instantiate(healthPrefab, dropPos, Quaternion.identity, collectablesGroup.transform);
            healthInstance.GetComponent<CollectableBehaviour>().SetValue(10);
        }
    }

    public float GetEnemyDamage()
    {
        return attributes.attackDamage.FinalValue;
    }
}
