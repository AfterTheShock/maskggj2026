using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeToChoose : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI descriptionText;

    public UpgradeObject thisUpgrade;

    [SerializeField] Image maskImage;

    public string upgradeDescription;
    public Sprite upgradeSprite;

    [Header("General Button Visuals Config")]
    [SerializeField] float targetScaleMultiplierBig = 1.15f;
    [SerializeField] float targetScaleMultiplierEnd = 1.1f;
    [SerializeField] float targetScaleMultiplierSmallOnBuy = 1.45f;
    [SerializeField] float scaleLerpSpeed = 18f;
    [SerializeField] Vector3 rotationOnHover = new Vector3(0, 0, 1);
    [SerializeField] float rotationLerpSpeed = 10;
    private Vector3 startScale = Vector3.one;
    private Vector3 startRotation = Vector3.zero;
    private bool isHoveringButton = false;
    private bool isGettingBig = false;

    public void DrawUpgradeToChoose()
    {
        descriptionText.text = upgradeDescription;

        maskImage.sprite = upgradeSprite;
    }

    public void OnClickUpgrade()
    {
        UpgradesManager.Instance.ChooseUpgrade(thisUpgrade);
    }

    private void Start()
    {
        startScale = this.transform.localScale;
        startRotation = this.transform.localEulerAngles;

        this.transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        ManageButtonSizeAndRot();
    }

    private void ManageButtonSizeAndRot()
    {
        Vector3 currentTargetScale = startScale;
        Vector3 currentTargetRotation = startRotation;
        if (isHoveringButton) currentTargetScale = startScale * targetScaleMultiplierEnd;
        if (isGettingBig)
        {
            currentTargetScale = startScale * targetScaleMultiplierBig;
            currentTargetRotation = startRotation + rotationOnHover;
        }

        this.transform.localScale = Vector3.Lerp(this.transform.localScale, currentTargetScale, scaleLerpSpeed * Time.unscaledDeltaTime);

        this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, Quaternion.Euler(currentTargetRotation), rotationLerpSpeed * Time.unscaledDeltaTime);

        if (isGettingBig && Vector3.Distance(this.transform.localScale, startScale * targetScaleMultiplierBig) <= startScale.x * 0.03) isGettingBig = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverOverButton();
    }

    private void OnHoverOverButton()
    {
        isHoveringButton = true;
        isGettingBig = true;
        if (Random.Range(0, 3) == 0) rotationOnHover *= -1;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHoveringButton = false;
    }
}
