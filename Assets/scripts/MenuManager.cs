using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Manager
{
	protected override void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space))
		{
			ChangeScene ();
		}
	}
}
