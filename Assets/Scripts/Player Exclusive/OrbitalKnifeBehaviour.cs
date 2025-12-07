using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OrbitalKnifeBehaviour : MonoBehaviour
{
    private AttributesSystem ownerAttributes;
    public DamageSource damageSource = DamageSource.VOID;

    public float damage = 5f;

    public void Setup(AttributesSystem ownerAttributes, DamageSource damageSource)
    {
        this.ownerAttributes = ownerAttributes;
        this.damageSource = damageSource;

        damage = ownerAttributes.attackDamage.FinalValue;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            if(IsCriticalHit(ownerAttributes)) TurnIntoCriticalHit();
            if (collision.gameObject.GetComponent<HealthSystem>() != null) collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage, damageSource);
            damage = ownerAttributes.attackDamage.FinalValue;
        }
    }

    public static bool IsCriticalHit(AttributesSystem attributes)
    {
        return Random.value < attributes.criticalChance.FinalValue || attributes.criticalChance.FinalValue >= 1f;
    }

    public void TurnIntoCriticalHit()
    {
        damage = ownerAttributes.attackDamage.FinalValue * ownerAttributes.criticalMultiplier.FinalValue;
        StartCoroutine(BlinkRed());
        SFXManager.Instance.PlaySFX(SFXManager.Instance.SFXLibrary.GetSFXByName("Pluh"));
    }

    IEnumerator BlinkRed()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        sr.color = Color.white;
    }
}
