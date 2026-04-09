using UnityEngine;

public class EnemySwordHitbox : MonoBehaviour
{
    [Header("Attack Settings")]
    public Transform attackPoint;

    public float attackRange = 2f;   // forward length
    public float attackWidth = 1.5f; // side width
    public int damage = 10;

    [Header("Hit Control")]
    public LayerMask playerLayer;
    private bool hasHit = false;

    // Called from animation event
    public void DealDamage()
    {
        if (hasHit) return;

        hasHit = true;

        // Box position (in front of weapon)
        Vector3 center = attackPoint.position + attackPoint.forward * (attackRange / 2f);

        // Box size
        Vector3 halfExtents = new Vector3(
            attackWidth / 2f,
            attackWidth / 2f,
            attackRange / 2f
        );

        Collider[] hits = Physics.OverlapBox(
            center,
            halfExtents,
            attackPoint.rotation,
            playerLayer
        );

        Debug.Log("Enemy Hits detected: " + hits.Length);

        foreach (var hit in hits)
        {
            Debug.Log("Enemy hit: " + hit.name);

            Health player = hit.GetComponentInParent<Health>();

            if (player != null)
            {
                player.ChangeHealth(damage);
            }
        }
    }

    // Reset after attack ends
    public void ResetHit()
    {
        hasHit = false;
    }

    // Debug visualization
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;

        Vector3 center = attackPoint.position + attackPoint.forward * (attackRange / 2f);
        Vector3 size = new Vector3(attackWidth, attackWidth, attackRange);

        Gizmos.matrix = Matrix4x4.TRS(center, attackPoint.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);
    }
}