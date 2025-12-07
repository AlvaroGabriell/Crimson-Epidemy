using System;
using System.Data.Common;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float destroyTime = 3f;
    private Rigidbody2D rb;
    public AttributesSystem shooterAttributes;
    public DamageSource damageSource = DamageSource.VOID;

    public float damage = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetVelocity();

        // Destrói o projétil depois de determinado tempo.
        Destroy(gameObject, destroyTime);
    }

    public void Setup(AttributesSystem shooterAttributes, DamageSource damageSource)
    {
        this.shooterAttributes = shooterAttributes;
        this.damageSource = damageSource;

        damage = shooterAttributes.attackDamage.FinalValue;
    }

    public void TurnIntoCriticalHit()
    {
        damage = shooterAttributes.attackDamage.FinalValue * shooterAttributes.criticalMultiplier.FinalValue;
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<HealthSystem>() != null) collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage, damageSource);
            Destroy(gameObject);
        }
    }

    private void SetVelocity()
    {
        rb.linearVelocity = shooterAttributes.projectileSpeed.FinalValue * transform.right;
    }
}
