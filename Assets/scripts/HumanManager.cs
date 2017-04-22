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

	private Transform[] points_of_interest;

	protected void Awake()
	{
		points_of_interest = transform.GetComponentsInChildren<Transform>();

		humans = new List<Human>();
	}

	protected void Update()
	{
		if (humans.Count < max_humans)
		{
			humans.Add(SpawnHuman());
		}
	}

	protected void OnDrawGizmos()
	{
		points_of_interest = transform.GetComponentsInChildren<Transform>();

		Gizmos.color = Color.red;
		foreach (Transform poi in points_of_interest)
		{
			Gizmos.DrawSphere(poi.position, 0.1f);
		}
	}

	public Vector3 GetPointOfInterest()
	{
		return points_of_interest[Random.Range(0, points_of_interest.Length-1)].position;
	}

	public void RemoveHuman(Human human)
	{
		humans.Remove(human);
	}

	private Human SpawnHuman()
	{
		GameObject obj = Instantiate(human_prefab, GetPointOfInterest(), Quaternion.identity) as GameObject;
		return obj.GetComponent<Human>();
	}
}
