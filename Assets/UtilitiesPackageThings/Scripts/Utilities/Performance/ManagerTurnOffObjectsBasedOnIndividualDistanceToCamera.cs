using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

[BurstCompile]
public class ManagerTurnOffObjectsBasedOnIndividualDistanceToCamera : MonoBehaviour
{
    [Header("Turns on off cheking X obj per frame based on each obj distance to de cam")]
    [SerializeField] List<GameObject> objectsToTurnOnOff;

    [SerializeField] List<Transform> listToConvertToArrayOnStart;

    [SerializeField] int numOfObjToChechPerFrame = 2;

    [SerializeField] float distanceToTurnOnOff = 10;

    [SerializeField] bool turnToThisWhenClose = true;

    private int currentLoopingIndex = 0;

    Vector3 camPos;

    private void OnEnable()
    {
        if(objectsToTurnOnOff.Count == 0 && listToConvertToArrayOnStart.Count != 0)
        {
            foreach (Transform go in listToConvertToArrayOnStart)
            {
                objectsToTurnOnOff.Add(go.gameObject);
            }
        }
    }

    private void Update()
    {
        camPos = Camera.main.transform.position;

        for (int i = 0; i < numOfObjToChechPerFrame; i++) ChechNextObject();
    }

    [BurstCompile]
    private void ChechNextObject()
    {
        if (objectsToTurnOnOff.Count == 0) return;

        if(objectsToTurnOnOff[currentLoopingIndex] == null)
        {
            objectsToTurnOnOff.RemoveAt(currentLoopingIndex);
            IncrementLoopingIndexByOne();
        }
        
        if(Vector3.Distance(camPos, objectsToTurnOnOff[currentLoopingIndex].transform.position) <= distanceToTurnOnOff)
        {
            objectsToTurnOnOff[currentLoopingIndex].SetActive(turnToThisWhenClose);
        }
        else
        {
            objectsToTurnOnOff[currentLoopingIndex].SetActive(!turnToThisWhenClose);
        }


        IncrementLoopingIndexByOne();
    }

    private void IncrementLoopingIndexByOne()
    {
        currentLoopingIndex++;
        if (currentLoopingIndex >= objectsToTurnOnOff.Count) currentLoopingIndex = 0;
    }
}
