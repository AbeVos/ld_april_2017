using UnityEngine;

public class PropScaleRandomizer : MonoBehaviour
{
    [SerializeField] private Vector2 _xRange, _yRange, _zRange;

    void Awake()
    {
        Vector3 newScale = new Vector3(
            Random.Range(_xRange.x, _xRange.y) * transform.localScale.x,
            Random.Range(_yRange.x, _yRange.y) * transform.localScale.y,
            Random.Range(_zRange.x, _zRange.y) * transform.localScale.z);

        transform.localScale =  newScale;
    }

}
