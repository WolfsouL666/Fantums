using UnityEngine;

public class DoorTransition : MonoBehaviour
{
    [Header("=== Destination ===")]
    [SerializeField] private Transform teleportTarget;

    private bool playerInRange;
    private GameObject playerObject;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (teleportTarget == null)
            {
                Debug.LogError("DoorTransition: No teleport target assigned!");
                return;
            }

            if (playerObject == null) return;

            Rigidbody2D rb = playerObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }

            playerObject.transform.position = teleportTarget.position;
            Debug.Log("Player teleported!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"DoorTrigger hit by: {other.gameObject.name} | Tag: {other.tag}");

        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerObject = other.gameObject;
            Debug.Log("Press F to enter door.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerObject = null;
        }
    }
}