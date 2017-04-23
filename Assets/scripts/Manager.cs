using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
	[SerializeField]
	protected Scenes next_scene;

	protected virtual void Awake()
	{
		UIManager.FadeIn ();
	}

	protected virtual void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			ChangeScene ();
		}
	}

	protected void ChangeScene()
	{
		UIManager.FadeOut();

		StartCoroutine(FadeOut(1f));
	}

	private IEnumerator FadeOut(float time=1f)
	{
		yield return new WaitForSeconds (time);

		Game.ChangeScene (next_scene);
	}
}