using UnityEngine;
using System.Collections;

public class DoorSwitch : MonoBehaviour {

    public bool triggered;
    public Light[] lights;

    private void Update()
    {
        if (triggered)
        {
            GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
            foreach(Light lighttt in lights)
            {
                lighttt.color = Color.white;
            }
        }
    }
}
