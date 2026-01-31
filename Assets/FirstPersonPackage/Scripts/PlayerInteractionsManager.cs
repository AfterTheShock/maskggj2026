using UnityEngine;


public class PlayerInteractionsManager : MonoBehaviour
{
    [SerializeField] float interactionDistance = 4f;
    [SerializeField] LayerMask raycastLayer;
    [SerializeField] LayerMask interactableLayer;

    [SerializeField] bool shoudTriggerGrabAnimation = true;

    public InteractableObject interactableBeingLokedAt;

    private bool isHoldingInteraction = false;

    private FirstPersonInputSystem_Actions inputSystemActions;

    private void OnEnable()
    {
        if (inputSystemActions == null) inputSystemActions = new FirstPersonInputSystem_Actions();
        inputSystemActions.Player.Enable();
    }

    private void OnDisable()
    {
        if (inputSystemActions != null) inputSystemActions.Player.Disable();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        CheckIfLookingAtObject();

        InteractWithLookedAtObject();

    }

    private void CheckIfLookingAtObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, interactionDistance, raycastLayer))
        {
            hit.transform.TryGetComponent<InteractableObject>(out InteractableObject newObjectToLook);

            if (interactableBeingLokedAt != null && newObjectToLook == interactableBeingLokedAt) return;

            if((interactableLayer & (1 << hit.transform.gameObject.layer)) != 0)
            {//Object is in mask
                if (newObjectToLook != null)
                {
                    StartLookingAtObject(newObjectToLook);
                }
                else Debug.Log("Object doest not have InteractableObject component");
            }
            else
            {//Is not interactable
                StopLookingAtObject();
            }
        }
        else
        {
            StopLookingAtObject();
        }
    }

    private void StartLookingAtObject(InteractableObject objectToStartLooking)
    {
        if (interactableBeingLokedAt == objectToStartLooking) return;

        objectToStartLooking.StartLookingAtObject();
        interactableBeingLokedAt = objectToStartLooking;
    }

    private void StopLookingAtObject()
    {
        if (interactableBeingLokedAt != null) 
        { 
            interactableBeingLokedAt.StoptLookingAtObject();

            if (isHoldingInteraction) StopIteractingWithObject();
        }
        interactableBeingLokedAt = null;
    }

    private void InteractWithLookedAtObject()
    {
        if (interactableBeingLokedAt == null) return;
        
        if (!inputSystemActions.Player.Interact.WasPressedThisFrame()) 
        {
            if (isHoldingInteraction && inputSystemActions.Player.Interact.ReadValue<float>() == 0) StopIteractingWithObject();

            return;
        }

        //If here shoud interact with object
        interactableBeingLokedAt.InteractWithObject();
        isHoldingInteraction = true;

        //Play grab or interact animations
        if(shoudTriggerGrabAnimation && PlayerVisualsController.Instance != null)
        {
            if (interactableBeingLokedAt.playinteractAnimationWhenInteracted) PlayerVisualsController.Instance.PlayInteractAnimation();
            else if (interactableBeingLokedAt.playGrabAnimationWhenInteracted) PlayerVisualsController.Instance.PlayGrabAnimation();

        }
    }

    private void StopIteractingWithObject()
    {
        isHoldingInteraction = false;

        if (interactableBeingLokedAt == null) return;

        interactableBeingLokedAt.StopInteractingWithObject();
    }
}
