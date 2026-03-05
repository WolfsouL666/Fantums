using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private float damageCooldown = 1f;

    private float cooldownTimer;

    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[EnemyHitbox] ENTER detected: {other.gameObject.name} | Tag: {other.tag} | Layer: {LayerMask.LayerToName(other.gameObject.layer)}");
        TryDamagePlayer(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryDamagePlayer(other);
    }

    private void TryDamagePlayer(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (cooldownTimer > 0f) return;

        PlayerHealth hp = other.GetComponent<PlayerHealth>();
        if (hp != null)
        {
            hp.TakeDamage(contactDamage);
            Debug.Log($"[EnemyHitbox] HIT PLAYER for {contactDamage} damage!");
        }
        else
        {
            Debug.LogWarning("[EnemyHitbox] Player has no PlayerHealth component!");
        }

        Rigidbody2D playerRB = other.GetComponent<Rigidbody2D>();
        if (playerRB != null)
        {
            Vector2 knockDir = (other.transform.position - transform.parent.position).normalized;
            knockDir.y = 0.5f;
            playerRB.linearVelocity = Vector2.zero;
            playerRB.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);
        }

        cooldownTimer = damageCooldown;
    }
}