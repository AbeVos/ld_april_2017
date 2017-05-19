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
    private float followDistance = 14f;
    [SerializeField]
    private float interactionDistance = 8f;

    private Camera camera;
    private Poozle player;

    private Quaternion restRotation;
    private Quaternion interactionRotation;
    private Vector3 forward;

    private State currentState = State.Following;

    public Camera Camera { get { return camera; } }

    public Vector3 Forward { get { return forward; } }
    public Vector3 Right { get { return camera.transform.right; } }

    protected void Awake()
    {
        camera = Camera.main;
		player = GetComponent<Poozle>();

        restRotation = camera.transform.rotation;
        interactionRotation = Quaternion.Euler(camera.transform.eulerAngles - 30f * Vector3.right);
        forward = Vector3.Cross(camera.transform.right, Vector3.up);
    }

    protected void Update()
    {
        switch (currentState)
        {
            case State.Following:
                camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation,
                    restRotation, 5f * Time.deltaTime);

                camera.transform.position = Vector3.Lerp(camera.transform.position,
                    (transform.position - followDistance * (restRotation * Vector3.forward)) + player.Direction * 2f, 5f * Time.deltaTime);
                break;

            case State.Interaction:
                camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation,
                    interactionRotation, 3f * Time.deltaTime);

                camera.transform.position = Vector3.Lerp(camera.transform.position,
                    transform.position - interactionDistance * (interactionRotation * Vector3.forward), 3f * Time.deltaTime);
                break;
        }
    }

    public void StartInteraction()
    {
        currentState = State.Interaction;
    }

    public void StopInteraction()
    {
        currentState = State.Following;
    }
}
