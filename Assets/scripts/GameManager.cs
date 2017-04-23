using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager
{
	private static GameManager manager;

	private static Player player;
	private static HumanManager human_manager;

	public static GameManager Mgr { get { return manager; } }
	public static Player Player { get { return player; } }
	public static HumanManager HumanManager { get { return human_manager; } }

	protected override void Awake()
	{
		base.Awake();

		manager = this;
		player = FindObjectOfType<Player>();
		human_manager = FindObjectOfType<HumanManager>();
	}

	public static void EndGame()
	{
		manager.ChangeScene();
	}
}
