using UnityEngine;

/// <summary>
/// ScriptableObject data musuh — buat satu asset per tipe musuh.
/// Klik kanan di Project → Create → Enemy System → Enemy Stats
/// </summary>
[CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemy System/Enemy Stats")]
public class EnemyStatsSO : ScriptableObject
{
    [Header("Identity")]
    public string    enemyName  = "Enemy";
    public EnemyType enemyType  = EnemyType.Melee;

    [Header("Health")]
    public float maxHealth      = 30f;

    [Header("Movement")]
    public float moveSpeed      = 3.5f;
    public float stoppingDistance = 1.5f; // NavMeshAgent stopping distance

    [Header("Offense")]
    public float damage         = 5f;
    public float attackCooldown = 1.2f;
    public float attackRange    = 2f;

    [Header("Range Only")]
    public float detectionRange   = 15f;
    public float preferredRange   = 8f;   // Jarak ideal untuk nembak
    public float projectileSpeed  = 12f;
    public GameObject projectilePrefab;

    [Header("Reward")]
    public float xpReward       = 10f;
    public int   coinReward     = 1;

    [Header("Wave Scaling")]
    public float healthScaling  = 0.1f;  // +10% HP per wave
    public float damageScaling  = 0.05f; // +5% damage per wave
}

public enum EnemyType { Melee, Range }
