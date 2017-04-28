using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : Interactive
{
	protected override void Interaction(float time)
	{
		transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, time/ 2);

		if (time >= 2f)
		{
			StopInteraction();
			Interactor.StopInteraction();

			Destroy(gameObject);
		}
	}
}
