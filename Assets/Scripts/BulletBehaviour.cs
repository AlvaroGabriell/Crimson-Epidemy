using System;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float bulletBaseSpeed = 10f, destroyTime = 3f;
    private Rigidbody2D rb;
    public float bulletDamage = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetVelocity();

        // Destrói o projétil depois de determinado tempo.
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<HealthSystem>() != null)
            {
                collision.gameObject.GetComponent<HealthSystem>().TakeDamage(bulletDamage);
            }
            Destroy(gameObject);
        }
    }

    private void SetVelocity()
    {
        rb.linearVelocity = transform.right * bulletBaseSpeed;
    }
}
