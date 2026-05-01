using UnityEngine;
using UnityEngine.AI;  // <- NavMeshAgent ada di sini

/// <summary>
/// Base class semua musuh 3D — menggunakan NavMeshAgent untuk pathfinding.
/// MeleeEnemy3D dan RangeEnemy3D inherit dari class ini.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Stats Asset")]
    public EnemyStatsSO stats;

    // Runtime values (di-scale per wave)
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float maxHealth;
    [HideInInspector] public float damage;
    [HideInInspector] public float attackCooldown;
    [HideInInspector] public float attackRange;

    protected Transform    playerTransform;
    protected NavMeshAgent agent;
    protected float        attackTimer;
    protected bool         isDead;

    [HideInInspector] public string poolTag;

    // ─── Awake: cache komponen ───────────────────────────────────────────
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // ─── Initialize dipanggil spawner setelah Get dari pool ─────────────
    public virtual void Initialize(int waveNumber = 1)
    {
        isDead      = false;
        attackTimer = 0f;

        // Scale stat sesuai wave
        float waveMult = 1f + (waveNumber - 1) * stats.healthScaling;
        maxHealth      = stats.maxHealth * waveMult;
        currentHealth  = maxHealth;
        damage         = stats.damage * (1f + (waveNumber - 1) * stats.damageScaling);
        attackCooldown = stats.attackCooldown;
        attackRange    = stats.attackRange;

        // Setup NavMeshAgent
        agent.speed           = stats.moveSpeed;
        agent.stoppingDistance = stats.stoppingDistance;
        agent.isStopped       = false;

        // Cari player
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerTransform = playerObj.transform;
    }

    protected virtual void Update()
    {
        if (isDead || playerTransform == null) return;
        attackTimer += Time.deltaTime;
        HandleBehavior();
    }

    // ─── Gerak ke target via NavMesh ─────────────────────────────────────
    protected void MoveTo(Vector3 target)
    {
        if (agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.SetDestination(target);
        }
    }

    protected void StopMoving()
    {
        if (agent.isOnNavMesh)
            agent.isStopped = true;
    }

    // Hadapkan musuh ke player (tanpa miring sumbu X/Z)
    protected void FacePlayer()
    {
        if (playerTransform == null) return;
        Vector3 dir = playerTransform.position - transform.position;
        dir.y = 0f;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    protected float DistanceToPlayer()
    {
        if (playerTransform == null) return float.MaxValue;
        return Vector3.Distance(transform.position, playerTransform.position);
    }

    // ─── Override di subclass ────────────────────────────────────────────
    protected abstract void HandleBehavior();
    protected abstract void Attack();

    // ─── Damage & Death ──────────────────────────────────────────────────
    public virtual void TakeDamage(float amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        if (currentHealth <= 0f) Die();
    }

    protected virtual void Die()
    {
        isDead = true;
        StopMoving();
        PlayerStats.Instance?.GainXP(stats.xpReward);

        // Kembalikan ke pool
        if (!string.IsNullOrEmpty(poolTag) && ObjectPool.Instance != null)
            ObjectPool.Instance.Return(poolTag, gameObject);
        else
            Destroy(gameObject);
    }

    // ─── Reset saat dikembalikan ke pool ─────────────────────────────────
    void OnDisable()
    {
        if (agent != null && agent.isOnNavMesh)
            agent.isStopped = true;
    }
}
