using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	private enum State
	{
		Following,
		Interaction
	}

	[SerializeField]
	float follow_distance = 16f;
	[SerializeField]
	float interaction_distance = 8f;

	private Camera camera;

	private Quaternion rest_direction;
	private Vector3 forward;
	private Vector3 target;

	private State current_state = State.Following;

	public Camera Camera { get { return camera; } }

	public Vector3 Forward { get { return forward; } }
	public Vector3 Right { get { return camera.transform.right; } }
	public Vector3 Target { set { target = value; } }

	protected void Awake()
	{
		camera = Camera.main;

		rest_direction = camera.transform.rotation;
		forward = Vector3.Cross (camera.transform.right, Vector3.up);
	}

	protected void Update()
	{
		switch (current_state)
		{
		case State.Following:
			camera.transform.position = Vector3.Lerp (camera.transform.position,
				transform.position - follow_distance * (rest_direction * Vector3.forward),
				5f * Time.deltaTime);
			break;
		
		case State.Interaction:
			camera.transform.position = Vector3.Lerp (camera.transform.position,
				transform.position - interaction_distance * (rest_direction * Vector3.forward),
				5f * Time.deltaTime);
			break;
		}

	}

	public void StartInteraction()
	{
		current_state = State.Interaction;
	}

	public void StopInteraction()
	{
		current_state = State.Following;
	}
}
