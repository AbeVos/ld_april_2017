﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
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
	protected bool skippable = false;

	protected State current_state = State.Idle;
	protected float t = 0f;

	protected Player interactor;

	public bool Skippable { get { return skippable; } }

	protected void Update()
	{
		if (current_state == State.Interact)
		{
			transform.position -= Vector3.up * 0.01f;

			if (t >= 1f)
			{
				StopInteraction();
				interactor.StopInteraction();
			}
		}

		t += Time.deltaTime;
	}

	protected void OnTriggerEnter(Collider col)
	{
		if (current_state == State.Finished) return;

		Player player = col.GetComponent<Player> ();

		if (player != null)
		{
			interactor = player;
			player.SetTargetInteractive (this);
		}
	}

	protected void OnTriggerExit(Collider col)
	{
		Player player = col.GetComponent<Player> ();

		if (player != null)
		{
			interactor = null;
			player.SetTargetInteractive (null);
		}
	}

	public bool StartInteraction()
	{
		if (current_state == State.Finished)
		{
			return false;
		}
		else
		{
			SetState(State.Interact);
			return true;
		}
	}

	public void StopInteraction()
	{
		SetState(State.Finished);
	}

	private void SetState(State state)
	{
		current_state = state;
		t = 0f;
	}
}