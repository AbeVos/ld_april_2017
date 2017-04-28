using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
	private enum FadeState
	{
		FadeIn,
		White,
		FadeOut,
		Black
	};

	private static Color _awhite = new Color(1,1,1,0);
	private static Color _ablack = new Color (0, 0, 0, 0);

	private static Image _fader;
	private static Text _prompt;
	private static FadeState _currentState = FadeState.Black;

	private static float _currentTime = 0f;

	private static Transform _position;
	private static Color _targetColor;

	protected void Awake()
	{
		_fader = transform.FindChild("Fader").GetComponent<Image>();
		_prompt = transform.FindChild("Prompt").GetComponent<Text>();
	}

	protected void Update()
	{
		_currentTime += Time.deltaTime;

		switch (_currentState)
		{
		case FadeState.FadeIn:
			_fader.color = Color.Lerp (Color.black, _ablack, _currentTime);

			if (_currentTime >= 1f)
			{
				_currentState = FadeState.White;
			}
			break;

		case FadeState.White:
			_fader.color = _ablack;
			break;

		case FadeState.FadeOut:
			_fader.color = Color.Lerp (_ablack, Color.black, _currentTime);

			if (_currentTime >= 1f)
			{
				_currentState = FadeState.Black;
			}
			break;

		case FadeState.Black:
			_fader.color = Color.black;
			break;
		}

		if (_position != null)
		{
			_prompt.color = Color.Lerp(_prompt.color, _targetColor, 10f * Time.deltaTime);
			_prompt.rectTransform.position = Camera.main.WorldToScreenPoint(_position.position);
		}
	}

	public static void FadeOut(float time=1f)
	{
		Debug.Log ("Fade out");

		_currentTime = 0f;

		_currentState = FadeState.FadeOut;
	}

	public static void FadeIn(float time=1f)
	{
		Debug.Log ("Fade in");

		_currentTime = 0f;

		_currentState = FadeState.FadeIn;
	}

	public static void ShowPrompt(string message, Transform worldPosition)
	{
		_prompt.text = message;
		_targetColor = Color.white;
		_position = worldPosition;
	}

	public static void HidePrompt()
	{
		_targetColor = _awhite;
	}
}
