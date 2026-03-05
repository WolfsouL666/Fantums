using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("=== Movement ===")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    [Header("=== Ground Check ===")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private LayerMask groundLayer;

    [Header("=== Grab / Cling ===")]
    [SerializeField] private Transform grabCheck;
    [SerializeField] private float grabCheckRadius = 0.4f;
    [SerializeField] private LayerMask clingLayer;
    [SerializeField] private float clingJumpForceX = 6f;
    [SerializeField] private float clingJumpForceY = 12f;

    [Header("=== Attack ===")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.6f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackCooldown = 0.4f;
    [SerializeField] private LayerMask enemyLayer;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private bool isClinging;
    private bool facingRight = true;
    private float attackTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapBox(
            groundCheck.position,
            new Vector2(0.4f, groundCheckRadius),
            0f,
            groundLayer
        );

        bool clingContact = Physics2D.OverlapCircle(grabCheck.position, grabCheckRadius, clingLayer);

        if (clingContact && !isGrounded && Input.GetMouseButtonDown(1) && !isClinging)
        {
            isClinging = true;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
        }

        if (isClinging && Input.GetMouseButtonUp(1))
        {
            ReleaseCling();
        }

        if (isClinging && !clingContact)
        {
            ReleaseCling();
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isClinging)
            {
                float awayDir = facingRight ? 1f : -1f;
                rb.gravityScale = 3f;
                isClinging = false;
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(new Vector2(awayDir * clingJumpForceX, clingJumpForceY), ForceMode2D.Impulse);
            }
            else if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        attackTimer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackCooldown;
        }

        if (!isClinging)
        {
            if (moveInput > 0 && !facingRight) Flip();
            else if (moveInput < 0 && facingRight) Flip();
        }
    }

    private void FixedUpdate()
    {
        if (isClinging)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = 3f;
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    private void ReleaseCling()
    {
        isClinging = false;
        rb.gravityScale = 3f;
    }

    private void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemyHP = hit.GetComponent<EnemyHealth>();
            if (enemyHP == null)
                enemyHP = hit.GetComponentInParent<EnemyHealth>();

            if (enemyHP != null)
                enemyHP.TakeDamage(attackDamage);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheck.position, new Vector3(0.4f, groundCheckRadius * 2f, 0f));
        }
        if (grabCheck != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(grabCheck.position, grabCheckRadius);
        }
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}