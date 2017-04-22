using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager
{
	private static Player player;

	public static Player Player { get { return player; } }

	protected override void Awake()
	{
		base.Awake();

		player = FindObjectOfType<Player>();
	}
}
