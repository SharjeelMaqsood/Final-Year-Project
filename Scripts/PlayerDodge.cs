using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerDodge : MonoBehaviour
{
    public float invincibilityTime = 0.6f;
    public float cooldown = 2f;

    public float aoeRadius = 3f;
    public int aoeDamage = 20;

    public Animator animator;
    public GameObject effect;

    public bool isInvincible { get; private set; }
    public bool isDodging { get; private set; }

    private bool canUse = true;

    public void OnDodge(InputValue value)
    {
        if (value.isPressed && canUse)
            StartCoroutine(DodgeAbility());
    }

    IEnumerator DodgeAbility()
    {
        canUse = false;
        isInvincible = true;
        isDodging = true;

        animator.SetTrigger("Dodge");

        if (effect != null)
            Instantiate(effect, transform.position, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(transform.position, aoeRadius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
                hit.GetComponent<EnemyHealth>()?.TakeDamage(aoeDamage);
        }

        yield return new WaitForSeconds(invincibilityTime);

        isInvincible = false;
        isDodging = false;

        yield return new WaitForSeconds(cooldown);

        canUse = true;
    }
}