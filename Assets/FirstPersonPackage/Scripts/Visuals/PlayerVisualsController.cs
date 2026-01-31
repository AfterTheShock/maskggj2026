
using UnityEngine;

public class PlayerVisualsController : MonoBehaviour
{
    [SerializeField] float animationTransitionTime = 0.25f;
    [SerializeField] float playerYoffsetWhenCrouching = 0.5f;
    [SerializeField] string idleAnimationName = "PlayerIdle";
    [SerializeField] string walkingAnimationName = "PlayerWalking";
    [SerializeField] string runningAnimationName = "PlayerRunning";
    [SerializeField] string crouchingAnimationName = "PlayerCrouching";
    [SerializeField] string crouchingIdleAnimationName = "PlayerCrouchingIdle";
    [SerializeField] string interactAnimationName = "PlayerInteract";
    [SerializeField] string grabAnimationName = "PlayerGrab";

    [SerializeField] Animator animator;
    [SerializeField] Animator shadowsAnimator;
    [SerializeField] PlayerMovementManagerFirstPerson playerMovement;

    private GameObject oldParentPlayer;

    #region singletonPatern
    private static PlayerVisualsController _instance;
    public static PlayerVisualsController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindFirstObjectByType<PlayerVisualsController>();
            }
            return _instance;
        }
    }
    #endregion

    private void Start()
    {
        oldParentPlayer = this.transform.parent.gameObject;
        this.transform.SetParent(Camera.main.transform);
    }

    private void Update()
    {
        this.transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y,0);

        ManagePlayerAnimations();

        if(oldParentPlayer == null) Destroy(this.gameObject);

    }

    private void ManagePlayerAnimations()
    {
        //Crouching & crouching idle
        if(playerMovement.currentMovementState == MovementState.crouching)
        {
            this.transform.GetChild(0).localPosition = new Vector3(0, playerYoffsetWhenCrouching, 0); 

            if (playerMovement.isPressingMoveButton)
            {
                ChangeAnimationTo(crouchingAnimationName);
            }
            else
            {
                //Is crouching but not moving
                ChangeAnimationTo(crouchingIdleAnimationName);
            }
            return;
        }
        this.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);

        if (!playerMovement.isPressingMoveButton)
        {
            //Idle
            ChangeAnimationTo(idleAnimationName);
        }
        else if (playerMovement.currentMovementState == MovementState.walking)
        {
            //walking
            ChangeAnimationTo(runningAnimationName);
        }
        else if (playerMovement.currentMovementState == MovementState.running)
        {
            //Running
            ChangeAnimationTo(runningAnimationName);
        }
        else if (playerMovement.currentMovementState == MovementState.sliding)
        {
            //Sliding
            ChangeAnimationTo(crouchingAnimationName);
        }
        else
        {
            Debug.Log("No animation state, THIS SHOULD NOT HAPPEND");
        }
    }

    public void PlayGrabAnimation()
    {
        ChangeAnimationTo(grabAnimationName, 1, true);
    }

    public void PlayInteractAnimation()
    {
        ChangeAnimationTo(interactAnimationName, 1, true);
    }

    private void ChangeAnimationTo(string animName, int animLayer = 0, bool playNoMaterAnimatorState = false)
    {
        if (animName == "") return;

        if (animator.speed == 0) animator.Play(animName, animLayer);

        if (animator.IsInTransition(animLayer) && !playNoMaterAnimatorState) return;

        if (!animator.GetCurrentAnimatorStateInfo(animLayer).IsName(animName) || playNoMaterAnimatorState)
        {
            animator.CrossFadeInFixedTime(animName, animationTransitionTime, animLayer);

            if (shadowsAnimator)
            {
                shadowsAnimator.CrossFadeInFixedTime(animName, animationTransitionTime, animLayer);
            }
        }
    }

}
