using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/UpgradeObject", order = 1)]
public class UpgradeObject : ScriptableObject
{
    [TextArea] public string upgradeDescription;

    public Sprite upgradeImageFull;


    [Header("Mascara del arma")]
    public float damage = 0f;
    public float fireRate = 0f;
    public float knockback = 0f;

    [Header("Mascara del player")]
    public float speed = 0f;
    public float jumpForce = 0f;
    public float damageResistance = 0f;
    public float lifeRegeneration = 0f;

    [Header("Mascara de debufos")]
    public float enemySlowness = 0f;
    public float enemyDamageReduction = 0f;
    public float enemyHealthReduction = 0f;
}
