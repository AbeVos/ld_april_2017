using System.Collections;
using System.Collections.Generic;
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
	private CameraManager camera;
	private Transform avatar;

	private State current_state = State.Free;
	private bool is_running = false;

	private Vector3 direction;
	private Vector3 momentum;
	private Vector3 upward_vel;

	private Interactive target_interactive;

	protected void Awake()
	{
		controller = GetComponent<CharacterController> ();
		camera = GetComponent<CameraManager> ();
		avatar = transform.GetChild(0);

		direction = new Vector3 ();
	}

	protected void Update()
	{
		direction = Vector3.zero;

		if (current_state == State.Free)
		{
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

			direction = direction.normalized;

			float speed;
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				is_running = true;
				speed = run_speed;
			}
			else
			{
				is_running = false;
				speed = walk_speed;
			}

			TurnAvatar(run_speed);
			controller.Move (Time.deltaTime * (direction.magnitude * avatar.forward * speed + gravity * Vector3.down));

			if (Input.GetKeyDown(KeyCode.Space))
			{
				momentum = speed * direction;
				upward_vel = jump_force * Vector3.up;
				direction += upward_vel;
				current_state = State.Jump;
			}

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

			controller.Move(Time.deltaTime * (direction + gravity * Vector3.down));

			Ray ray = new Ray(transform.position, Vector3.down);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 0.2f, 1 << LayerMask.NameToLayer("Floor")))
			{
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
