using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeToChoose : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI descriptionText;

    public UpgradeObject thisUpgrade;

    [SerializeField] Image maskImage;

    public string upgradeDescription;
    public Sprite upgradeSprite;

    public void DrawUpgradeToChoose()
    {
        descriptionText.text = upgradeDescription;

        maskImage.sprite = upgradeSprite;
    }

    public void OnClickUpgrade()
    {
        UpgradesManager.Instance.ChooseUpgrade(thisUpgrade);
    }
}
