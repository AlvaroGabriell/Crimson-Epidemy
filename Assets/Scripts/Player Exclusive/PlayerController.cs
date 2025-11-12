using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthSystem))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AttributesSystem))]
[RequireComponent(typeof(LevelSystem))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private HealthSystem health;
    private LevelSystem level;
    [HideInInspector]
    public AttributesSystem attributes;
    private float horizontalMovement = 0f, verticalMovement = 0f;
    private bool isMoving = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<HealthSystem>();
        animator = GetComponent<Animator>();
        level = GetComponent<LevelSystem>();
        attributes = GetComponent<AttributesSystem>();

        // Setting attributes
        attributes.maxHealth.SetBaseValue(20f);
        attributes.healthRegen.SetBaseValue(0f);
        attributes.regenSpeed.SetBaseValue(2f);
        attributes.moveSpeed.SetBaseValue(3f);
        attributes.attackDamage.SetBaseValue(5f);
        attributes.attackSpeed.SetBaseValue(1f);
        attributes.projectileSpeed.SetBaseValue(8f);
        attributes.criticalChance.SetPercentValue(10f);
        attributes.criticalMultiplier.SetBaseValue(2f);
        attributes.pickupRange.SetBaseValue(2.5f);

        health.attributes = attributes;
        health.SetMaxHealthAndFullHeal(attributes.maxHealth.FinalValue);
    }

    void FixedUpdate()
    {
        HandleMovement();
        UpdateValues();
    }

    void UpdateValues()
    {
        // ---------- Animator ----------
        isMoving = Mathf.Abs(horizontalMovement) > 0f || Mathf.Abs(verticalMovement) > 0f;
        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("moveSpeed", attributes.moveSpeed.FinalValue / 3);

        // ---------- Health ----------
        health.SetMaxHealth(attributes.maxHealth.FinalValue);
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
        if (collision.CompareTag("Collectable"))
        {
            CollectableBehaviour collectable = collision.GetComponent<CollectableBehaviour>();
            switch (collectable.GetCollectableType())
            {
                case CollectableType.Xp:
                    level.AddXp(collectable.GetValue());
                    break;
                case CollectableType.Health:
                    health.HealHealth(collectable.GetValue());
                    //TODO: Tocar sfx de coletar vida
                    break;
            }
            Destroy(collision.gameObject);
        }
    }

    //Calcula e executa o movimento do jogador.
    public void HandleMovement()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * attributes.moveSpeed.FinalValue, verticalMovement * attributes.moveSpeed.FinalValue);

        if (horizontalMovement > 0f) transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (horizontalMovement < 0f) transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    //Captura o input de movimento
    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        verticalMovement = context.ReadValue<Vector2>().y;
    }

    public void PlaySFX(string sfxName)
    {
        SFXManager.Play(sfxName);
    }
}
