using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private float movement_speed = 1f;

	private CharacterController controller;
	private CameraManager camera;

	private Vector3 direction;

	protected void Awake()
	{
		controller = GetComponent<CharacterController> ();
		camera = GetComponent<CameraManager> ();

		direction = new Vector3 ();
	}

	protected void Update()
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

		direction = direction.normalized * movement_speed;

		controller.SimpleMove (direction);
	}
}
