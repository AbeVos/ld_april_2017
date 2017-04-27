using UnityEngine;

public class PropTintRandomizer : MonoBehaviour
{
    [SerializeField] private Vector2 HueRange, SaturationRange,ValueRange;

	void Awake ()
	{
	    GetComponent<MeshRenderer>().material.color = Random.ColorHSV(HueRange.x, HueRange.y, 
            SaturationRange.x, SaturationRange.y, ValueRange.x, ValueRange.y);
	}

}
