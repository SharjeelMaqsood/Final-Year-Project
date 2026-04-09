using UnityEngine;
using System.Collections;
using MagicPigGames;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public GameObject deathUI;
    public ProgressBar progressBar;

    private Animator animator;
    private PlayerDodge dodge;
    private Controller controller;
    private PlayerCombat combat;

    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        dodge = GetComponent<PlayerDodge>();
        controller = GetComponent<Controller>();
        combat = GetComponent<PlayerCombat>();

        currentHealth = maxHealth;
        UpdateBar();

        deathUI.SetActive(false);
    }

    public void ChangeHealth(int amount)
    {
        if (isDead) return;

        if (dodge != null && dodge.isInvincible)
            return;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);

        UpdateBar();

        if (currentHealth > 0)
        {
            StartCoroutine(Stun());
        }
        else
        {
            Die();
        }
    }

    IEnumerator Stun()
    {
        combat.isStunned = true;

        animator.SetTrigger("Hit");

        yield return new WaitForSeconds(0.5f);

        combat.isStunned = false;
    }

    void UpdateBar()
    {
        float percent = (float)currentHealth / maxHealth;
        progressBar.SetProgress(1f - percent);
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        controller.enabled = false;

        deathUI.SetActive(true);
        Time.timeScale = 0f;
    }
}