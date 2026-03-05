using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("=== Patrol Points ===")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("=== Settings ===")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float arrivalThreshold = 0.3f;
    [SerializeField] private float waitTime = 0f;

    private Transform currentTarget;
    private Rigidbody2D rb;
    private float waitTimer;
    private bool waiting;
    private Vector3 baseScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        baseScale = transform.localScale;
    }

    private void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("EnemyPatrol: Assign both PatrolPoint_A and PatrolPoint_B!");
            enabled = false;
            return;
        }

        currentTarget = pointB;
        FaceTarget();
    }

    private void FixedUpdate()
    {
        if (waiting)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            waitTimer -= Time.fixedDeltaTime;
            if (waitTimer <= 0f)
            {
                waiting = false;
            }
            return;
        }

        float dirX = Mathf.Sign(currentTarget.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(dirX * moveSpeed, rb.linearVelocity.y);

        float distX = Mathf.Abs(transform.position.x - currentTarget.position.x);
        if (distX <= arrivalThreshold)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
            FaceTarget();

            if (waitTime > 0f)
            {
                waiting = true;
                waitTimer = waitTime;
            }
        }
    }

    private void FaceTarget()
    {
        float dirX = Mathf.Sign(currentTarget.position.x - transform.position.x);
        Vector3 s = baseScale;
        s.x = Mathf.Abs(baseScale.x) * dirX;
        transform.localScale = s;
    }

    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 a = pointA.position;
            Vector3 b = pointB.position;
            a.y = transform.position.y;
            b.y = transform.position.y;
            Gizmos.DrawLine(a, b);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pointA.position, 0.3f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(pointB.position, 0.3f);
        }
    }
}