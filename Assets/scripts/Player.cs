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
    private float _walkSpeed = 1f;
    [SerializeField]
    private float _runSpeed = 4f;
    //TODO: Create sprint button

    private float _jumpForce = 20f;
    private float _gravity = 5.0f;

    private CharacterController _controller;
    private CameraManager _camera;
    private Transform _avatar;
    private Animator _animationController;

    private State _currentState = State.Free;

    private Vector3 _direction;
    private Vector3 _momentum;
    private Vector3 _upwardVel;

    private Interactive _targetInteractive;

    [SerializeField, FMODUnity.EventRef]
    private string _pawEventRef, _furEventRef, _voxEventRef, _purrEventRef;
    private EventInstance _pawEventInstance;
    private EventInstance _furEventInstance;
    private EventInstance _voxEventInstance;
    private EventInstance _purrEventInstance;
    private ParameterInstance _pawMovementParameterInstance;
    private ParameterInstance _furMovementParameterInstance = null;
    private ParameterInstance _purrParameterInstance = null;
    // private FMOD.Studio.ParameterInstance surfaceParameterInstance = null;

    protected void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camera = GetComponent<CameraManager>();
        _avatar = transform.GetChild(0);
        _animationController = _avatar.GetComponentInChildren<Animator>();

        _direction = new Vector3();
        if (Application.platform != RuntimePlatform.LinuxEditor)
            SetupFmodEvents();
    }

    private void SetupFmodEvents()
    {
        if (_pawEventInstance == null)
        {
            _pawEventInstance = FMODUnity.RuntimeManager.CreateInstance(_pawEventRef);
            _pawEventInstance.getParameter("cat_paws_movement", out _pawMovementParameterInstance);
        }

        if (_furEventInstance == null)
        {
            _furEventInstance = FMODUnity.RuntimeManager.CreateInstance(_furEventRef);
            _furEventInstance.getParameter("cat_fur_movement", out _furMovementParameterInstance);
        }

        if (_voxEventInstance == null)
        {
            _voxEventInstance = FMODUnity.RuntimeManager.CreateInstance(_voxEventRef);
        }

        if (_purrEventInstance == null)
        {
            _purrEventInstance = FMODUnity.RuntimeManager.CreateInstance(_purrEventRef);
        }

        // purr loops
        _purrEventInstance.start();
    }

    protected void Update()
    {
        _direction = Vector3.zero;

        if (_currentState == State.Free)
        {
            if (Input.GetKey(KeyCode.W))
            {
                _direction += _camera.Forward;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _direction -= _camera.Forward;
            }

            if (Input.GetKey(KeyCode.D))
            {
                _direction += _camera.Right;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                _direction -= _camera.Right;
            }

            _direction = _direction.normalized;

            float speed;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                speed = _runSpeed;
            }
            else
            {
                speed = _walkSpeed;
            }

            TurnAvatar(_runSpeed);
            _controller.Move(Time.deltaTime * (_direction.magnitude * _avatar.forward * speed + _gravity * Vector3.down));
            _animationController.SetFloat("speed", _controller.velocity.magnitude);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _momentum = speed * _direction;
                _upwardVel = _jumpForce * Vector3.up;
                _direction += _upwardVel;
                _currentState = State.Jump;
            }

            if (_targetInteractive != null
                && Input.GetKey(KeyCode.E))
            {
                bool hasStarted = _targetInteractive.StartInteraction();

                if (hasStarted)
                {
                    Debug.Log("Start interaction");
                    _animationController.SetBool("interacting", true);
                    PlayVox();
                    _camera.StartInteraction();
                    _currentState = State.Interaction;
                }
            }
        }
        else if (_currentState == State.Interaction)
        {

            // Stop interaction manually
            if (Input.GetKey(KeyCode.E)
                && _targetInteractive.Skippable)
            {
                StopPurr();
                _targetInteractive.StopInteraction();
                StopInteraction();
            }
        }
        else if (_currentState == State.Jump)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _upwardVel *= 0.8f;
            }
            else
            {
                _upwardVel *= 0.5f;
            }

            _direction = _upwardVel + 2f * _momentum;

            _animationController.SetBool("jump", true);
            _controller.Move(Time.deltaTime * (_direction + _gravity * Vector3.down));

            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 0.2f, 1 << LayerMask.NameToLayer("Floor")))
            {
                _animationController.SetBool("jump", false);
                _currentState = State.Free;
            }
        }
    }

    /// <summary>
    /// Set _interactive_ as target_interactive.
    /// </summary>
    public void SetTargetInteractive(Interactive interactive)
    {
        _targetInteractive = interactive;

        if (_targetInteractive == null)
        {
            UiManager.HidePrompt();
        }
        else
        {
            UiManager.ShowPrompt("Press E to interact", _targetInteractive.transform);
        }
    }

    /// <summary>
    /// Called when target_interactive is done. Stops interaction state.
    /// </summary>
    public void StopInteraction()
    {
        _currentState = State.Free;
        _camera.StopInteraction();
        _animationController.SetBool("interacting", false);
    }

    public void PlayFootstepSound()
    {
        if (Application.platform == RuntimePlatform.LinuxEditor) { return; }

        if (_currentState == State.Free)
        {
            _pawMovementParameterInstance.setValue(_momentum.magnitude / (_runSpeed * .6f));
            _furMovementParameterInstance.setValue(_momentum.magnitude / (_runSpeed * .6f));
        }

        _pawEventInstance.start();
        _furEventInstance.start();
    }

    public void StartPurr()
    {
        if (Application.platform != RuntimePlatform.LinuxEditor)
            _purrParameterInstance.setValue(1);
    }

    public void StopPurr()
    {
        if (Application.platform != RuntimePlatform.LinuxEditor)
            _purrParameterInstance.setValue(0);
    }

    public void PlayVox()
    {
        if (Application.platform != RuntimePlatform.LinuxEditor)
            _voxEventInstance.start();
    }

    private void TurnAvatar(float speed)
    {
        if (_direction.sqrMagnitude > 0)
        {
            _avatar.rotation = Quaternion.Lerp(
                _avatar.rotation,
                Quaternion.LookRotation(_direction),
                2f * speed * Time.deltaTime);
        }
    }
}
