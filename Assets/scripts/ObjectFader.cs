using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    private List<Transform> _blockingObjects;
    private List<Transform> _nonBlockingObjects;
    private Color _originalColor;
    private RaycastHit[] _hits;

    // Use this for initialization
    private void Start()
    {
        _blockingObjects = new List<Transform>();
        _nonBlockingObjects = new List<Transform>();
    }

    // Update is called once per frame
    private void Update()
    {
        Ray ray = new Ray(transform.position, Camera.main.transform.position - transform.position);

        foreach (Transform blockingObject in _blockingObjects)
        {
            _hits = Physics.RaycastAll(ray, 30, 1 << LayerMask.NameToLayer("BlocksView"));
            bool containsBlockObj = false;

            foreach (RaycastHit hit in _hits)
            {
                if (hit.transform == blockingObject)
                {
                    containsBlockObj = true;
                    break;
                }
            }

            if (!containsBlockObj)
            {
                _nonBlockingObjects.Add(blockingObject);
                _blockingObjects.Remove(blockingObject);
                break;
            }

            Material material = blockingObject.GetComponent<MeshRenderer>().material; 
            material.color = Color.Lerp(material.color, new Color(material.color.r, material.color.g, material.color.b, 0.3f), Time.deltaTime*2);
        }

        _hits = Physics.RaycastAll(ray, 30, 1 << LayerMask.NameToLayer("BlocksView"));

        foreach (RaycastHit hit in _hits)
        {
            if (!_blockingObjects.Contains(hit.transform))
            {
                _blockingObjects.Add(hit.transform);
            }
        }

        foreach (Transform nonBlockingObject in _nonBlockingObjects)
        {
            Material material = nonBlockingObject.GetComponent<MeshRenderer>().material;
            material.color = Color.Lerp(material.color, new Color(material.color.r, material.color.g, material.color.b, 1), Time.deltaTime*2);

            if (material.color.a > 0.95f)
            {
                material.color = new Color(material.color.r, material.color.g, material.color.b, 1);
                _nonBlockingObjects.Remove(nonBlockingObject);
                break;
                // Abe is beste B -1
            }
        }
    }

}
