using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yarn : Interactive
{
	new protected Rigidbody rigidbody;

	protected void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	protected override void Interaction(float time)
	{
		Vector3 direction = (transform.position - interactor.transform.position).normalized;

		rigidbody.AddForce(0.1f * direction, ForceMode.Impulse);

		StopInteraction();
		interactor.StopInteraction();
	}
}
