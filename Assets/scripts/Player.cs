using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private enum State
	{
		Free,
		Interaction
	}

	[SerializeField]
	private float walk_speed = 1f;
	[SerializeField]
	private float run_speed = 4f;
	//TODO: Create sprint button

	private CharacterController controller;
	private CameraManager camera;

	private State current_state = State.Free;
	private bool is_running = false;

	private Vector3 direction;

	private Interactive target_interactive;

	protected void Awake()
	{
		controller = GetComponent<CharacterController> ();
		camera = GetComponent<CameraManager> ();

		direction = new Vector3 ();
	}

	protected void Update()
	{
		if (current_state == State.Free)
		{
			direction = Vector3.zero;

			if (Input.GetKey (KeyCode.W))
			{
				direction += camera.Forward;
			}
			else if (Input.GetKey (KeyCode.S))
			{
				direction -= camera.Forward;
			}

			if (Input.GetKey (KeyCode.D))
			{
				direction += camera.Right;
			}
			else if (Input.GetKey (KeyCode.A))
			{
				direction -= camera.Right;
			}

			if (!is_running)
			{
				direction = direction.normalized * walk_speed;
			}
			else
			{
				direction = direction.normalized * run_speed;
			}

			controller.SimpleMove (direction);

			if (target_interactive != null
			    && Input.GetKey (KeyCode.E))
			{
				bool has_started = target_interactive.StartInteraction();

				if (has_started)
				{
					Debug.Log("Start interaction");
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
				target_interactive.StopInteraction();
				StopInteraction();
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
			UIManager.ShowPrompt("Press <key> to interact", target_interactive.transform.position);
		}
	}

	/// <summary>
	/// Called when target_interactive is done. Stops interaction state.
	/// </summary>
	public void StopInteraction()
	{
		current_state = State.Free;
		camera.StopInteraction();
	}
}
