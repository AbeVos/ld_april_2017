using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : MonoBehaviour
{
	private static Transform[] points_of_interest;

	public static Transform[] PointsOfInterest
	{
		get { return points_of_interest; }
	}

	protected void Awake()
	{
		points_of_interest = transform.GetComponentsInChildren<Transform>();
	}

	public static Vector3 GetPointOfInterest()
	{
		return points_of_interest[Random.Range(0, points_of_interest.Length-1)].position;
	}
}
