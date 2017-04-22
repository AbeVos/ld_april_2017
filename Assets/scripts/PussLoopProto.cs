using UnityEngine;

public class PussLoopProto : MonoBehaviour
{
    public Camera cam;

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        transform.position +=
            new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * 0.05f;

    }
}
