using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/** <summary>
* Script da vida. Seta um valor de vida máximo para o objeto, com variáveis pra controlar se
* o objeto pode tomar dano, pode morrer e se está vivo. Tem também métodos pra mudar a vida,
* pegar, curar e dar dano. 
* </summary> **/
public class HealthSystem : MonoBehaviour
{
    [Header("Health")]
    private float health; 
    [SerializeField] private float maxHealth = 20f;
    public bool canDie = true, canRegen = true, regenActive = false, canTakeDamage = true, isAlive = true;
    public AttributesSystem attributes;

    [Header("SFX")]
    public string damageSFX;
    public string deathSFX;

    public event Action OnDeath;

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
        HealFullHealth();
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

        isAlive = false;

        OnDeath?.Invoke();
    }

    public void HealHealth(float pHealing)
    {
        health = Mathf.Min(health + pHealing, maxHealth);
    }
    public void HealFullHealth()
    {
        health = maxHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    public bool ShouldDie()
    {
        return health <= 0;
    }

    public void StartRegen()
    {
        StartCoroutine(Regen());
    }
    public void StopRegen()
    {
        StopCoroutine(Regen());
    }
    
    // Should not be called in non-player object, although it is possible.
    private IEnumerator Regen()
    {
        while (true)
        {
            if (isAlive && canRegen)
            {
                HealHealth(attributes.healthRegen.FinalValue);
            }

            float regenInterval = Mathf.Max(5f / attributes.regenSpeed.FinalValue, 0f);
            yield return new WaitForSeconds(regenInterval);
        }
    }
}
