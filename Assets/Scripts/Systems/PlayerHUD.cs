using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Text healthText;

    private void Update()
    {
        if (playerHealth != null && healthText != null)
        {
            healthText.text = $"HP: {playerHealth.GetCurrentHealth()} / {playerHealth.GetMaxHealth()}";
        }
    }
}