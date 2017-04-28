using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
	[SerializeField]
	protected Scenes NextScene;

	protected virtual void Awake()
	{
		UiManager.FadeIn ();
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
		UiManager.FadeOut();

		StartCoroutine(FadeOut(1f));
	}

	private IEnumerator FadeOut(float time=1f)
	{
		yield return new WaitForSeconds (time);

		Game.ChangeScene (NextScene);
	}
}