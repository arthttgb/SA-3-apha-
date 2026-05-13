using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI")]
    public TextMeshProUGUI healthText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0f)
            currentHealth = 0f;

        UpdateUI();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void UpdateUI()
    {
        if (healthText != null)
            healthText.text = "Vida: " + currentHealth.ToString("0");
    }

    void Die()
    {
        Debug.Log("Jogador morreu!");
        gameObject.SetActive(false);
    }
}