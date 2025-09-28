using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private float health; //Actual Health
    public float maxHealth = 20.0f; //Max and Base Health, can be changed with setMaxHealth (will update actual health)

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

    public void DealDamage(float pDamage)
    {
        health = Mathf.Min(health - pDamage, 0);
    }

    public void HealHealth(float pHealing)
    {
        health = Mathf.Max(health + pHealing, maxHealth);
    }

    public float GetHealth()
    {
        return health;
    }
    
}
