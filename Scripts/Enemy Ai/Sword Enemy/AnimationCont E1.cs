using UnityEngine;

public class AnimationContE1: MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Movement
    public void UpdateMovement(float speed)
    {
        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }

    // Attack trigger
    public void TriggerAttack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.SetTrigger("Attack");
        }
    }
    // Optional (future use)
    public void TriggerHit()
    {
        animator.SetTrigger("Hit");
    }

    public void TriggerDie()
    {
        animator.SetTrigger("Die");
    }
}