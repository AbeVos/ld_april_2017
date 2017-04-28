using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
	None=0,
	Main=1,
	Game=2,
	Score=3
};

public enum Score
{
	CatPerson,
	DogPerson,
	Box,
	Yarn,
	Fish
}

public class Game : MonoBehaviour
{
	[SerializeField]
	private bool _loadScene = true;

	private static Scenes _currentScene = Scenes.None;

	private static int[] _scoreValues;
	private static int[] _scores;

	private static TextDispenser _textDispenser;
	private static Scoreboard _scoreboard;

	protected void Awake()
	{
		SceneManager.sceneLoaded += SceneManager_sceneLoaded;

		if (_loadScene)
		{
			ChangeScene (Scenes.Main);
		}

		_scoreValues = new int[] {100, 20, 30, 5, 60};
		_scores = new int[5];

		_textDispenser = FindObjectOfType<TextDispenser>();
		_scoreboard = FindObjectOfType<Scoreboard>();
	}

	public static void ChangeScene(Scenes newScene)
	{
		Debug.Log ("Change scene from " + _currentScene + " to " + newScene);

		if ((int)_currentScene > 0) 
		{
			Debug.Log ("Unload " + _currentScene);
			SceneManager.UnloadSceneAsync((int)_currentScene);
		}

		_currentScene = newScene;

		SceneManager.LoadSceneAsync((int)_currentScene, LoadSceneMode.Additive);
	}

	public static void AddScore(Score type)
	{
		Debug.Log("Found a " + type.ToString() + "!");
		_scores[(int)type]++;

		_textDispenser.DispenseText();
	}

	private void SceneManager_sceneLoaded (Scene scene, LoadSceneMode loadSceneMode)
	{
		if (scene.buildIndex == 3)
		{
			_scoreboard.gameObject.SetActive(true);
			_scoreboard.SetScore(_scores, _scoreValues);
		}
		else
		{
			_scoreboard.gameObject.SetActive(false);
		}	
	}
}
