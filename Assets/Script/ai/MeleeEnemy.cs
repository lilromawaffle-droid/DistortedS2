using UnityEngine;

/// <summary>
/// Musuh Melee 3D — kejar player via NavMesh, serang saat sudah dekat.
/// </summary>
public class MeleeEnemy : EnemyBase
{
    protected override void HandleBehavior()
    {
        float dist = DistanceToPlayer();
        FacePlayer();

        if (dist <= attackRange)
        {
            // Dalam jangkauan — berhenti dan serang
            StopMoving();
            if (attackTimer >= attackCooldown)
            {
                Attack();
                attackTimer = 0f;
            }
        }
        else
        {
            // Kejar player
            MoveTo(playerTransform.position);
        }
    }

    protected override void Attack()
    {
        // Double-check jarak sebelum damage (cegah ghost hit)
        if (DistanceToPlayer() <= attackRange + 0.3f)
        {
            PlayerStats.Instance?.TakeDamage(damage);
        }
    }
}
