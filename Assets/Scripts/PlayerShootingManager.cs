using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerShootingManager : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;

    [SerializeField] float shootCooldown = 0.5f;
    [SerializeField] float knockbackStrenght = 15;
    private float timeLeftToShoot;

    [Header("GunVisuals")]
    [SerializeField] Transform gunHolder;
    [SerializeField] Vector3 shootRecoilRotation = new Vector3(350,0,0);
    [SerializeField] Vector3 shootRecoilRandomRotation = new Vector3(5,5,5);
    [SerializeField] float backToNormalLerpSpeed = 15;
    
    [Header("Gun Func")]
    [SerializeField] private int magazineAmmo;
    [SerializeField] private int totalAmmo;
    [SerializeField] private float reloadCooldown;
    private int currentMagazineAmmo;
    private float currentReloadCooldown = 0;
    private bool reloading;

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
            if (Input.GetMouseButton(0)) Shoot();
        }

        //gunHolder.localEulerAngles = Vector3.Lerp(gunHolder.localEulerAngles, new Vector3(359,0,0), Time.deltaTime * backToNormalLerpSpeed);
        gunHolder.localRotation = Quaternion.Lerp(gunHolder.localRotation, Quaternion.identity, Time.deltaTime * backToNormalLerpSpeed);
    }

    private void Shoot()
    {
        if (reloading) return;
        if(currentMagazineAmmo <= 0) return;
        
        timeLeftToShoot = shootCooldown - shootCooldown * (UpgradeableStatsSingleton.Instance.fireRate - 1);

        Transform cameraTransform = Camera.main.transform;

        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity, enemyLayer))
        {
            float knockbackToGive = knockbackStrenght * UpgradeableStatsSingleton.Instance.knockback;

            if (hit.rigidbody) hit.rigidbody.AddForce(cameraTransform.forward.normalized * knockbackToGive, ForceMode.Impulse);
            
        }

        VisualRecoil();
        
        currentMagazineAmmo--;
        
        UserInterfaceUpdate();
    }

    private void Reload()
    {
        if (reloading) return;
        if (totalAmmo <= currentMagazineAmmo) return;    // No hay municion en la recamara
        
        int ammoDifference = magazineAmmo - currentMagazineAmmo;
        currentMagazineAmmo = magazineAmmo;
        totalAmmo -= ammoDifference;

        currentReloadCooldown = reloadCooldown;
        
        UserInterfaceUpdate();
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
    }
}
