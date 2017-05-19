using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class JumpTrigger : MonoBehaviour
{
	[SerializeField]
	private JumpTrigger target;

	private BoxCollider trigger;

	protected void Awake()
	{
		trigger = GetComponent<BoxCollider>();
		trigger.isTrigger = true;
	}

	protected void OnTriggerEnter(Collider col)
	{
		Poozle poozle = col.GetComponent<Poozle>();

		if (poozle != null)
		{
			poozle.JumpStart = this;
			poozle.JumpTarget = target;
		}
	}

	protected void OnTriggerExit(Collider col)
	{
		Poozle poozle = col.GetComponent<Poozle>();

		if (poozle != null && poozle.JumpTarget.Equals(target))
		{
			poozle.JumpStart = null;
			poozle.JumpTarget = null;
		}
	}

	protected void OnDrawGizmos()
	{
		Gizmos.DrawLine(-transform.right * trigger.bounds.extents.x + transform.position,
			transform.right * trigger.bounds.extents.x + transform.position);
	}

	public Vector3 NearestPoint(Vector3 position)
	{
		Vector3 t = position - transform.position;
		Vector3 u = Vector3.Normalize(transform.right * trigger.bounds.extents.x);

		return Vector3.Dot(u, t) * u + transform.position;
	}
}
