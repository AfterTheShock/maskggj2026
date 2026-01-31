using UnityEngine;

public class FirstPersonCameraManager : MonoBehaviour
{
    [SerializeField] float sensitivity = 0.2f;

    [SerializeField] bool turnOffCameraSmoothness = false;

    private float maxCameraSmoothness = 25;
    [Range(5,25)]
    [SerializeField] float cameraSmoothness = 18f;

    [SerializeField] Transform cameraTransfrom;
    public Transform orientationTransfrom;
    public bool invertMouseXAxis = false;
    public bool invertMouseYAxis = false;

    private float xRotation;
    private float yRotation;

    //private InputSystem_Actions inputSystemActions;
    private FirstPersonInputSystem_Actions inputSystemActions;

    private void OnEnable()
    {
        if (inputSystemActions == null) inputSystemActions = new FirstPersonInputSystem_Actions();
        inputSystemActions.Player.Enable();

        GeInitialRotation();
    }

    private void GeInitialRotation()
    {
        yRotation = cameraTransfrom.eulerAngles.y;
        xRotation = cameraTransfrom.eulerAngles.x;

        orientationTransfrom.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void OnDisable()
    {
        if (inputSystemActions != null) inputSystemActions.Player.Disable();
    }

    void Start()
    {
        LockAndHideCursor();
        DeParentCamerHolder();
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;

        RotateCamera();
    }

    private void LateUpdate()
    {
        MoveCamera();
    }

    private void RotateCamera()
    {
        float mouseX = inputSystemActions.Player.Look.ReadValue<Vector2>().x * sensitivity;
        float mouseY = inputSystemActions.Player.Look.ReadValue<Vector2>().y * sensitivity;

        if (invertMouseXAxis) mouseX *= -1;
        if (invertMouseYAxis) mouseY *= -1;


        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85, 85);

        //With no smoothness
        if (turnOffCameraSmoothness && cameraSmoothness >= maxCameraSmoothness -0.15f)
        {
            cameraTransfrom.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientationTransfrom.rotation = Quaternion.Euler(0, yRotation, 0);
        }
        else //With smoothness
        {
            Quaternion targetRotation = Quaternion.Euler(xRotation, yRotation, 0);

            //Duplicate lerp if angle is too big to prevent 180+ rotation exess
            if(Quaternion.Angle(targetRotation, cameraTransfrom.rotation) >= 100)
            {
                cameraTransfrom.rotation = Quaternion.Lerp(cameraTransfrom.rotation, targetRotation, cameraSmoothness * Time.deltaTime);
            }

            cameraTransfrom.rotation = Quaternion.Lerp(cameraTransfrom.rotation, targetRotation, cameraSmoothness * Time.deltaTime);

            orientationTransfrom.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    public void SetRotationOffPlayerCamera(Transform rotToSet)
    {
        yRotation = rotToSet.eulerAngles.y;
        xRotation = rotToSet.eulerAngles.x;
        cameraTransfrom.rotation = rotToSet.rotation;
    }

    private void MoveCamera()
    {
        cameraTransfrom.position = this.transform.position;
    }

    private void DeParentCamerHolder()
    {
        cameraTransfrom.parent = null;
    }

    private void LockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDestroy()
    {
        if(cameraTransfrom != null) Destroy(cameraTransfrom.gameObject);
    }
}
