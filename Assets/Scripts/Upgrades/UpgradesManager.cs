using UnityEngine;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{
    [SerializeField] UpgradeObject[] possibleUpgrades = new UpgradeObject[0];

    private UpgradeObject currentCard1;
    private UpgradeObject currentCard2;
    private UpgradeObject currentCard3;

    [SerializeField] UpgradeToChoose upgradeButton1;
    [SerializeField] UpgradeToChoose upgradeButton2;
    [SerializeField] UpgradeToChoose upgradeButton3;

    [SerializeField] GameObject[] thingsToTurnOnOffWithUpgradeScreen;

    [SerializeField] int maxMaskFragment = 3;
    [SerializeField] int curentMaskFragment = 0;

    [SerializeField] bool forceUpgradeScreenButton = false;

    [SerializeField] GameObject masksEarnedImage;

    #region SingletonPattern
    private static UpgradesManager _instance;

    public static UpgradesManager Instance { get { return _instance; } }


    private void Awake()
    {
        _instance = this;
    }
    #endregion

    private void Update()
    {
        if (forceUpgradeScreenButton) 
        {
            forceUpgradeScreenButton = false;
            ShowUpgradeScreen();
        } 
    }

    public void ShowUpgradeScreen()
    {
        ShowCursorCursor();
        Time.timeScale = 0f;

        foreach (GameObject go in thingsToTurnOnOffWithUpgradeScreen)
        {
            go.SetActive(true);
        }

        currentCard1 = possibleUpgrades[Random.Range(0, possibleUpgrades.Length)];
        upgradeButton1.thisUpgrade = currentCard1;
        upgradeButton1.upgradeDescription = currentCard1.upgradeDescription;
        upgradeButton1.upgradeSprite = currentCard1.upgradeImageFull;
        upgradeButton1.DrawUpgradeToChoose();

        currentCard2 = possibleUpgrades[Random.Range(0, possibleUpgrades.Length)];
        upgradeButton2.thisUpgrade = currentCard2;
        upgradeButton2.upgradeDescription = currentCard2.upgradeDescription;
        upgradeButton2.upgradeSprite = currentCard2.upgradeImageFull;
        upgradeButton2.DrawUpgradeToChoose();

        currentCard3 = possibleUpgrades[Random.Range(0, possibleUpgrades.Length)];
        upgradeButton3.thisUpgrade = currentCard3;
        upgradeButton3.upgradeDescription = currentCard3.upgradeDescription;
        upgradeButton3.upgradeSprite = currentCard3.upgradeImageFull;
        upgradeButton3.DrawUpgradeToChoose();
    }

    public void ChooseUpgrade(UpgradeObject chosenUpgrade)
    {
        //[Header("Mascara del arma")]
        UpgradeableStatsSingleton.Instance.damage += chosenUpgrade.damage;
        UpgradeableStatsSingleton.Instance.fireRate += chosenUpgrade.fireRate;
        UpgradeableStatsSingleton.Instance.knockback += chosenUpgrade.knockback;

        //[Header("Mascara del player")]
        UpgradeableStatsSingleton.Instance.speed += chosenUpgrade.speed;
        UpgradeableStatsSingleton.Instance.jumpForce += chosenUpgrade.jumpForce;
        UpgradeableStatsSingleton.Instance.damageResistance += chosenUpgrade.damageResistance;
        UpgradeableStatsSingleton.Instance.lifeRegeneration += chosenUpgrade.lifeRegeneration;

        //[Header("Mascara de debufos")]
        UpgradeableStatsSingleton.Instance.enemySlowness += chosenUpgrade.enemySlowness;
        UpgradeableStatsSingleton.Instance.enemyDamageReduction += chosenUpgrade.enemyDamageReduction;
        UpgradeableStatsSingleton.Instance.enemyHealthReduction += chosenUpgrade.enemyHealthReduction;

        HideUpgradeScreen();

        SpawnNewUiChosenMask(chosenUpgrade);

        curentMaskFragment++;

        if (curentMaskFragment >= maxMaskFragment) curentMaskFragment = 0;
    }

    private void SpawnNewUiChosenMask(UpgradeObject chosenUpgrade)
    {
        if (masksEarnedImage == null) return;

        GameObject newImage = Instantiate(masksEarnedImage);
        newImage.transform.SetParent(masksEarnedImage.transform.parent);

        newImage.GetComponent<Image>().sprite = chosenUpgrade.upgradeImageFull;

        newImage.SetActive(true);
    }

    private void HideUpgradeScreen()
    {
        LockAndHideCursor();

        Time.timeScale = 1f;

        foreach (GameObject go in thingsToTurnOnOffWithUpgradeScreen)
        {
            go.SetActive(false);
        }
    }

    private void ShowCursorCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    private void LockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
