using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
  
    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRadius = 1.5f;
    public int damage = 10;

   
    // Call this from animation event
    public void DealDamage()
    {
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRadius);

        foreach (var hit in hits)
        {
            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    // Optional debug visualization
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}