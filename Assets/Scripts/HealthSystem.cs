using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private float health; //Actual Health
    public float maxHealth = 20.0f; //Max and Base Health, can be changed with setMaxHealth (will update actual health)
    public bool canDie = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMaxHealth(float pMaxHealth)
    {
        this.maxHealth = pMaxHealth;
        health = maxHealth;
    }

    public void SetHealth(float pHealth)
    {
        health = pHealth;
    }

    public void TakeDamage(float pDamage)
    {
        health = Mathf.Max(health - pDamage, 0);

        if (ShouldDie() && canDie == true) Destroy(gameObject);
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
