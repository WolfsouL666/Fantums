using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("=== Health ===")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float invincibilityTime = 1f;

    private int currentHealth;
    private bool isInvincible;
    private float invincibilityTimer;
    private SpriteRenderer sr;

    private void Awake()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;

            
            sr.color = new Color(1f, 1f, 1f, Mathf.PingPong(Time.time * 10f, 1f));

            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
                sr.color = Color.blue; 
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        Debug.Log($"Player HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        isInvincible = true;
        invincibilityTimer = invincibilityTime;
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        
        transform.position = new Vector3(-8f, -2.5f, 0f);
        currentHealth = maxHealth;
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}