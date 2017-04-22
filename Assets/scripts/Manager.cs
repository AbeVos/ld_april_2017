using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
	[SerializeField]
	private Scenes next_scene;

	protected void Awake()
	{
		Game.RegisterManager (this);

		UIManager.FadeIn ();
	}

	protected void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			ChangeScene (next_scene, 1f);
		}
	}

	private void ChangeScene(Scenes next_scene, float time=1f)
	{
		UIManager.FadeOut();

		StartCoroutine(FadeOut(time));
	}

	private IEnumerator FadeOut(float time=1f)
	{
		yield return new WaitForSeconds (time);

		Game.ChangeScene (next_scene);
	}
}