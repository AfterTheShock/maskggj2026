using UnityEngine;

public class UpgradeableStatsSingleton : MonoBehaviour
{
    #region SingletonPattern
    private static UpgradeableStatsSingleton _instance;

    public static UpgradeableStatsSingleton Instance { get { return _instance; } }


    private void Awake()
    {
        _instance = this;
    }
    #endregion

    [Header("Mascara del arma")]
    public float damage = 1f;
    public float fireRate = 1f;
    public float knockback = 0.1f;

    [Header("Mascara del player")]
    public float speed = 1f;
    public float jumpForce = 1f;
    public float damageResistance = 0f;
    public float lifeRegeneration = 0.1f;

    [Header("Mascara de debufos")]
    public float enemySlowness = 0f;
    public float enemyDamageReduction = 0f;
    public float enemyHealthReduction = 0f;
}
