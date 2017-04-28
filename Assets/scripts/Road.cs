using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField]
    private GameObject _carPrefab;
    [SerializeField]
    private int _maxCars = 10;
    [SerializeField]
    private float _spawnInterval = 0.5f;
    [SerializeField]
    private float _carSpeed = 5f;

    private Player _player;
    private BoxCollider _collider;

    private List<GameObject> _cars;
    private List<GameObject> _carsHold;
    private int _carIdx;
    float _roadLength;

    private float _t = 0f;

    protected void Awake()
    {
        _player = FindObjectOfType<Player>();
        _collider = GetComponent<BoxCollider>();

        _cars = new List<GameObject>();
        _carsHold = new List<GameObject>();

        _roadLength = _collider.size.z;
        float carDistance = _roadLength / _maxCars;

        for (int i = 0; i < _maxCars; i++)
        {
            GameObject obj = Instantiate(_carPrefab,
                transform.position - 0.5f * _roadLength * transform.forward,
                transform.rotation, transform) as GameObject;
            obj.SetActive(false);
            _carsHold.Add(obj);
        }
    }

    protected void Update()
    {
        foreach (GameObject car in _cars)
        {
            car.transform.localPosition += Time.deltaTime * _carSpeed * Vector3.forward;

            if (Vector3.Dot(car.transform.localPosition, Vector3.forward) > _roadLength / 2)
            {
                car.SetActive(false);
                car.transform.localPosition = -_roadLength / 2 * Vector3.forward;
                _carsHold.Add(car);
                _cars.Remove(car);
                break;
            }
        }

        if (_t >= _spawnInterval && _carsHold.Count > 0)
        {
            float distance = Vector3.Distance(transform.position + transform.forward *
                Vector3.Dot(_player.transform.position - transform.position,
                    transform.forward),
                _player.transform.position);

            float probability = Mathf.Max(0, 5f - distance + _collider.size.x / 2f) / 5f;

            if (Random.value < probability)
            {
                _carsHold[0].SetActive(true);
                _cars.Add(_carsHold[0]);
                _carsHold.Remove(_carsHold[0]);
            }

            _t = 0f;
        }

        _t += Time.deltaTime;
    }

    void OnDrawGizmos()
    {
        _player = FindObjectOfType<Player>();

        Gizmos.DrawSphere(transform.position + transform.forward * Vector3.Dot(_player.transform.position - transform.position, transform.forward), 0.5f);
    }
}
