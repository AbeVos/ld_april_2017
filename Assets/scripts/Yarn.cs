using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yarn : Interactive
{
	new protected Rigidbody Rigidbody;

	protected void Awake()
	{
		Rigidbody = GetComponent<Rigidbody>();
	}

	protected override void Interaction(float time)
	{
		Vector3 direction = (transform.position - Interactor.transform.position).normalized;

		Rigidbody.AddForce(0.1f * direction, ForceMode.Impulse);

		StopInteraction();
		Interactor.StopInteraction();
	}
}
