using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	[SerializeField]
	float camera_distance = 5f;

	private Camera camera;

	private Quaternion rest_direction;
	private Vector3 forward;

	public Camera Camera { get { return camera; } }

	public Vector3 Forward { get { return forward; } }
	public Vector3 Right { get { return camera.transform.right; } }

	protected void Awake()
	{
		camera = Camera.main;

		rest_direction = camera.transform.rotation;
		forward = Vector3.Cross (camera.transform.right, Vector3.up);
	}

	protected void Update()
	{
		Debug.DrawRay (camera.transform.position, camera_distance * (rest_direction * Vector3.forward));

		camera.transform.position = Vector3.Lerp (camera.transform.position,
			transform.position - camera_distance * (rest_direction * Vector3.forward),
			5f * Time.deltaTime);
	}
}
