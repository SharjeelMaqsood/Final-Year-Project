using UnityEngine;
using MagicPigGames;
using UnityEngine.InputSystem;
using System.Collections;

public class Health : MonoBehaviour
{
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
        controls.Player.DamageTest.performed += OnDamageTest;
    }

    void OnDisable()
    {
        controls.Player.DamageTest.performed -= OnDamageTest;
        controls.Disable();
    }

    void Start()
    {
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

    public void ChangeHealth(int amount)
    {
        if (isDead) return;

        // 🛡️ INVINCIBILITY CHECK
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
}