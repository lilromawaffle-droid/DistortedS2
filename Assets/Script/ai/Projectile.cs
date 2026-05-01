using UnityEngine;

/// <summary>
/// Peluru musuh Range 3D. Bergerak lurus dan kembali ke pool saat kena player.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public string poolTag  = "EnemyProjectile";
    public float  damage   = 5f;
    public float  speed    = 12f;
    public float  lifetime = 5f;

    private Rigidbody rb;
    private float     timer;

    void Awake()
    {
        rb                  = GetComponent<Rigidbody>();
        rb.useGravity        = false;
        rb.constraints       = RigidbodyConstraints.FreezeRotation;
        rb.isKinematic       = false;
    }

    void OnEnable()
    {
        timer = 0f;
    }

    public void Launch(Vector3 direction, float spd, float dmg)
    {
        speed  = spd;
        damage = dmg;
        timer  = 0f;
        rb.linearVelocity = direction.normalized * speed;
        transform.forward = direction.normalized;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime) ReturnToPool();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerStats.Instance?.TakeDamage(damage);
            ReturnToPool();
        }
        else if (!col.CompareTag("Enemy") && !col.isTrigger)
        {
            // Kena dinding atau obstacle — hilangkan
            ReturnToPool();
        }
    }

    void ReturnToPool()
    {
        rb.linearVelocity = Vector3.zero;
        if (ObjectPool.Instance != null)
            ObjectPool.Instance.Return(poolTag, gameObject);
        else
            Destroy(gameObject);
    }
}
