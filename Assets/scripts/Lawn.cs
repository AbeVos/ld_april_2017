using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lawn : MonoBehaviour
{
    [SerializeField]
    private GameObject _grass;

    [SerializeField, Range(0.1f, 10f)]
    private float _density = 1f;
    [SerializeField, Range(0f, 360f)]
    private float _rotationVariance = 360f;

    private List<GameObject> _grassObjects;

    protected void Awake()
    {
        DestroyGrass();
        GenerateUniform();
    }

    private void GenerateUniform()
    {
        _grassObjects = new List<GameObject>();

        for (float x = -0.5f * transform.localScale.x; x < 0.5f * transform.localScale.x; x += 1f / _density)
        {
            for (float y = -0.5f * transform.localScale.z; y < 0.5f * transform.localScale.z; y += 1f / _density)
            {
                Vector3 position = x * transform.right + y * transform.forward;

                _grassObjects.Add(PlantGrass(transform.position + position));
            }
        }
    }

    private GameObject PlantGrass(Vector3 position)
    {
        Quaternion rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, _rotationVariance), 0f);

        RaycastHit hit;

        if (Physics.Raycast(position, Vector3.down, out hit))
        {
            GameObject grassObj = Instantiate(_grass, hit.point + 0.5f * Vector3.up, rotation);
            grassObj.transform.parent = transform;

            // Instanced materials werken blijkbaar niet met static batching
            //grassObj.isStatic = true;

            return grassObj;
        }

        return null;
    }

    private void DestroyGrass()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(transform.childCount - 1).gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = new Color(1, 1, 0, 0.6f);
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
