using UnityEngine;
using System.Collections;

public class DoorSwitch : MonoBehaviour {

    public bool triggered;
    public Light[] lights;
    public GameObject[] electricFloors;

    private void Update()
    {
        if (triggered)
        {
            GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
            foreach(Light lighttt in lights)
            {
                lighttt.color = Color.gray;
                lighttt.intensity = 5;
            }
            foreach(GameObject i in electricFloors)
            {
                i.tag = "Electric";
                i.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}
