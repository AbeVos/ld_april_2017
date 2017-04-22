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
	private float follow_distance = 16f;
	[SerializeField]
	private float interaction_distance = 8f;

	new private Camera camera;

	private Quaternion rest_rotation;
	private Quaternion interaction_rotation;
	private Vector3 forward;

	private State current_state = State.Following;

	public Camera Camera { get { return camera; } }

	public Vector3 Forward { get { return forward; } }
	public Vector3 Right { get { return camera.transform.right; } }

	protected void Awake()
	{
		camera = Camera.main;

		rest_rotation = camera.transform.rotation;
		interaction_rotation = Quaternion.Euler(camera.transform.eulerAngles - 30f * Vector3.right);
		forward = Vector3.Cross (camera.transform.right, Vector3.up);
	}

	protected void Update()
	{
		switch (current_state)
		{
		case State.Following:
			camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation,
				rest_rotation,
				5f * Time.deltaTime);

			camera.transform.position = Vector3.Lerp (camera.transform.position,
				transform.position - follow_distance * (rest_rotation * Vector3.forward),
				5f * Time.deltaTime);
			break;
		
		case State.Interaction:
			camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation,
				interaction_rotation,
				3f * Time.deltaTime);
			
			camera.transform.position = Vector3.Lerp (camera.transform.position,
				transform.position - interaction_distance * (interaction_rotation * Vector3.forward),
				3f * Time.deltaTime);
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
