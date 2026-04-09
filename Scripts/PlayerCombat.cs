using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    public int attackDamage = 10;
    public float attackRange = 1.5f;
    public float attackWidth = 1f;
    public float attackCooldown = 1f;

    public Animator animator;
    public Transform attackPoint;

    public LayerMask enemyLayer;

    public bool hasWeapon = false;

    public bool isAttacking { get; private set; }
    public bool isStunned { get; set; }

    private bool canAttack = true;
    private PlayerDodge dodge;

    void Start()
    {
        dodge = GetComponent<PlayerDodge>();
    }

    public void OnAttack(InputValue value)
    {
        if (!value.isPressed) return;

        if (!canAttack || !hasWeapon) return;
        if (isStunned) return;
        if (dodge != null && dodge.isDodging) return;

        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        canAttack = false;

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.2f);

        DealDamage();

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        canAttack = true;
    }

    void DealDamage()
    {
        Vector3 center = attackPoint.position + attackPoint.forward * (attackRange / 2f);
        Vector3 halfExtents = new Vector3(attackWidth / 2f, attackWidth / 2f, attackRange / 2f);

        Collider[] hits = Physics.OverlapBox(center, halfExtents, attackPoint.rotation, enemyLayer);

        foreach (var hit in hits)
        {
            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
                enemy.TakeDamage(attackDamage);
        }
    }
    public void EquipWeapon()
    {
        hasWeapon = true;
        Debug.Log("Weapon Equipeed");
    }
}
