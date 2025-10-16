using System;
using System.Data.Common;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float destroyTime = 3f;
    private Rigidbody2D rb;
    public AttributesSystem playerAttributes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetVelocity();

        // Destrói o projétil depois de determinado tempo.
        Destroy(gameObject, destroyTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<HealthSystem>() != null)
            {
                float damage = (Random.value < playerAttributes.criticalChance.FinalValue || playerAttributes.criticalChance.FinalValue >= 1f) ? playerAttributes.attackDamage.FinalValue * playerAttributes.criticalMultiplier.FinalValue : playerAttributes.attackDamage.FinalValue;
                collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    private void SetVelocity()
    {
        rb.linearVelocity = playerAttributes.projectileSpeed.FinalValue * transform.right;
    }
}
