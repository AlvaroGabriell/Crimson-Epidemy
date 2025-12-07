using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BossBehaviourHelper : MonoBehaviour
{
    private BossBehaviour boss;
    public Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        boss = GetComponentInParent<BossBehaviour>();
    }

    public void HeadSlam() // Chamada no animator quando a cabeça do boss toca o chão
    {
        boss.isOnGround = true;
        boss.PerformHeadSlam();
    }

    public void GetOffGround() // Chamada no animator quando a cabeça do boss deixa o chão
    {
        boss.isOnGround = false;
    }

    public void EndAttack() // Chamada no animator quando a animação de ataque acaba
    {
        Debug.Log("Aasasa");
        boss.isAttacking = false;
    }
}
