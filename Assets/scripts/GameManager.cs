using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager
{
	private static GameManager _manager;

	private static Player _player;
	private static HumanManager _humanManager;

	public static GameManager Mgr { get { return _manager; } }
	public static Player Player { get { return _player; } }
	public static HumanManager HumanManager { get { return _humanManager; } }

	protected override void Awake()
	{
		base.Awake();

		_manager = this;
		_player = FindObjectOfType<Player>();
		_humanManager = FindObjectOfType<HumanManager>();
	}

	public static void EndGame()
	{
		_manager.ChangeScene();
	}
}
