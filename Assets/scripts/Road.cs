using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
	[SerializeField]
	private GameObject car_prefab;
	[SerializeField]
	private int max_cars = 10;
	[SerializeField]
	private float spawn_interval = 0.5f;
	[SerializeField]
	private float car_speed = 5f;

	private Player player;
	private BoxCollider collider;

	private List<GameObject> cars;
	private List<GameObject> cars_hold;
	private int car_idx;
	float road_length;

	private float t = 0f;

	protected void Awake()
	{
		player = FindObjectOfType<Player>();
		collider = GetComponent<BoxCollider>();

		cars = new List<GameObject>();
		cars_hold = new List<GameObject>();

		road_length = collider.size.z;
		float car_distance = road_length / max_cars;

		for (int i = 0; i < max_cars; i++)
		{
			GameObject obj = Instantiate(car_prefab,
				transform.position - 0.5f * road_length * transform.forward,
				transform.rotation, transform) as GameObject;
			obj.SetActive(false);
			cars_hold.Add(obj);
		}
	}

	protected void Update()
	{
		foreach (GameObject car in cars)
		{
			car.transform.localPosition += Time.deltaTime * car_speed * Vector3.forward;

			if (Vector3.Dot(car.transform.localPosition, Vector3.forward) > road_length / 2)
			{
				car.SetActive(false);
				car.transform.localPosition = -road_length / 2 * Vector3.forward;
				cars_hold.Add(car);
				cars.Remove(car);
				break;
			}
		}

		if (t >= spawn_interval && cars_hold.Count > 0)
		{
			float distance = Vector3.Distance(transform.position + transform.forward *
				Vector3.Dot(player.transform.position - transform.position,
					transform.forward),
				player.transform.position);

			float probability = Mathf.Max(0, 5f - distance + collider.size.x / 2f) / 5f;

			if (Random.value < probability)
			{
				cars_hold[0].SetActive(true);
				cars.Add(cars_hold[0]);
				cars_hold.Remove(cars_hold[0]);
			}

			t = 0f;
		}

		t += Time.deltaTime;
	}

	void OnDrawGizmos()
	{
		player = FindObjectOfType<Player>();

		Gizmos.DrawSphere(transform.position + transform.forward * Vector3.Dot(player.transform.position - transform.position, transform.forward), 0.5f);
	}
}
