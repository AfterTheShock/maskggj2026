using UnityEngine;

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

    void Update()
    {
        timeLeftToShoot -= Time.deltaTime;

        if(timeLeftToShoot <= 0)
        {
            if (Input.GetMouseButton(0)) 
            {
                Shoot();
            } 
        }

        //gunHolder.localEulerAngles = Vector3.Lerp(gunHolder.localEulerAngles, new Vector3(359,0,0), Time.deltaTime * backToNormalLerpSpeed);
        gunHolder.localRotation = Quaternion.Lerp(gunHolder.localRotation, Quaternion.identity, Time.deltaTime * backToNormalLerpSpeed);
    }

    private void Shoot()
    {
        timeLeftToShoot = shootCooldown - shootCooldown * (UpgradeableStatsSingleton.Instance.fireRate - 1);

        Transform cameraTransform = Camera.main.transform;

        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity, enemyLayer))
        {
            float knockbackToGive = knockbackStrenght * UpgradeableStatsSingleton.Instance.knockback;

            if (hit.rigidbody) hit.rigidbody.AddForce(cameraTransform.forward.normalized * knockbackToGive, ForceMode.Impulse);
            
        }

        VisualRecoil();
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
