using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Interactive
{
	protected override void Interaction(float time)
	{
		GetComponent<BoxCollider>().enabled = false;
		GetComponent<SphereCollider>().enabled = false;
		interactor.transform.position = Vector3.Lerp(interactor.transform.position, transform.position, 5f * Time.deltaTime);

		if (time >= 2f)
		{
			interactor.transform.position += transform.forward;
			GetComponent<BoxCollider>().enabled = true;
			GetComponent<SphereCollider>().enabled = true;

			StopInteraction();
			interactor.StopInteraction();
		}
	}
}
