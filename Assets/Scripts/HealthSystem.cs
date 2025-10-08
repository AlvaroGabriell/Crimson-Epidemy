using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Health")]
    private float health; //Actual Health
    public float maxHealth = 20.0f; //Max and Base Health, can be changed with setMaxHealth (will update actual health)
    public bool canDie = true;

    [Header("SFX")]
    public string damageSFX;
    public string deathSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    public void SetMaxHealth(float pMaxHealth)
    {
        maxHealth = pMaxHealth;
        health = maxHealth;
    }

    public void SetHealth(float pHealth)
    {
        health = pHealth;
    }

    public void TakeDamage(float pDamage)
    {
        health = Mathf.Max(health - pDamage, 0);

        if (!string.IsNullOrEmpty(damageSFX)) SFXManager.Play(damageSFX);

        if (ShouldDie() && canDie == true) Die();
    }

    private void Die()
    {
        if (!string.IsNullOrEmpty(deathSFX)) SFXManager.Play(deathSFX);

        Destroy(gameObject);
    }

    public void HealHealth(float pHealing)
    {
        health = Mathf.Min(health + pHealing, maxHealth);
    }

    public float GetHealth()
    {
        return health;
    }

    public bool ShouldDie()
    {
        if (health <= 0) return true;
        return false;
    }
    
}
