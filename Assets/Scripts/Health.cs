using UnityEngine;
using MagicPigGames;
using UnityEngine.InputSystem;
using System.Collections;

public class Health : MonoBehaviour
{
    private Animator animator;
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public GameObject deathUI;
    public ProgressBar progressBar;

    private CanvasGroup deathCanvasGroup;

    [Header("References")]
    private PlayerDodge dodge;
    private Controller controller;

    private PlayerInputActions controls;

    private bool isDead = false;

    void Awake()
    {
        controls = new PlayerInputActions();
    }

    void OnEnable()
    {
        controls.Enable();
        
    }

    void OnDisable()
    {
        
        controls.Disable();
    }

    void Start()
    {
        animator = GetComponent<Animator>();    
        dodge = GetComponent<PlayerDodge>();
        controller = GetComponent<Controller>();

        currentHealth = maxHealth;

        UpdateBar();

        deathCanvasGroup = deathUI.GetComponent<CanvasGroup>();
        deathCanvasGroup.alpha = 0f;
        deathUI.SetActive(false);
    }

    private void OnDamageTest(InputAction.CallbackContext ctx)
    {
        ChangeHealth(10);
    }

    void OnDamageTest()
    {
        ChangeHealth(10);
    }
    public void ChangeHealth(int amount)
    {
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }
        if (isDead) return;


       // StartCoroutine(HitPause());

        if (dodge != null && dodge.isInvincible)
        {
            Debug.Log("NO DAMAGE (INVINCIBLE)");
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);

        Debug.Log("Health: " + currentHealth);

        UpdateBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateBar()
    {
        float percent = (float)currentHealth / maxHealth;

        // keep inverted if your bar needs it
        progressBar.SetProgress(1f - percent);
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        deathUI.SetActive(true);

        StartCoroutine(FadeInDeathUI());

        if (controller != null)
            controller.enabled = false;
    }

    IEnumerator FadeInDeathUI()
    {
        float duration = 1.5f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            deathCanvasGroup.alpha = time / duration;
            yield return null;
        }

        deathCanvasGroup.alpha = 1f;

        Time.timeScale = 0f;
    }
    IEnumerator HitPause()
    {
        Time.timeScale = 0.3f;
        yield return new WaitForSecondsRealtime(0.03f);
        Time.timeScale = 1f;
    }
}