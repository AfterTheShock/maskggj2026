using TMPro;
using UnityEngine;

public class UserInterfaceManager : MonoBehaviour
{
    #region SingletonPattern
    private static UserInterfaceManager _instance;

    public static UserInterfaceManager Instance { get { return _instance; } }


    private void Awake()
    {
        _instance = this;
    }
    #endregion
    
    [SerializeField] private TextMeshProUGUI ammoText;

    [SerializeField] GameObject hitMarker;

    public void UpdateAmmoText(int leftSideAmmo, int rightSideAmmo)
    {
        ammoText.text = $"{leftSideAmmo}/{rightSideAmmo}";
    }

    public void ReloadingEffect(int rightSideAmmo)
    {
        ammoText.color = Color.red;
    }

    public void NormalEffect()
    {
        ammoText.color = Color.white;
    }

    public void ShowHitMarker()
    {
        if (hitMarker == null) return;

        GameObject newHitMarker = Instantiate(hitMarker);

        newHitMarker.transform.SetParent(transform.GetChild(0));
        newHitMarker.transform.localPosition = Vector3.zero;
    }
}
