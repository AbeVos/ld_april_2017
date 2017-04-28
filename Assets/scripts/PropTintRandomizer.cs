using UnityEngine;

public class PropTintRandomizer : MonoBehaviour
{
    [SerializeField] private Vector2 _hueRange, _saturationRange, _valueRange;

    void Awake()
    {
        GetComponent<MeshRenderer>().material.color = Random.ColorHSV(_hueRange.x, _hueRange.y,
            _saturationRange.x, _saturationRange.y, _valueRange.x, _valueRange.y);
    }

}
