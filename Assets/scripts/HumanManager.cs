using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : MonoBehaviour
{
	[SerializeField]
	private GameObject human_prefab;
	[SerializeField]
	private int max_humans = 5;

	private List<Human> humans;

	private PointOfInterest[] points_of_interest;
	private HumanSpawner[] spawners;

	protected void Awake()
	{
		points_of_interest = transform.GetComponentsInChildren<PointOfInterest>();
		spawners = transform.GetComponentsInChildren<HumanSpawner>();

		humans = new List<Human>();
	}

	protected void Update()
	{
		if (humans.Count < max_humans)
		{
			HumanSpawner spawner = spawners[Random.Range(0, spawners.Length)];

			humans.Add(spawner.SpawnHuman(transform));
		}
	}

	protected void OnDrawGizmos()
	{
		points_of_interest = transform.GetComponentsInChildren<PointOfInterest>();

		Gizmos.color = Color.red;
		foreach (PointOfInterest poi in points_of_interest)
		{
			Gizmos.DrawSphere(poi.transform.position, 1.8f);
		}
	}

	public Vector3 GetPointOfInterest()
	{
		return points_of_interest[Random.Range(0, points_of_interest.Length-1)].transform.position;
	}

	public void RemoveHuman(Human human)
	{
		humans.Remove(human);
	}
}
