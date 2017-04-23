using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum HumanVoiceType
{
    masculine,
    feminine
}

public class Human : Interactive
{
	[SerializeField]
	private float player_distance = 10f;
    public HumanVoiceType voiceType;

	private NavMeshAgent agent;
	private Transform avatar;
    private Animator animationController;

	private bool looking_around = true;

    [SerializeField, FMODUnity.EventRef]
    private string masReactionEventRef, femReactionEventRef, walkEventRef;
    private FMOD.Studio.EventInstance reactionEventInstance;
    private FMOD.Studio.EventInstance walkEventInstance;

    protected void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		avatar = transform.GetChild(0);
	    animationController = avatar.GetComponentInChildren<Animator>();

	    if (Application.platform != RuntimePlatform.LinuxEditor)
	    {
	        if (voiceType == HumanVoiceType.masculine)
	        {
	            reactionEventInstance = FMODUnity.RuntimeManager.CreateInstance(masReactionEventRef);
	        }
	        else
	        {
	            reactionEventInstance = FMODUnity.RuntimeManager.CreateInstance(femReactionEventRef);
	        }

	        walkEventInstance = FMODUnity.RuntimeManager.CreateInstance(walkEventRef);
        }
        
    }

    protected override void Update()
	{
		base.Update();

		if (current_state == State.Idle)
		{
			if (looking_around)
			{
				if (Vector3.Distance(GameManager.Player.transform.position, transform.position) >= player_distance)
				{
					agent.SetDestination(GameManager.HumanManager.GetPointOfInterest());
					looking_around = false;
				}
				else
				{
					Vector3 target = GameManager.Player.transform.position;
					target.y = transform.position.y;
					transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target - transform.position), 5f * Time.deltaTime);
				}	
			}
			else if (Vector3.Distance(transform.position, agent.destination) < agent.stoppingDistance
				|| (type == Score.CatPerson && Vector3.Distance(GameManager.Player.transform.position, transform.position) < player_distance))
			{
				agent.SetDestination(transform.position);
				looking_around = true;
			}

            animationController.SetFloat("speed",agent.velocity.magnitude);
		}
		else if (current_state == State.Finished)
		{
			agent.enabled = false;
			transform.position += t * Vector3.up;
			transform.eulerAngles += t * 50f * Vector3.up;

			if (t >= 3f)
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
			avatar.transform.localPosition = 0.8f * Vector3.up;
            animationController.SetBool("interacting", false);
			StopInteraction();
			interactor.StopInteraction();
		}
	}

	protected override void SetState(State state)
	{
		base.SetState(state);

		if (state == State.Interact)
		{
		    animationController.SetBool("interacting", true);
			if (Application.platform != RuntimePlatform.LinuxEditor)
            	reactionEventInstance.start();
			agent.SetDestination(transform.position);
		}
	}

    public void PlayFootstep()
    {
        if (Application.platform != RuntimePlatform.LinuxEditor)
            walkEventInstance.start();
    }
}
