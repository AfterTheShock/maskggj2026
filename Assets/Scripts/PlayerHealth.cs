using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float playerHealth = 100;

    public void TakeDamage(float enemyDamage)
    {
        if (playerHealth <= enemyDamage) {
            playerHealth = 0;
            Debug.Log("Moriste!");
        } else
        {
            playerHealth = playerHealth - enemyDamage;
            Debug.Log("Vida actual: " +  playerHealth + "/100");
        }
    }
}
