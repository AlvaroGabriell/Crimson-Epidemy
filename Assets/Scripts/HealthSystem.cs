using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Health")]
    private float health; 
    [SerializeField] private float maxHealth = 20f;
    public bool canDie = true, canRegen = false, regenActive = false, canTakeDamage = true, isAlive = true;
    public AttributesSystem attributes;

    [Header("SFX")]
    public string damageSFX;
    public string deathSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameObject.GetComponent<AttributesSystem>() != null) SetMaxHealthAndFullHeal(gameObject.GetComponent<AttributesSystem>().maxHealth.FinalValue);
        else health = maxHealth;
    }

    public void SetMaxHealth(float pMaxHealth)
    {
        maxHealth = pMaxHealth;
    }
    public void SetMaxHealthAndFullHeal(float pMaxHealth)
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
        if (!canTakeDamage || !isAlive) return;

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
        return health <= 0;
    }

    public void StartOrStopRegen()
    {
        if (regenActive) StartCoroutine(Regen());
        else StopCoroutine(Regen());
    }
    
    // Player-only method. Should not be called in non-player object, although it is possible.
    private IEnumerator Regen()
    {
        while (true)
        {
            if (isAlive && canRegen)
            {
                HealHealth(attributes.healthRegen.FinalValue);
            }

            float regenInterval = 5f / attributes.regenSpeed.FinalValue;

            yield return new WaitForSeconds(regenInterval);
        }
    }
}
