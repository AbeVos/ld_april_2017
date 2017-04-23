using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    private List<Transform> blockingObjects;
    private List<Transform> nonBlockingObjects;
    private Color originalColor;
    private RaycastHit[] hits;

    // Use this for initialization
    private void Start()
    {
        blockingObjects = new List<Transform>();
        nonBlockingObjects = new List<Transform>();
    }

    // Update is called once per frame
    private void Update()
    {
        Ray ray = new Ray(transform.position, Camera.main.transform.position - transform.position);
        RaycastHit newHit;

        foreach (Transform blockingObject in blockingObjects)
        {
            hits = Physics.RaycastAll(ray, 30, 1 << LayerMask.NameToLayer("BlocksView"));
            bool contains_block_obj = false;

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform == blockingObject)
                {
                    contains_block_obj = true;
                    break;
                }
            }

            if (!contains_block_obj)
            {
                nonBlockingObjects.Add(blockingObject);
                blockingObjects.Remove(blockingObject);
                break;
            }

            Material material = blockingObject.GetComponent<MeshRenderer>().material; 
            material.color = Color.Lerp(material.color, new Color(material.color.r, material.color.g, material.color.b, 0.3f), Time.deltaTime*2);
        }

        hits = Physics.RaycastAll(ray, 30, 1 << LayerMask.NameToLayer("BlocksView"));

        foreach (RaycastHit hit in hits)
        {
            if (!blockingObjects.Contains(hit.transform))
            {
                blockingObjects.Add(hit.transform);
            }
        }

        foreach (Transform nonBlockingObject in nonBlockingObjects)
        {
            Material material = nonBlockingObject.GetComponent<MeshRenderer>().material;
            material.color = Color.Lerp(material.color, new Color(material.color.r, material.color.g, material.color.b, 1), Time.deltaTime*2);

            if (material.color.a > 0.95f)
            {
                material.color = new Color(material.color.r, material.color.g, material.color.b, 1);
                nonBlockingObjects.Remove(nonBlockingObject);
                break;
                // Abe is beste B -1
            }
        }
    }

}
