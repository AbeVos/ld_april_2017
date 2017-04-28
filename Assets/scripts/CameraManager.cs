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
    private float _followDistance = 14f;
    [SerializeField]
    private float _interactionDistance = 8f;

    private Camera _camera;

    private Quaternion _restRotation;
    private Quaternion _interactionRotation;
    private Vector3 _forward;

    private State _currentState = State.Following;

    public Camera Camera { get { return _camera; } }

    public Vector3 Forward { get { return _forward; } }
    public Vector3 Right { get { return _camera.transform.right; } }

    protected void Awake()
    {
        _camera = Camera.main;

        _restRotation = _camera.transform.rotation;
        _interactionRotation = Quaternion.Euler(_camera.transform.eulerAngles - 30f * Vector3.right);
        _forward = Vector3.Cross(_camera.transform.right, Vector3.up);
    }

    protected void Update()
    {
        switch (_currentState)
        {
            case State.Following:
                _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation,
                    _restRotation, 5f * Time.deltaTime);

                _camera.transform.position = Vector3.Lerp(_camera.transform.position,
                    transform.position - _followDistance * (_restRotation * Vector3.forward), 5f * Time.deltaTime);
                break;

            case State.Interaction:
                _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation,
                    _interactionRotation, 3f * Time.deltaTime);

                _camera.transform.position = Vector3.Lerp(_camera.transform.position,
                    transform.position - _interactionDistance * (_interactionRotation * Vector3.forward), 3f * Time.deltaTime);
                break;
        }
    }

    public void StartInteraction()
    {
        _currentState = State.Interaction;
    }

    public void StopInteraction()
    {
        _currentState = State.Following;
    }
}
