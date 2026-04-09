using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private Animator animator;
    private Rigidbody rb;

    [Header("Knockback")]
    public float knockbackForce = 3f;
    public Transform player;

    private bool isDead = false;

    public bool IsDead => isDead;
    public bool IsStunned { get; private set; }
    [SerializeField] private float stunDuration = 0.6f;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (player == null && GameObject.FindWithTag("Player") != null)
            player = GameObject.FindWithTag("Player").transform;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " took damage: " + damage + " | HP left: " + currentHealth);

        if (animator != null)
            animator.SetTrigger("Hit");

        if (currentHealth > 0)
        {
            StartCoroutine(StunRoutine());
        }

        if (rb != null && player != null)
        {
            Vector3 knockDir = (transform.position - player.position).normalized;
            knockDir.y = 0f;
            rb.AddForce(knockDir * knockbackForce, ForceMode.Impulse);
        }

        if (currentHealth <= 0)
            Die();
    }

    IEnumerator StunRoutine()
    {
        IsStunned = true;
        yield return new WaitForSeconds(stunDuration);
        IsStunned = false;
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log(gameObject.name + " died");

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        if (animator != null)
            animator.SetTrigger("Die");

        Destroy(gameObject, 2.6f);
    }
}