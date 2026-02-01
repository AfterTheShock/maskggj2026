using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarManager : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI HealthPoints; // Referencia al texto
    [SerializeField] private PlayerHealth player;

    private void Update()
    {
        if (player != null)
        {
            if (healthSlider != null)
            {
                healthSlider.maxValue = player.maxHealth;
                healthSlider.value = player.currentHealth;
            }

            if (HealthPoints != null)
            {
                // CeilToInt para que no muestre decimales
                int current = Mathf.CeilToInt(player.currentHealth);
                int max = Mathf.CeilToInt(player.maxHealth);

                HealthPoints.text = $"{current} / {max}";
            }
        }
    }
}