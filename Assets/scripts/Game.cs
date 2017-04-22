using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
	Main=1,
	Game=2,
	Score=3
};

public class Game : MonoBehaviour
{
	private static Scenes current_scene = 0;
	private static Manager current_manager;

	protected void Awake()
	{
		SceneManager.sceneLoaded += SceneManager_sceneLoaded;

		ChangeScene (Scenes.Main);
	}

	public static void RegisterManager(Manager manager)
	{
		current_manager = manager;
	}

	public static void ChangeScene(Scenes new_scene)
	{
		Debug.Log ("Change scene from " + current_scene + " to " + new_scene);

		if ((int)current_scene > 0) 
		{
			Debug.Log ("Unload " + current_scene);
			SceneManager.UnloadSceneAsync((int)current_scene);
		}

		current_scene = new_scene;

		SceneManager.LoadSceneAsync((int)current_scene, LoadSceneMode.Additive);
	}

	private void SceneManager_sceneLoaded (Scene scene, LoadSceneMode loadSceneMode)
	{
		
	}
}
