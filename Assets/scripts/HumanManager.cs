using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : MonoBehaviour
{
    [SerializeField]
    private int _maxHumans = 5;

    private List<Human> _humans;

    private PointOfInterest[] _pointsOfInterest;
    private HumanSpawner[] _spawners;

    protected void Awake()
    {
        _pointsOfInterest = transform.GetComponentsInChildren<PointOfInterest>();
        _spawners = transform.GetComponentsInChildren<HumanSpawner>();

        _humans = new List<Human>();
    }

    protected void Update()
    {
        if (_humans.Count < _maxHumans)
        {
            HumanSpawner spawner = _spawners[Random.Range(0, _spawners.Length)];

            _humans.Add(spawner.SpawnHuman(transform));
        }
    }

    protected void OnDrawGizmos()
    {
        _pointsOfInterest = transform.GetComponentsInChildren<PointOfInterest>();

        Gizmos.color = Color.red;
        foreach (PointOfInterest poi in _pointsOfInterest)
        {
            Gizmos.DrawSphere(poi.transform.position, 1.8f);
        }
    }

    public Vector3 GetPointOfInterest()
    {
        return _pointsOfInterest[Random.Range(0, _pointsOfInterest.Length - 1)].transform.position;
    }

    public void RemoveHuman(Human human)
    {
        _humans.Remove(human);
    }
}
