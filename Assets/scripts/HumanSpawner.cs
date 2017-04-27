using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject human_prefab;

    private Transform door;

    private bool open_door = false;
    private float t = 0f;

    protected void Awake()
    {
        door = transform.FindChild("Door");
    }

    protected void Update()
    {


        //door.localRotation = new Quaternion(0,6f,0,1);

        if (open_door)
        {
            //Debug.Log("Opening door");
            door.localRotation = Quaternion.Lerp(door.localRotation, Quaternion.Euler(120f * Vector3.up), 3f * Time.deltaTime);

            t += Time.deltaTime;
            if (t >= 3f) open_door = false;
        }
        else
        {
            door.localRotation = Quaternion.Lerp(door.localRotation, Quaternion.Euler(0f * Vector3.up), 3f * Time.deltaTime);
        }
    }

    protected void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.transform.name);
    }

    public Human SpawnHuman(Transform parent)
    {
        GameObject obj = Instantiate(human_prefab, transform.position, Quaternion.identity, parent) as GameObject;
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
            human.voiceType = HumanVoiceType.masculine;
        }
        else
        {
            human.voiceType = HumanVoiceType.feminine;
        }

        Debug.Log(human.voiceType.ToString());

        MaterialPropertyBlock props = new MaterialPropertyBlock();
        Color randomColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.3f, 1f);
        randomColor.a = 0.6f;
        props.SetColor("_Color", randomColor);
        props.SetColor("_HairColor", Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.2f, 0.6f));
        props.SetFloat("_SkinTone", Random.Range(0.1f,0.9f));

        obj.GetComponentInChildren<SkinnedMeshRenderer>().SetPropertyBlock(props);

        open_door = true;
        t = 0f;

        return human;
    }
}
