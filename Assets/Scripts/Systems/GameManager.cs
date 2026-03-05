using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Transform respawnPoint;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public Vector3 GetRespawnPosition()
    {
        return respawnPoint != null ? respawnPoint.position : new Vector3(-8f, -2.5f, 0f);
    }
}