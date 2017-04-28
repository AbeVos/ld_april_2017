using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Interactive
{
	protected override void Interaction(float time)
	{
		StopInteraction();
		Interactor.StopInteraction();
	}

	public override void StopInteraction()
	{
		SetState(State.Finished);
		UIManager.HidePrompt();
		GameManager.EndGame();
	}
}
