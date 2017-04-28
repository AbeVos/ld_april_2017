using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactive : MonoBehaviour
{
	protected enum State
	{
		/// <summary>
		/// Interactive has not been activated and awaits activation.
		/// </summary>
		Idle,
		/// <summary>
		/// Interactive has been activated and is currently in interaction.
		/// </summary>
		Interact,
		/// <summary>
		/// Interactive has been activated and cannot be activated again.
		/// </summary>
		Finished
	};

	[SerializeField]
	protected Score type = Score.CatPerson;
	[SerializeField]
	protected bool skippable = false;
	[SerializeField]
	protected bool Repeatable = false;

	protected State CurrentState = State.Idle;
	protected float T = 0f;

	protected Player Interactor;

	public Score Type
	{ 
		get { return type; }
		set { type = value; }
	}
	public bool Skippable { get { return skippable; } }

	protected virtual void Update()
	{
		if (CurrentState == State.Interact)
		{
			Interaction(T);
		}

		T += Time.deltaTime;
	}

	protected void OnTriggerEnter(Collider col)
	{
		if (CurrentState == State.Finished) return;

		Player player = col.GetComponent<Player> ();

		if (player != null)
		{
			Interactor = player;
			player.SetTargetInteractive (this);
		}
	}

	protected void OnTriggerExit(Collider col)
	{
		Player player = col.GetComponent<Player> ();

		if (player != null)
		{
			//interactor = null;
			player.SetTargetInteractive (null);
		}
	}

	public bool StartInteraction()
	{
		if (CurrentState == State.Finished)
		{
			return false;
		}
		else
		{
			SetState(State.Interact);
			return true;
		}
	}

	public virtual void StopInteraction()
	{
		Game.AddScore(type);

		if (Repeatable)
		{
			SetState(State.Idle);
		}
		else
		{
			SetState(State.Finished);
		}
	}

	protected abstract void Interaction(float time);

	protected virtual void SetState(State state)
	{
		CurrentState = state;
		T = 0f;
	}
}
