using UnityEngine;
using UnityEngine.Events;

public class ExecuteEventAfterXSeconds : MonoBehaviour
{
    [SerializeField] float secondsToWaitBeforeTriggering = 1f;
    [SerializeField] UnityEvent eventToTrigger;

    [SerializeField] bool triggerOnStartFunction = true;
    [SerializeField] bool triggerOnEnableFunction = false;

    void Start()
    {
        if (triggerOnStartFunction) 
        {
            CancelInvoke();
            Invoke("EventInvoker", secondsToWaitBeforeTriggering);
        } 
    }

    private void OnEnable()
    {
        if (triggerOnEnableFunction)
        {
            CancelInvoke();
            Invoke("EventInvoker", secondsToWaitBeforeTriggering);
        }
    }

    public void CallEventToTriggerAfterXSeconds()
    {
        Invoke("EventInvoker", secondsToWaitBeforeTriggering);
    }

    private void EventInvoker()
    {
        eventToTrigger.Invoke();
    }
}
