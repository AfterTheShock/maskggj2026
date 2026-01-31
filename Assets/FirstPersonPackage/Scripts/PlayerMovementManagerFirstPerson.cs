using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementManagerFirstPerson : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] bool isSprintingEnabled = true;
    [SerializeField] float walkingSpeed = 5;
    [SerializeField] float runningSpeed = 9;
    [SerializeField] float crouchingSpeed = 3;
    [SerializeField] Transform orientationTransform;
    private float currentTargetMovementSpeed = 7;
    private float currentMovementSpeed = 0;
    private bool isPresingRunningButton;
    private bool isPresingCrouchingButton;

    [Header("GroundCheck")]
    [SerializeField] float groundDrag = 7f;
    [SerializeField] float airDrag = 0.4f;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float playerHeight = 0.25f;
    private bool grounded;

    [Header("Jumping")]
    [SerializeField] bool isJumpEnabled = true;
    [SerializeField] float jumpForce = 12;
    [SerializeField] float jumpCooldown = 0.45f;
    [SerializeField] float airSpeedMultiplier = 0.35f;
    private bool isPressingJumpButton = false;
    private bool readyToJump = true;

    [Header("Crouching")]
    [SerializeField] bool isCrouchingEnabled = true;
    [SerializeField] float playerHeadDistance = 2f;
    [SerializeField] float crouchYScaleLerpSpeed = 7f;
    public float crouchYScale = 0.5f;
    
    private float startYScale;
    private bool isHeadStuck = false;

    [Header("LimitVelocity")]
    [SerializeField] bool shoudLimitVelocityToCurrentVelocity = true;

    [Header("LerpMovementSpeed")]
    [SerializeField] bool shoudLerpCurrentMovementSpeed = true;
    [SerializeField] float lerpSpeed = 14;

    [Header("Slope Handling")]
    [SerializeField] bool doSlopeHandling = true;
    [SerializeField] float maxSlopeAngle = 40;
    [SerializeField] float downwardsForceOnSlopeUp = 4;
    private RaycastHit slopeHit;

    [Header("Random")]
    [SerializeField] Vector3 positionOffsetForRaycasts = new Vector3(0,0.15f,0);

    [Header("HeadBob")]
    [SerializeField] bool isHeadBobEnabled = true;
    [SerializeField] GameObject standingStillCinemachineCamera;
    [SerializeField] GameObject walkingCinemachineCamera;
    [SerializeField] GameObject runningCinemachineCamera;
    [SerializeField] GameObject crouchingCinemachineCamera;

    private float horizontalInput;
    private float verticalInput;
    public bool isPressingMoveButton;

    private Vector3 moveDirection;

    private Rigidbody rb;

    private ConstantForce playerConstantForce;

    public MovementState currentMovementState = MovementState.walking;

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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerConstantForce = GetComponent<ConstantForce>();

        ChangeTargetMovementSpeed(walkingSpeed);

        startYScale = this.transform.localScale.y;
    }

    void Update()
    {
        //Core functions
        GetPlayerInput();

        ManageCurrentSpeedState();
        ManageLerpingCurrentSpeed();

        CheckIfPlayerIsGrounded();
        MovePlayer();
        if(isCrouchingEnabled) ManageYScaleBasedOnCurrentPlayerState();

        //Conditionals
        if (isJumpEnabled && !isHeadStuck) ManageJumping();

        //Extras
        if (shoudLimitVelocityToCurrentVelocity) LimitPlayerVelocity();

        if (isHeadBobEnabled) ManagePlayerHeadBob();
    }

    private void GetPlayerInput()
    {
        if (currentMovementState != MovementState.sliding)
        {
            horizontalInput = inputSystemActions.Player.Move.ReadValue<Vector2>().x;
            verticalInput = inputSystemActions.Player.Move.ReadValue<Vector2>().y;

            if (horizontalInput != 0 || verticalInput != 0) isPressingMoveButton = true;
            else isPressingMoveButton = false;

            isPressingJumpButton = inputSystemActions.Player.Jump.ReadValue<float>() != 0;

            isPresingRunningButton = inputSystemActions.Player.Sprint.ReadValue<float>() != 0;
        }
        
        isPresingCrouchingButton = inputSystemActions.Player.Crouch.ReadValue<float>() != 0;
    }

    private void ManageCurrentSpeedState()
    {
        if (currentMovementState == MovementState.crouching || currentMovementState == MovementState.sliding)
        {
            isHeadStuck = Physics.Raycast(transform.position + positionOffsetForRaycasts, transform.up, playerHeadDistance, groundLayerMask);
            if (isHeadStuck) return;
        }
        isHeadStuck = false;

        if (isPresingCrouchingButton && isCrouchingEnabled)
        {
            ChangeTargetMovementSpeed(crouchingSpeed);
            currentMovementState = MovementState.crouching;

        }
        else if(isPresingRunningButton && isSprintingEnabled)
        {
            ChangeTargetMovementSpeed(runningSpeed);
            currentMovementState = MovementState.running;
        }
        else if(isPressingMoveButton)
        {
            ChangeTargetMovementSpeed(walkingSpeed);
            currentMovementState = MovementState.walking;
        }
        else
        {
            ChangeTargetMovementSpeed(walkingSpeed);
            currentMovementState = MovementState.idle;
        }
    }

    private void ManageYScaleBasedOnCurrentPlayerState()
    {
        if(currentMovementState == MovementState.crouching)
        {
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z), crouchYScaleLerpSpeed * Time.deltaTime);
        }
        else
        {
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(transform.localScale.x, startYScale, transform.localScale.z), crouchYScaleLerpSpeed * Time.deltaTime);
        }
    }

    private void ManageLerpingCurrentSpeed()
    {
        if (shoudLerpCurrentMovementSpeed)
        {
            if (horizontalInput != 0 || verticalInput != 0)
            {
                currentMovementSpeed = Mathf.Lerp(currentMovementSpeed, currentTargetMovementSpeed, lerpSpeed * Time.deltaTime);
            }
            else
            {
                currentMovementSpeed = Mathf.Lerp(currentMovementSpeed, 0, lerpSpeed * Time.deltaTime);
            }
        }
        else
        {
            currentMovementSpeed = currentTargetMovementSpeed;
        }
    }

    private void CheckIfPlayerIsGrounded()
    {
        grounded = Physics.Raycast(transform.position + positionOffsetForRaycasts, -transform.up, playerHeight, groundLayerMask);
    }

    private void MovePlayer()
    {
        if (grounded) rb.linearDamping = groundDrag;
        else rb.linearDamping = airDrag;

        moveDirection = orientationTransform.forward * verticalInput + orientationTransform.right * horizontalInput;

        if (OnSlope() && doSlopeHandling)
        {
            rb.useGravity = false;
            playerConstantForce.enabled = false;

            rb.AddForce(GetSlopeMovementDirection() * currentMovementSpeed * 500 * Time.deltaTime, ForceMode.Force);

            if (rb.linearVelocity.y > 0) rb.AddForce(-transform.up * downwardsForceOnSlopeUp * 500 * Time.deltaTime, ForceMode.Force);
        }
        else if(doSlopeHandling)
        {
            rb.useGravity = true;
            playerConstantForce.enabled = true;
        }

        if(grounded) rb.AddForce(moveDirection.normalized * currentMovementSpeed * 500 * Time.deltaTime, ForceMode.Force);

        else rb.AddForce(moveDirection.normalized * currentMovementSpeed * 500 * airSpeedMultiplier * Time.deltaTime, ForceMode.Force);
    }

    private void LimitPlayerVelocity()
    {
        if (!shoudLimitVelocityToCurrentVelocity) return;

        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if(flatVelocity.magnitude > currentTargetMovementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * currentTargetMovementSpeed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }
    }

    private void ManageJumping()
    {
        if(isPressingJumpButton && readyToJump && grounded)
        {
            readyToJump = false;

            Invoke(nameof(ResetReadyToJump), jumpCooldown);

            //Zero y velocity before jumping for consistent jumping height
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

            //Jump
            rb.AddForce(transform.up * (jumpForce * UpgradeableStatsSingleton.Instance.jumpForce), ForceMode.Impulse);
        }
    }

    private void ResetReadyToJump()
    {
        readyToJump = true;
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position + positionOffsetForRaycasts, -transform.up, out slopeHit, playerHeight))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }


        return false;
    }

    private Vector3 GetSlopeMovementDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void ManagePlayerHeadBob()
    {
        if (!isHeadBobEnabled) return;

        //StandingStill
        if(horizontalInput == 0 && verticalInput == 0 && standingStillCinemachineCamera != null)
        {
            TurnOffAllCinemachineCameras(standingStillCinemachineCamera);
            standingStillCinemachineCamera.SetActive(true);
        }
        else
        //Other states
        if(currentMovementState == MovementState.crouching && crouchingCinemachineCamera != null)
        {
            if(crouchingCinemachineCamera == null && walkingCinemachineCamera != null)
            {
                TurnOffAllCinemachineCameras(walkingCinemachineCamera);
                walkingCinemachineCamera.SetActive(true);
            }
            else if(crouchingCinemachineCamera != null)
            {
                TurnOffAllCinemachineCameras(crouchingCinemachineCamera);
                crouchingCinemachineCamera.SetActive(true);
            }
        }
        else if(currentMovementState == MovementState.walking && walkingCinemachineCamera != null)
        {
            TurnOffAllCinemachineCameras(walkingCinemachineCamera);
            walkingCinemachineCamera.SetActive(true);
        }
        else if(currentMovementState == MovementState.running && runningCinemachineCamera != null)
        {
            TurnOffAllCinemachineCameras(runningCinemachineCamera);
            runningCinemachineCamera.SetActive(true);

        }
    }

    private void TurnOffAllCinemachineCameras(GameObject cameraToIgnore = null)
    {
        if(cameraToIgnore != standingStillCinemachineCamera) standingStillCinemachineCamera.SetActive(false);
        if (cameraToIgnore != walkingCinemachineCamera) walkingCinemachineCamera.SetActive(false);
        if (cameraToIgnore != runningCinemachineCamera) runningCinemachineCamera.SetActive(false);
        if(crouchingCinemachineCamera != null && cameraToIgnore != crouchingCinemachineCamera) crouchingCinemachineCamera.SetActive(false);
    }

    private void ChangeTargetMovementSpeed(float newSpeed)
    {
        currentTargetMovementSpeed = newSpeed * UpgradeableStatsSingleton.Instance.speed;
    }
    
    
}

public enum MovementState
{
    idle,
    walking,
    crouching,
    running,
    sliding
}