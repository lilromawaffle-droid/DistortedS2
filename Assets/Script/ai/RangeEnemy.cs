using UnityEngine;

/// <summary>
/// Musuh Range 3D — jaga jarak ideal dari player dan tembak peluru.
/// Mundur jika player terlalu dekat.
/// </summary>
public class RangeEnemy : EnemyBase
{
    [Header("Range Specific")]
    public string projectilePoolTag = "EnemyProjectile";
    public Transform firePoint;  // Assign titik keluar peluru (opsional)

    private float detectionRange;
    private float preferredRange;
    private float projectileSpeed;

    public override void Initialize(int waveNumber = 1)
    {
        base.Initialize(waveNumber);
        detectionRange = stats.detectionRange;
        preferredRange = stats.preferredRange;
        projectileSpeed = stats.projectileSpeed;
    }

    protected override void HandleBehavior()
    {
        float dist = DistanceToPlayer();
        FacePlayer();

        if (dist > detectionRange)
        {
            // Terlalu jauh — mendekati player
            MoveTo(playerTransform.position);
        }
        else if (dist < attackRange)
        {
            // Terlalu dekat — mundur
            Vector3 fleeDir = (transform.position - playerTransform.position).normalized;
            Vector3 fleeTarget = transform.position + fleeDir * 5f;
            MoveTo(fleeTarget);
        }
        else
        {
            // Jarak ideal — diam dan tembak
            StopMoving();
            if (attackTimer >= attackCooldown)
            {
                Attack();
                attackTimer = 0f;
            }
        }
    }

    protected override void Attack()
    {
        if (playerTransform == null) return;

        // Titik tembak: pakai firePoint jika ada, kalau tidak pakai posisi musuh
        Vector3 origin = firePoint != null ? firePoint.position : transform.position + Vector3.up * 0.5f;

        // Arah ke player (dengan sedikit lead prediction opsional)
        Vector3 dir = (playerTransform.position + Vector3.up * 0.5f - origin).normalized;

        GameObject projObj = ObjectPool.Instance != null
            ? ObjectPool.Instance.Get(projectilePoolTag, origin, Quaternion.identity)
            : (stats.projectilePrefab != null ? Instantiate(stats.projectilePrefab, origin, Quaternion.identity) : null);

        if (projObj == null) return;

        var proj = projObj.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.poolTag = projectilePoolTag;
            proj.Launch(dir, projectileSpeed, damage);
        }
    }
}
