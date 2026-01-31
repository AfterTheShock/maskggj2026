using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;
    public float currentHealth;
    public bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float enemyDamage)
    {
        float modifiedDamage = enemyDamage * (1 - UpgradeableStatsSingleton.Instance.damageResistance);

        if (currentHealth <= modifiedDamage) {
            currentHealth = 0;
            Die();
        } else
        {
            currentHealth = currentHealth - modifiedDamage;
            Debug.Log("Vida actual: " + currentHealth + "/" + maxHealth);
        }
    }

    void Die()
    {
        isDead = true;
        // Se frena el tiempo para evitar que el enemigo siga intentando hacer daño al jugador
        Time.timeScale = 0f;
        Debug.Log("Moriste!");
    }
}
