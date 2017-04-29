using UnityEngine;

public class MenuManager : Manager
{
	protected override void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			ChangeScene ();
		}
	}
}
