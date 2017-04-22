using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	private enum State
	{
		FadeIn,
		White,
		FadeOut,
		Black
	};

	private static Color ablack = new Color (0, 0, 0, 0);

	private static Image fader;
	private static State current_state = State.Black;

	private static float current_time = 0f;
	private static float time_taken;

	protected void Awake()
	{
		fader = transform.GetChild (0).GetComponent<Image> ();
	}

	protected void Update()
	{
		current_time += Time.deltaTime;

		switch (current_state)
		{
		case State.FadeIn:
			fader.color = Color.Lerp (Color.black, ablack, current_time);

			if (current_time >= 1f)
			{
				current_state = State.White;
			}
			break;

		case State.White:
			fader.color = ablack;
			break;

		case State.FadeOut:
			fader.color = Color.Lerp (ablack, Color.black, current_time);

			if (current_time >= 1f)
			{
				current_state = State.Black;
			}
			break;

		case State.Black:
			fader.color = Color.black;
			break;
		}
	}

	public static void FadeOut(float time=1f)
	{
		Debug.Log ("Fade out");

		current_time = 0f;
		time_taken = time;

		current_state = State.FadeOut;
	}

	public static void FadeIn(float time=1f)
	{
		Debug.Log ("Fade in");

		current_time = 0f;
		time_taken = time;

		current_state = State.FadeIn;
	}

	public static void ShowPrompt(string message, Vector3 world_position)
	{
		Debug.Log (message);

		//TODO: Show text object on UI
	}

	public static void HidePrompt()
	{
		//TODO: Hide text object from UI
	}
}
