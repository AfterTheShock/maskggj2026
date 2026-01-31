using Unity.Cinemachine;
using UnityEngine;

public class CameraZoomManager : MonoBehaviour
{
    [SerializeField] CinemachineCamera[] cinemachineCameras = null;

    [SerializeField] float fovWhileZooming = 45;
    [SerializeField] float zoomLerpSpeed = 10f;

    private float startFov;
    private float currentFov;

    private void Start()
    {
        cinemachineCameras = GameObject.FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None);

        startFov = Camera.main.fieldOfView;
        currentFov = startFov;
    }

    private void Update()
    {
        cinemachineCameras = GameObject.FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None);

        if (Input.GetMouseButton(1)) AplyZoomToAllCinemachines(fovWhileZooming);

        else AplyZoomToAllCinemachines(startFov);

    }

    private void AplyZoomToAllCinemachines(float zoomToApply)
    {
        currentFov = Mathf.Lerp(currentFov, zoomToApply, zoomLerpSpeed * Time.deltaTime);

        foreach (var cam in cinemachineCameras)
        {
            cam.Lens.FieldOfView = currentFov;
            
        }
    }
}
