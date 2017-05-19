using UnityEngine;
using UnityEngine.UI;

public class TextDispenser : MonoBehaviour
{
    private Text _textObject;
    private RectTransform _textTransform;

    private string[] _messages;

    private bool _dispensingText = false;
    private float _t = 0f;

    protected void Awake()
    {
        _textObject = GetComponentInChildren<Text>();
        _textTransform = _textObject.GetComponent<RectTransform>();

        //text_object.enabled = false;

        _messages = new string[] {
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
        if (_dispensingText)
        {
            _textTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, _t);
            //text_transform.anchoredPosition = 500f * (Mathf.Pow(2f * t - 1f, 2) - 1f) * Vector3.up;
            _textTransform.anchoredPosition = 500f * 4 * Mathf.Pow(_t, 2) * (_t - 1) * Vector2.up;

            _t += Time.deltaTime;

            if (_t >= 1f)
            {
                _dispensingText = false;
            }

            _textObject.color = Color.Lerp(_textObject.color, Color.white, 3f * Time.deltaTime);
        }
        else
        {
            _textObject.color = Color.Lerp(_textObject.color, new Color(1, 1, 1, 0), 3f * Time.deltaTime);
            _textTransform.anchoredPosition += 5f * Vector2.up;
        }
    }

    public void DispenseText()
    {
        if (_dispensingText) return;
        // TODO: Add xp sound

        _textObject.text = _messages[Random.Range(0, _messages.Length)];

        _dispensingText = true;
        _t = 0f;

        _textTransform.localScale = Vector3.zero;
        _textObject.enabled = true;
    }
}
