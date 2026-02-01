using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;
    public float currentHealth;
    public bool isDead = false;

    [SerializeField] float healthRegenAt1Mult = 0.5f;
    
    [SerializeField] private AudioClip[] clips;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        currentHealth += Time.deltaTime * UpgradeableStatsSingleton.Instance.lifeRegeneration * healthRegenAt1Mult;

        if(currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public void TakeDamage(float enemyDamage)
    {
        float modifiedDamage = enemyDamage * (1 - UpgradeableStatsSingleton.Instance.damageResistance);

        AudioClip[] selectedClips = { clips[0] };
        AudioManager.Instance.PlaySfx(selectedClips, transform, 1f, true, true, AudioReverbPreset.Off);
        
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
        // Se frena el tiempo para evitar que el enemigo siga intentando hacer da�o al jugador
        Time.timeScale = 0f;
        Debug.Log("Moriste!");
    }
}
