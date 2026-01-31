using UnityEngine;

public class DestroyAfterXSeconds : MonoBehaviour
{
    public float timeToDestroyObject = 3f;

    [SerializeField] GameObject thingToSpawnOnDestroy;
    [SerializeField] GameObject thingToTurnOnOnDestroy;


    private void Update()
    {
        timeToDestroyObject -= Time.deltaTime;

        if (timeToDestroyObject <= 0) DestroyObject();
    }

    private void DestroyObject()
    {
        if(thingToSpawnOnDestroy != null) Instantiate(thingToSpawnOnDestroy).transform.position = this.transform.position;

        if(thingToTurnOnOnDestroy != null) thingToTurnOnOnDestroy.SetActive(true);

        Destroy(this.gameObject);
    }
}
