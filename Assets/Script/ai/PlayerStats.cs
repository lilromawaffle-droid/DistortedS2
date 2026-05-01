using UnityEngine;

/// <summary>
/// Simpan di GameObject Player. Berisi semua stat pemain.
/// </summary>
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Base Stats")]
    public float maxHealth       = 100f;
    public float currentHealth;
    public float moveSpeed       = 5f;
    public float defense         = 0f;   // Damage reduction flat

    [Header("Offense")]
    public float attackDamage    = 10f;
    public float attackSpeed     = 1f;   // Attacks per second
    public float attackRange     = 2f;
    public float projectileSpeed = 12f;

    [Header("Utility")]
    public float pickupRadius    = 2f;
    public float xp              = 0f;
    public int   level           = 1;
    public float xpToNextLevel   = 100f;

    // Events
    public System.Action<float, float> OnHealthChanged; // current, max
    public System.Action<int>          OnLevelUp;
    public System.Action               OnPlayerDied;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance      = this;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        float final = Mathf.Max(1f, damage - defense);
        currentHealth = Mathf.Clamp(currentHealth - final, 0f, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0f) Die();
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void GainXP(float amount)
    {
        xp += amount;
        while (xp >= xpToNextLevel) LevelUp();
    }

    void LevelUp()
    {
        xp            -= xpToNextLevel;
        level         += 1;
        xpToNextLevel *= 1.2f;
        maxHealth     += 10f;
        currentHealth  = maxHealth;
        attackDamage  += 2f;
        defense       += 0.5f;
        OnLevelUp?.Invoke(level);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log($"Level Up! Sekarang level {level}");
    }

    void Die()
    {
        OnPlayerDied?.Invoke();
        Debug.Log("Player Died!");
        // Tambahkan Game Over logic di sini
    }
}
