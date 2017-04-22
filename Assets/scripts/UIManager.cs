using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	private enum FadeState
	{
		FadeIn,
		White,
		FadeOut,
		Black
	};

	private static Color awhite = new Color(1,1,1,0);
	private static Color ablack = new Color (0, 0, 0, 0);

	private static Image fader;
	private static Text prompt;
	private static FadeState current_state = FadeState.Black;

	private static float current_time = 0f;

	private static Transform position;
	private static Color target_color;

	protected void Awake()
	{
		fader = transform.GetChild(0).GetComponent<Image>();
		prompt = transform.GetChild(1).GetComponent<Text>();
	}

	protected void Update()
	{
		current_time += Time.deltaTime;

		switch (current_state)
		{
		case FadeState.FadeIn:
			fader.color = Color.Lerp (Color.black, ablack, current_time);

			if (current_time >= 1f)
			{
				current_state = FadeState.White;
			}
			break;

		case FadeState.White:
			fader.color = ablack;
			break;

		case FadeState.FadeOut:
			fader.color = Color.Lerp (ablack, Color.black, current_time);

			if (current_time >= 1f)
			{
				current_state = FadeState.Black;
			}
			break;

		case FadeState.Black:
			fader.color = Color.black;
			break;
		}

		if (position != null)
		{
			prompt.color = Color.Lerp(prompt.color, target_color, 10f * Time.deltaTime);
			prompt.rectTransform.position = Camera.main.WorldToScreenPoint(position.position);
		}
	}

	public static void FadeOut(float time=1f)
	{
		Debug.Log ("Fade out");

		current_time = 0f;

		current_state = FadeState.FadeOut;
	}

	public static void FadeIn(float time=1f)
	{
		Debug.Log ("Fade in");

		current_time = 0f;

		current_state = FadeState.FadeIn;
	}

	public static void ShowPrompt(string message, Transform world_position)
	{
		prompt.text = message;
		target_color = Color.white;
		position = world_position;
	}

	public static void HidePrompt()
	{
		target_color = awhite;
	}
}
