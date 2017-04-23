using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class Player : MonoBehaviour
{
    private enum State
    {
        Free,
        Interaction,
        Jump
    }

    [SerializeField]
    private float walk_speed = 1f;
    [SerializeField]
    private float run_speed = 4f;
    //TODO: Create sprint button

    private float jump_force = 20f;
    private float gravity = 5.0f;

    private CharacterController controller;
    new private CameraManager camera;
    private Transform avatar;
    private Animator animationController;

    private State current_state = State.Free;

    private Vector3 direction;
    private Vector3 momentum;
    private Vector3 upward_vel;

    private Interactive target_interactive;

    [SerializeField, FMODUnity.EventRef]
    private string pawEventRef, furEventRef, voxEventRef, purrEventRef;
    private EventInstance pawEventInstance;
    private EventInstance furEventInstance;
    private EventInstance voxEventInstance;
    private EventInstance purrEventInstance;
    private ParameterInstance pawMovementParameterInstance;
    private ParameterInstance furMovementParameterInstance = null;
    private ParameterInstance purrParameterInstance = null;
    // private FMOD.Studio.ParameterInstance surfaceParameterInstance = null;

    protected void Awake()
    {
        controller = GetComponent<CharacterController>();
        camera = GetComponent<CameraManager>();
        avatar = transform.GetChild(0);
        animationController = avatar.GetComponentInChildren<Animator>();

        direction = new Vector3();
        if (Application.platform != RuntimePlatform.LinuxEditor)
            SetupFmodEvents();
    }

    private void SetupFmodEvents()
    {
        if (pawEventInstance == null)
        {
            pawEventInstance = FMODUnity.RuntimeManager.CreateInstance(pawEventRef);
            pawEventInstance.getParameter("cat_paws_movement", out pawMovementParameterInstance);
        }

        if (furEventInstance == null)
        {
            furEventInstance = FMODUnity.RuntimeManager.CreateInstance(furEventRef);
            furEventInstance.getParameter("cat_fur_movement", out furMovementParameterInstance);
        }

        if (voxEventInstance == null)
        {
            voxEventInstance = FMODUnity.RuntimeManager.CreateInstance(voxEventRef);
        }

        if (purrEventInstance == null)
        {
            purrEventInstance = FMODUnity.RuntimeManager.CreateInstance(purrEventRef);
        }

        // purr loops
        purrEventInstance.start();
    }

    protected void Update()
    {
        direction = Vector3.zero;

        if (current_state == State.Free)
        {
            if (Input.GetKey(KeyCode.W))
            {
                direction += camera.Forward;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                direction -= camera.Forward;
            }

            if (Input.GetKey(KeyCode.D))
            {
                direction += camera.Right;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                direction -= camera.Right;
            }

            direction = direction.normalized;

            float speed;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                speed = run_speed;
            }
            else
            {
                speed = walk_speed;
            }

            TurnAvatar(run_speed);
            controller.Move(Time.deltaTime * (direction.magnitude * avatar.forward * speed + gravity * Vector3.down));
            animationController.SetFloat("speed", controller.velocity.magnitude);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                momentum = speed * direction;
                upward_vel = jump_force * Vector3.up;
                direction += upward_vel;
                current_state = State.Jump;
            }

            if (target_interactive != null
                && Input.GetKey(KeyCode.E))
            {
                bool has_started = target_interactive.StartInteraction();

                if (has_started)
                {
                    Debug.Log("Start interaction");
                    animationController.SetBool("interacting", true);
                    PlayVox();
                    camera.StartInteraction();
                    current_state = State.Interaction;
                }
            }
        }
        else if (current_state == State.Interaction)
        {

            // Stop interaction manually
            if (Input.GetKey(KeyCode.E)
                && target_interactive.Skippable)
            {
                StopPurr();
                target_interactive.StopInteraction();
                StopInteraction();
            }
        }
        else if (current_state == State.Jump)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                upward_vel *= 0.8f;
            }
            else
            {
                upward_vel *= 0.5f;
            }

            direction = upward_vel + 2f * momentum;

            animationController.SetBool("jump", true);
            controller.Move(Time.deltaTime * (direction + gravity * Vector3.down));

            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 0.2f, 1 << LayerMask.NameToLayer("Floor")))
            {
                animationController.SetBool("jump", false);
                current_state = State.Free;
            }
        }
    }

    /// <summary>
    /// Set _interactive_ as target_interactive.
    /// </summary>
    public void SetTargetInteractive(Interactive interactive)
    {
        target_interactive = interactive;

        if (target_interactive == null)
        {
            UIManager.HidePrompt();
        }
        else
        {
            UIManager.ShowPrompt("Press E to interact", target_interactive.transform);
        }
    }

    /// <summary>
    /// Called when target_interactive is done. Stops interaction state.
    /// </summary>
    public void StopInteraction()
    {
        current_state = State.Free;
        camera.StopInteraction();
        animationController.SetBool("interacting", false);
    }

    public void PlayFootstepSound()
    {
        if (Application.platform == RuntimePlatform.LinuxEditor) { return;}

        if (current_state == State.Free)
        {
            pawMovementParameterInstance.setValue(momentum.magnitude / (run_speed * .6f));
            furMovementParameterInstance.setValue(momentum.magnitude / (run_speed * .6f));
        }

        pawEventInstance.start();
        furEventInstance.start();
    }

    public void StartPurr()
    {
        if (Application.platform != RuntimePlatform.LinuxEditor)
            purrParameterInstance.setValue(1);
    }

    public void StopPurr()
    {
        if (Application.platform != RuntimePlatform.LinuxEditor)
            purrParameterInstance.setValue(0);
    }

    public void PlayVox()
    {
        if (Application.platform != RuntimePlatform.LinuxEditor)
            voxEventInstance.start();
    }

    private void TurnAvatar(float speed)
    {
        if (direction.sqrMagnitude > 0)
        {
            avatar.rotation = Quaternion.Lerp(
                avatar.rotation,
                Quaternion.LookRotation(direction),
                2f * speed * Time.deltaTime);
        }
    }


}
