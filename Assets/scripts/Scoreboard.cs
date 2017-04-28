using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
	private Text _objectsText;
	private Text _scoresText;
	private Text _valuesText;

	protected void Awake()
	{
		_objectsText = transform.GetChild(1).GetComponent<Text>();
		_scoresText = transform.GetChild(2).GetComponent<Text>();
		_valuesText = transform.GetChild(3).GetComponent<Text>();

		string[] names = System.Enum.GetNames(typeof(Score));

		_objectsText.text = "";
		for (int i=0; i < names.Length; i++)
		{
			_objectsText.text += AddSpacesToSentence(names[i], false) + "\n";
		}
		_objectsText.text += "\n\nTotal:";
	}

	public void SetScore(int[] scores, int[] scoreValues)
	{
		_scoresText.text = "";
		for (int i=0; i < scores.Length; i++)
		{
			_scoresText.text += scores[i] + "\n";
		}

		_valuesText.text = "";
		for (int i=0; i < scores.Length; i++)
		{
			_valuesText.text += "x " + scoreValues[i] + "\n";
		}

		int totalScore = 0;

		for (int i=0; i < scores.Length; i++)
		{
			totalScore += scores[i] * scoreValues[i];
		}

		_scoresText.text += "\n\n" + totalScore + "pts.";
	}

	private string AddSpacesToSentence(string text, bool preserveAcronyms)
	{
		StringBuilder newText = new StringBuilder(text.Length * 2);
		newText.Append(text[0]);
		for (int i = 1; i < text.Length; i++)
		{
			if (char.IsUpper(text[i]))
				if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
					(preserveAcronyms && char.IsUpper(text[i - 1]) && 
						i < text.Length - 1 && !char.IsUpper(text[i + 1])))
					newText.Append(' ');
			newText.Append(text[i]);
		}
		return newText.ToString();
	}
}
