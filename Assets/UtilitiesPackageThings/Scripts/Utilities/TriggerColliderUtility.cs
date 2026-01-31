using UnityEngine;
using UnityEngine.Events;


public class TriggerColliderUtility : MonoBehaviour
{
    [SerializeField] LayerMask otherColiderLayer;

    [SerializeField] bool destroyThisAfterEntering = true;

    [SerializeField] bool destroyThisAfterExiting = false;

    [SerializeField] UnityEvent onTrigerEnterEvent;
    [SerializeField] UnityEvent onTrigerExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        if ((otherColiderLayer & (1 << other.gameObject.layer)) == 0) return;

        onTrigerEnterEvent.Invoke();

        if(destroyThisAfterEntering) Destroy(this.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if ((otherColiderLayer & (1 << other.gameObject.layer)) == 0) return;

        onTrigerExitEvent.Invoke();

        if (destroyThisAfterExiting) Destroy(this.gameObject);
    }
}
