using UnityEngine;

public class TurnOffBasedOnDistanceToCamera : MonoBehaviour
{
    [Header("This turns off all objects in this list based on distance from THIS.position to the main cam")]

    [SerializeField] GameObject[] objectsToTurnOff;

    int areObjectsOnOrOff = 0; // 1 is on, -1 is off

    [SerializeField] float distanceToTurnOnOff = 25;

    [SerializeField] float timeBetweenChecks;
    private float currentTimeSinceLastCheck;

    private void Update()
    {
        currentTimeSinceLastCheck += Time.deltaTime;

        if(currentTimeSinceLastCheck >= timeBetweenChecks)
        {
            currentTimeSinceLastCheck = 0;

            float currentDistanceToCamera = Vector3.Distance(this.transform.position, Camera.main.transform.position);

            if (currentDistanceToCamera <= distanceToTurnOnOff && areObjectsOnOrOff != 1)
            {
                areObjectsOnOrOff = 1;

                foreach(GameObject obj in objectsToTurnOff)
                {
                    if (obj != null) { obj.SetActive(true); }
                }

            }
            else if (currentDistanceToCamera > distanceToTurnOnOff && areObjectsOnOrOff != -1)
            {
                areObjectsOnOrOff = -1;

                foreach (GameObject obj in objectsToTurnOff)
                {
                    if (obj != null) { obj.SetActive(false); }
                }
            }
        }
    }
}
