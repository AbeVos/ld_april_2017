using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
	private Text objects_text;
	private Text scores_text;
	private Text values_text;

	protected void Awake()
	{
		objects_text = transform.GetChild(1).GetComponent<Text>();
		scores_text = transform.GetChild(2).GetComponent<Text>();
		values_text = transform.GetChild(3).GetComponent<Text>();

		string[] names = System.Enum.GetNames(typeof(Score));

		objects_text.text = "";
		for (int i=0; i < names.Length; i++)
		{
			objects_text.text += AddSpacesToSentence(names[i], false) + "\n";
		}
		objects_text.text += "\n\nTotal:";
	}

	public void SetScore(int[] scores, int[] score_values)
	{
		scores_text.text = "";
		for (int i=0; i < scores.Length; i++)
		{
			scores_text.text += scores[i] + "\n";
		}

		values_text.text = "";
		for (int i=0; i < scores.Length; i++)
		{
			values_text.text += "x " + score_values[i] + "\n";
		}

		int total_score = 0;

		for (int i=0; i < scores.Length; i++)
		{
			total_score += scores[i] * score_values[i];
		}

		scores_text.text += "\n\n" + total_score + "pts.";
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
