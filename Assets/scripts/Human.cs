using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Human : Interactive
{
	[SerializeField]
	private float player_distance = 10f;

	private NavMeshAgent agent;
	private Transform avatar;

	private bool looking_around = true;

	protected void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		avatar = transform.GetChild(0);
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
					transform.LookAt(target);
				}	
			}
			else if (Vector3.Distance(transform.position, agent.destination) < agent.stoppingDistance
				|| (type == Score.CatPerson && Vector3.Distance(GameManager.Player.transform.position, transform.position) < player_distance))
			{
				agent.SetDestination(transform.position);
				looking_around = true;
			}
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
		avatar.transform.localPosition = (Mathf.Abs(0.5f * Mathf.Sin(2f * Mathf.PI * time)) + 0.3f) * Vector3.up;

		if (time > 2f)
		{
			avatar.transform.localPosition = 0.8f * Vector3.up;
			StopInteraction();
			interactor.StopInteraction();
		}
	}

	protected override void SetState(State state)
	{
		base.SetState(state);

		if (state == State.Interact)
		{
			agent.SetDestination(transform.position);
		}
	}
}
