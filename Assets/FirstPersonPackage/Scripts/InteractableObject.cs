using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] UnityEvent onInteractEvent;
    [SerializeField] UnityEvent onStopInteractingEvent;

    [Header("Looking")]
    [SerializeField] UnityEvent onLookedAtEvent;
    [SerializeField] UnityEvent onStopLookingEvent;

    public bool isBeeingLookedAt;

    public bool playinteractAnimationWhenInteracted = true;
    public bool playGrabAnimationWhenInteracted = false;

    [SerializeField] bool destroyOnInteract = false;

    public void InteractWithObject()
    {
        onInteractEvent.Invoke();

        if(destroyOnInteract) Destroy(gameObject);
    }

    public void StopInteractingWithObject()
    {
        onStopInteractingEvent.Invoke();
    }

    public void StartLookingAtObject()
    {
        onLookedAtEvent.Invoke();
        isBeeingLookedAt = true;
    }

    public void StoptLookingAtObject()
    {
        onStopLookingEvent.Invoke();
        isBeeingLookedAt = false;
    }


}
