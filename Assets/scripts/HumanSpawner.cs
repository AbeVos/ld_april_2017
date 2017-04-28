using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _humanPrefab;

    private Transform _door;

    private bool _openDoor = false;
    private float _t = 0f;

    protected void Awake()
    {
        _door = transform.FindChild("Door");
    }

    protected void Update()
    {


        //door.localRotation = new Quaternion(0,6f,0,1);

        if (_openDoor)
        {
            //Debug.Log("Opening door");
            _door.localRotation = Quaternion.Lerp(_door.localRotation, Quaternion.Euler(120f * Vector3.up), 3f * Time.deltaTime);

            _t += Time.deltaTime;
            if (_t >= 3f) _openDoor = false;
        }
        else
        {
            _door.localRotation = Quaternion.Lerp(_door.localRotation, Quaternion.Euler(0f * Vector3.up), 3f * Time.deltaTime);
        }
    }

    protected void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.transform.name);
    }

    public Human SpawnHuman(Transform parent)
    {
        GameObject obj = Instantiate<GameObject>(_humanPrefab, transform.position, Quaternion.identity, parent);
        Human human = obj.GetComponent<Human>();

        if (Random.Range(0, 2) == 0)
        {
            human.Type = Score.CatPerson;
        }
        else
        {
            human.Type = Score.DogPerson;
        }

        if (Random.Range(0, 2) == 0)
        {
            human.VoiceType = HumanVoiceType.Masculine;
        }
        else
        {
            human.VoiceType = HumanVoiceType.Feminine;
        }

//        Debug.Log(human.VoiceType.ToString());

        MaterialPropertyBlock props = new MaterialPropertyBlock();
        Color randomColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.3f, 1f);
        randomColor.a = 0.6f;
        props.SetColor("_Color", randomColor);
        props.SetColor("_HairColor", Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.2f, 0.6f));
        props.SetFloat("_SkinTone", Random.Range(0.1f, 0.9f));

        obj.GetComponentInChildren<SkinnedMeshRenderer>().SetPropertyBlock(props);

        _openDoor = true;
        _t = 0f;

        return human;
    }
}
