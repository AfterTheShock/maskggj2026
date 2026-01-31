using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonVisuals : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float extraSizeOnHover = 0.15f;

    [SerializeField] float sizeIncreaseSpeed = 3f;

    bool isHoveringButton = false;

    float startButtonSize = 1f;

    private void Start()
    {
        startButtonSize = this.transform.localScale.x;
    }

    void Update()
    {
        if (isHoveringButton)
        {
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.one * (startButtonSize + extraSizeOnHover), sizeIncreaseSpeed * 1.5f * Time.unscaledDeltaTime); 
        }
        else
        {
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.one * (startButtonSize), sizeIncreaseSpeed * Time.unscaledDeltaTime);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHoveringButton = true;
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isHoveringButton = false;
    }
}
