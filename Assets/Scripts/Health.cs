using UnityEngine;
using MagicPigGames;
using UnityEngine.InputSystem;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public ProgressBar progressBar; // Drag Horizontal Progress Bar here

    private PlayerInputActions controls;

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
        currentHealth = maxHealth;
        UpdateBar(); // initialize bar
    }

    private void OnDamageTest(InputAction.CallbackContext ctx)
    {
        ChangeHealth(10);
    }

    public void ChangeHealth(int amount)
    {
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

        // If your bar is inverted, keep this:
        progressBar.SetProgress(1f - percent);

        // If you FIX inversion in inspector, use this instead:
        // progressBar.SetProgress(percent);
    }

    void Die()
    {
        Debug.Log("Player Died");
    }
}