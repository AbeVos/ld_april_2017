using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDispenser : MonoBehaviour
{
	private Text text_object;
	private RectTransform text_transform;

	private string[] messages;

	private bool dispensing_text = false;
	private float t = 0f;

    [SerializeField, FMODUnity.EventRef]
    private string xpEventRef;
    private FMOD.Studio.EventInstance xpEventInstance;

    protected void Awake()
	{
		text_object = GetComponentInChildren<Text>();
		text_transform = text_object.GetComponent<RectTransform>();

        xpEventInstance = FMODUnity.RuntimeManager.CreateInstance(xpEventRef);
        //text_object.enabled = false;

        messages = new string[] {
			"PURRfect!",
			"FURry Good!",
			"Nice CATch!",
			"CLAW-some!",
			"PAW-some!",
			"MEOWnificent!",
			"SuPURRior!",
			"Great CATtitude!",
			"ConCATulations!",
			"PURRiceless!",
			"MEOWfelous!"
		};
	}

	protected void Update()
	{
		if (dispensing_text)
		{
			text_transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
			//text_transform.anchoredPosition = 500f * (Mathf.Pow(2f * t - 1f, 2) - 1f) * Vector3.up;
			text_transform.anchoredPosition = 500f * 4 * Mathf.Pow(t, 2) * (t - 1) * Vector2.up;

			t += Time.deltaTime;

			if (t >= 1f)
			{
				dispensing_text = false;
			}

			text_object.color = Color.Lerp(text_object.color, Color.white, 3f * Time.deltaTime);	
		}
		else
		{
			text_object.color = Color.Lerp(text_object.color, new Color(1,1,1,0), 3f * Time.deltaTime);
			text_transform.anchoredPosition += 5f * Vector2.up;
		}
	}

	public void DispenseText()
	{
		if (dispensing_text == true) return;
	    xpEventInstance.start();

		text_object.text = messages[Random.Range(0,messages.Length)];

		dispensing_text = true;
		t = 0f;

		text_transform.localScale = Vector3.zero;
		text_object.enabled = true;
	}
}
