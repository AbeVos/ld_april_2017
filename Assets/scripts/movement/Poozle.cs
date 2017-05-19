using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poozle : MonoBehaviour
{
	private enum State
	{
		Free,
		Jump
	}

	[SerializeField]
	private float speed = 5f;
	private float gravity = 9.8f;

	private Transform avatar;

	private CharacterController controller;
	new private CameraManager camera;

	private State current_state = State.Free;
	private float state_time = 0f;

	private Vector3 direction;

	/// <summary>Candidate jump start is set by JumpTarget property.</summary>
	private JumpTrigger candidate_jump_start;
	/// <summary>Candidate jump target is set by JumpTarget property.</summary>
	private JumpTrigger candidate_jump_target;

	/// <summary>Current jump start is used to get start position during jumping.</summary>
	private JumpTrigger current_jump_start;
	/// <summary>Current jump target is used to get target position during jumping.</summary>
	private JumpTrigger current_jump_target;

	private Vector3 jump_start_position;
	private Vector3 jump_target_position;

	public Vector3 Direction { get { return direction; } }

	public JumpTrigger JumpStart
	{
		get { return candidate_jump_start; }
		set { candidate_jump_start = value; }
	}

	public JumpTrigger JumpTarget
	{ 
		get { return candidate_jump_target; }
		set { candidate_jump_target = value; }
	}

	protected void Awake()
	{
		avatar = transform.GetChild(0);

		camera = GetComponent<CameraManager> ();
		controller = GetComponent<CharacterController> ();
	}

	protected void Update()
	{
		switch (current_state)
		{
		case State.Free:

			direction = InputDirection ();
			transform.rotation = Rotate (direction);

			direction = direction.normalized;
			controller.Move(Time.deltaTime * (direction.magnitude * transform.forward * speed + gravity * Vector3.down));

			if (Input.GetKeyDown(KeyCode.Space))
			{
				SetState(State.Jump);
			}

			break;

		case State.Jump:

			if (current_jump_target != null)
			{
				//current_jump_target.NearestPoint(transform.position);

				//transform.position = Vector3.Lerp(transform.position, current_jump_target.transform.position, state_time);
				transform.position = Vector3.Lerp(jump_start_position, jump_target_position, state_time);
			}

			if (state_time >= 1f)
			{
				SetState(State.Free);
			}

			break;
		}

		state_time += Time.deltaTime;
	}

	private void SetState(State new_state)
	{
		Debug.Log(new_state);

		if (new_state == State.Jump)
		{
			current_jump_start = candidate_jump_start;
			current_jump_target = candidate_jump_target;

			if (current_jump_start != null)
			{
				jump_start_position = current_jump_start.NearestPoint(transform.position);
				jump_target_position = current_jump_target.NearestPoint(jump_start_position);
			}
		}

		if (current_state == State.Jump)
		{
			current_jump_target = null;
		}

		current_state = new_state;

		state_time = 0f;
	}

	private Vector3 InputDirection()
	{
		Vector3 direction = Vector3.zero;

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

		return direction;
	}

	private Quaternion Rotate(Vector3 direction)
	{
		if (direction.sqrMagnitude > 0)
		{
			return Quaternion.Lerp(
				transform.rotation,
				Quaternion.LookRotation(direction),
				2f * speed * Time.deltaTime);
		}
		else
		{
			return transform.rotation;
		}	
	}
}
