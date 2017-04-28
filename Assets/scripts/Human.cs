using UnityEngine;
using UnityEngine.AI;

public enum HumanVoiceType
{
    Masculine,
    Feminine
}

public class Human : Interactive
{
    [SerializeField]
    private float _playerDistance = 10f;
    public HumanVoiceType VoiceType;

    private NavMeshAgent _agent;
    private Transform _avatar;
    private Animator _animationController;

    private bool _lookingAround = true;

    [SerializeField, FMODUnity.EventRef]
    private string _masReactionEventRef, _femReactionEventRef, _walkEventRef;
    private FMOD.Studio.EventInstance _reactionEventInstance;
    private FMOD.Studio.EventInstance _walkEventInstance;

    protected void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _avatar = transform.GetChild(0);
        _animationController = _avatar.GetComponentInChildren<Animator>();

        SetupFmod();
    }

    private void SetupFmod()
    {
        var attributes = FMODUnity.RuntimeUtils.To3DAttributes(transform);

        _reactionEventInstance = FMODUnity.RuntimeManager.CreateInstance(VoiceType == HumanVoiceType.Masculine ? _masReactionEventRef : _femReactionEventRef);
        _walkEventInstance = FMODUnity.RuntimeManager.CreateInstance(_walkEventRef);

        _reactionEventInstance.set3DAttributes(attributes);
        _walkEventInstance.set3DAttributes(attributes);
    }

    protected override void Update()
    {
        base.Update();

        if (CurrentState == State.Idle)
        {
            if (_lookingAround)
            {
                if (Vector3.Distance(GameManager.Player.transform.position, transform.position) >= _playerDistance)
                {
                    _agent.SetDestination(GameManager.HumanManager.GetPointOfInterest());
                    _lookingAround = false;
                }
                else
                {
                    Vector3 target = GameManager.Player.transform.position;
                    target.y = transform.position.y;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target - transform.position), 5f * Time.deltaTime);
                }
            }
            else if (Vector3.Distance(transform.position, _agent.destination) < _agent.stoppingDistance
                || (type == Score.CatPerson && Vector3.Distance(GameManager.Player.transform.position, transform.position) < _playerDistance))
            {
                _agent.SetDestination(transform.position);
                _lookingAround = true;
            }

            _animationController.SetFloat("speed", _agent.velocity.magnitude);
        }
        else if (CurrentState == State.Finished)
        {
            _agent.enabled = false;
            transform.position += T * Vector3.up;
            transform.eulerAngles += T * 50f * Vector3.up;

            if (T >= 3f)
            {
                GameManager.HumanManager.RemoveHuman(this);
                Destroy(gameObject);
            }
        }
    }

    protected override void Interaction(float time)
    {
        //avatar.transform.localPosition = (Mathf.Abs(0.5f * Mathf.Sin(2f * Mathf.PI * time)) + 0.3f) * Vector3.up;

        Vector3 target = GameManager.Player.transform.position;
        target.y = transform.position.y;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target - transform.position), 5f * Time.deltaTime);

        if (time > 2f)
        {
            _avatar.transform.localPosition = 0.8f * Vector3.up;
            _animationController.SetBool("interacting", false);
            StopInteraction();
            Interactor.StopInteraction();
        }
    }

    protected override void SetState(State state)
    {
        base.SetState(state);

        if (state == State.Interact)
        {
            _animationController.SetBool("interacting", true);
            if (Application.platform != RuntimePlatform.LinuxEditor)
                _reactionEventInstance.start();
            _agent.SetDestination(transform.position);
        }
    }

    public void PlayFootstep()
    {
        if (Application.platform != RuntimePlatform.LinuxEditor)
            _walkEventInstance.start();
    }
}
