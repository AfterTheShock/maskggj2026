using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    [SerializeField] float ammountOfMagazinesToGet = 1.5f;

    private bool alreadyGrabbed = false;

    public void GrabAmmoBox()
    {
        if(alreadyGrabbed) return;
        alreadyGrabbed = true;

        PlayerShootingManager.Instance.GetTotalAmmo(ammountOfMagazinesToGet);
    }
}
