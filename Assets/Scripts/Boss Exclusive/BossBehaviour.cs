using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AttributesSystem))]
[RequireComponent(typeof(HealthSystem))]
public class BossBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private ContactFilter2D contactFilter2D;
    private Rigidbody2D rb;
    private BossBehaviourHelper helper;

    private AttributesSystem attributes;
    private HealthSystem health;

    public bool isOnGround = false, isAttacking = false;
    private float sideSign = 0f;

    private Coroutine attackCoroutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        attributes = GetComponent<AttributesSystem>();
        health = GetComponent<HealthSystem>();
        health.attributes = attributes;

        helper = GetComponentInChildren<BossBehaviourHelper>();

        attributes.maxHealth.SetBaseValue(200f);
        attributes.healthRegen.SetBaseValue(0f);
        attributes.moveSpeed.SetBaseValue(6f);
        attributes.attackDamage.SetBaseValue(15f);
        attributes.criticalChance.SetPercentValue(0f);

        if (player == null) player = Utils.GetPlayer();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sideSign = Mathf.Sign(transform.position.x - player.transform.position.x);
        if (sideSign == 0) sideSign = 1;

        attackCoroutine = StartCoroutine(AttackRoutine());
    }

    void FixedUpdate()
    {
        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
        } else
        {
            FollowPlayer();
        }
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile")) health.TakeDamage(collision.GetComponent<BulletBehaviour>().damage, collision.GetComponent<BulletBehaviour>().damageSource);
        else if (isOnGround && collision.gameObject.CompareTag("Weapon")) health.TakeDamage(collision.GetComponent<OrbitalKnifeBehaviour>().damage, collision.GetComponent<OrbitalKnifeBehaviour>().damageSource);
    }

    private void FollowPlayer(float desiredDistance = 5f)
    {
        Vector3 playerPos = player.transform.position;
        Vector2 enemyPos = transform.position;

        Vector2 targetPos = new(
            playerPos.x + sideSign * desiredDistance,
            playerPos.y
        );
        
        bool moveToOtherSide = (playerPos.x - enemyPos.x) * sideSign > 0;

        if (moveToOtherSide)
        {
            sideSign *= -1;

            targetPos = new Vector2(
                playerPos.x + sideSign * desiredDistance,
                playerPos.y
            );
        }

        float dist = Vector2.Distance(enemyPos, targetPos);
        float speed = Mathf.Lerp(0f, attributes.moveSpeed.FinalValue, dist / desiredDistance);

        if(dist < 0.2f)
        {
            rb.linearVelocity = Vector2.zero;
        } else
        {
            Vector2 dir = (targetPos - enemyPos).normalized;
            rb.linearVelocity = dir * speed;
        }

        if(playerPos.x < enemyPos.x) transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (playerPos.x > enemyPos.x) transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private IEnumerator AttackRoutine()
    {
        if(isAttacking)
        yield return new WaitForSeconds(2f);
        while (true)
        {
            float waitTime = Random.Range(3f, 5f);
            yield return new WaitForSeconds(waitTime);

            Attack(GetRandomAttack());

            while(isAttacking) yield return null;
        }
    }

    AttackType GetRandomAttack()
    {
        AttackType[] types = (AttackType[])System.Enum.GetValues(typeof(AttackType));
        int index = Random.Range(0, types.Length);
        return types[index];
    }

    private void Attack(AttackType type)
    {
        if(isAttacking) return;
        isAttacking = true;

        switch (type)
        {
            case AttackType.SPIT:
                isAttacking = false;
                break;

            case AttackType.HEADSLAM:
                helper.animator.SetTrigger("HeadSlam");
                break;
        }
    }

    public void PerformHeadSlam()
    {
        BoxCollider2D col = GetComponentInChildren<BoxCollider2D>();
        GameObject bossHelper = col.gameObject;

        Vector2 size = GetScaledSize(col);
        Vector2 offset = GetScaledOffset(col);

        Vector2 center = (Vector2)bossHelper.transform.position + offset;

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            center,
            size,
            bossHelper.transform.eulerAngles.z
        );

        foreach (var h in hits)
        {
            if(h.CompareTag("Player")) h.GetComponent<HealthSystem>().TakeDamage(attributes.attackDamage.FinalValue, DamageSource.ENEMY);
        }
    }

    private Vector2 GetScaledSize(BoxCollider2D col)
    {
        Vector3 scale = col.transform.lossyScale;
        return new Vector2(col.size.x * scale.x, col.size.y * scale.y);
    }

    private Vector2 GetScaledOffset(BoxCollider2D col)
    {
        Vector3 scale = col.transform.lossyScale;

        Vector2 scaledOffset = new(col.offset.x * scale.x, col.offset.y * scale.y);

        return col.transform.rotation * scaledOffset;
    }
}

public enum AttackType
{
    SPIT,
    HEADSLAM
}
