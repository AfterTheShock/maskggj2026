using Unity.Cinemachine;
using UnityEngine;


public class PlayerShootingManager : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;

    [SerializeField] float shootCooldown = 0.5f;
    [SerializeField] float knockbackStrenght = 15;
    [SerializeField] float baseDamage = 25;
    private float timeLeftToShoot;

    [Header("GunVisuals")]
    [SerializeField] Transform gunHolder;
    [SerializeField] Vector3 reloadRotation = new Vector3(25,0,0);
    [SerializeField] Vector3 shootRecoilRotation = new Vector3(350,0,0);
    [SerializeField] Vector3 shootRecoilRandomRotation = new Vector3(5,5,5);
    [SerializeField] float backToNormalLerpSpeed = 15;
    [SerializeField] CinemachineImpulseSource shootImpulseSource;
    [SerializeField] ParticleSystem muzzelFlash;
    [SerializeField] GameObject muzzelFlashObject;
    [SerializeField] Animator gunAnimator;
    [SerializeField] string reloadAnimationName = "reload";

    [Header("Gun Func")]
    [SerializeField] private int magazineAmmo;
    [SerializeField] private int totalAmmo;
    [SerializeField] private float reloadCooldown;
    private int currentMagazineAmmo;
    private float currentReloadCooldown = 0;
    private bool reloading;

    [Header("Hit particles")]
    [SerializeField] LayerMask hitParticlesLayer;
    [SerializeField] GameObject hitEnemyParticlePrefab;
    [SerializeField] GameObject hitSomethingParticlePrefab;

    [SerializeField] private AudioClip[] clips;
    
    #region SingletonPattern
    private static PlayerShootingManager _instance;

    public static PlayerShootingManager Instance { get { return _instance; } }


    private void Awake()
    {
        _instance = this;
    }
    #endregion

    private void Start()
    {
        currentMagazineAmmo = magazineAmmo;
        UserInterfaceUpdate();
    }

    void Update()
    {
        timeLeftToShoot -= Time.deltaTime;
        
        currentReloadCooldown -= Time.deltaTime;
        if (currentReloadCooldown > 0) reloading = true;
        else reloading = false;

        if (reloading) UserInterfaceManager.Instance.ReloadingEffect(totalAmmo);
        else UserInterfaceManager.Instance.NormalEffect();
        
        if (Input.GetKeyDown(KeyCode.R)) Reload();
        
        if(timeLeftToShoot <= 0)
        {
            if (Input.GetMouseButton(0) && Time.timeScale != 0) Shoot();
        }

        //gunHolder.localEulerAngles = Vector3.Lerp(gunHolder.localEulerAngles, new Vector3(359,0,0), Time.deltaTime * backToNormalLerpSpeed);
        if (currentReloadCooldown >= reloadCooldown * 0.5f)
            gunHolder.localRotation = Quaternion.Lerp(gunHolder.localRotation, Quaternion.Euler(reloadRotation), Time.deltaTime * backToNormalLerpSpeed);
        else
            gunHolder.localRotation = Quaternion.Lerp(gunHolder.localRotation, Quaternion.identity, Time.deltaTime * backToNormalLerpSpeed);
    }

    private void Shoot()
    {
        if (reloading) return;
        if (currentMagazineAmmo <= 0)
        {
            Debug.Log("Shoot(): sin balas en el cargador.");
            Reload();
            return;
        }

        timeLeftToShoot = shootCooldown - shootCooldown * (UpgradeableStatsSingleton.Instance.fireRate - 1);

        var cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Shoot(): no se encontr� Camera.main");
            return;
        }

        Transform cameraTransform = cam.transform;
        Vector3 origin = cameraTransform.position;
        Vector3 dir = cameraTransform.forward;

        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, Mathf.Infinity, enemyLayer))
        {
            float knockbackToGive = knockbackStrenght * UpgradeableStatsSingleton.Instance.knockback;
            float damageToGive = baseDamage * UpgradeableStatsSingleton.Instance.damage;

            Enemy enemy = hit.collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageToGive);
                if(enemy != null && enemy.enemyHealth > 0) enemy.ApplyKnockback(dir * knockbackToGive);
            }

            SpawnHitSomethingParticle(hit.point, hit.normal, hit.transform, true);
            UserInterfaceManager.Instance.ShowHitMarker();
        }
        else if (Physics.Raycast(origin, dir, out hit, Mathf.Infinity, hitParticlesLayer))
        {
            SpawnHitSomethingParticle(hit.point,hit.normal);
        }

        VisualRecoil();


        if (muzzelFlash) muzzelFlash.Play();
        if (muzzelFlashObject) muzzelFlashObject.SetActive(true);

        currentMagazineAmmo--;

        UserInterfaceUpdate();

        AudioClip[] selectedClips = {  clips[0], clips[1] };
        AudioManager.Instance.PlaySfx(selectedClips, gameObject.transform, 1f, true, true, AudioReverbPreset.Off);
    }

    private void SpawnHitSomethingParticle(Vector3 position, Vector3 hitNormal, Transform hitTransform = null, bool hitWasEnemy = false)
    {
        GameObject hitParticle;

        if (hitWasEnemy)
        {
            if (hitEnemyParticlePrefab == null) return;
            hitParticle = Instantiate(hitEnemyParticlePrefab);
            hitParticle.transform.SetParent(hitTransform);
        }
        else
        {
            if (hitSomethingParticlePrefab == null) return;
            hitParticle = Instantiate(hitSomethingParticlePrefab);
        }

        hitParticle.transform.position = position;
        hitParticle.transform.forward = hitNormal;
    }

    public void GetTotalAmmo(float ammountOfMagazinesToGet)
    {
        totalAmmo += (int)(magazineAmmo * ammountOfMagazinesToGet);
        UserInterfaceUpdate();
    }

    private void Reload()
    {
        if (reloading) return;
        if (totalAmmo <= 0) return;    // No hay municion en la recamara

        if (gunAnimator) gunAnimator.Play(reloadAnimationName);

        int ammoDifference = magazineAmmo - currentMagazineAmmo;
        if(ammoDifference > totalAmmo) ammoDifference = totalAmmo;
        currentMagazineAmmo += ammoDifference;
        totalAmmo -= ammoDifference;

        currentReloadCooldown = reloadCooldown;
        
        UserInterfaceUpdate();
        
        
        AudioClip[] selectedClips = {  clips[2]};
        AudioManager.Instance.PlaySfx(selectedClips, gameObject.transform, 1f, true, true, AudioReverbPreset.Off);
    }

    private void UserInterfaceUpdate()
    {
        UserInterfaceManager.Instance.UpdateAmmoText(currentMagazineAmmo, totalAmmo);
    }

    private void VisualRecoil()
    {
        gunHolder.localEulerAngles = shootRecoilRotation;

        gunHolder.localEulerAngles += new Vector3(
            Random.Range(-shootRecoilRandomRotation.x, shootRecoilRandomRotation.x),
            Random.Range(-shootRecoilRandomRotation.y, shootRecoilRandomRotation.y),
            Random.Range(-shootRecoilRandomRotation.z, shootRecoilRandomRotation.z)
            );

        shootImpulseSource.GenerateImpulse();
    }
}
