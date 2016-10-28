using UnityEngine;
using System.Collections;

public class DoorSwitch : MonoBehaviour {

    public bool triggered;
    
    private void Update()
    {
        if (triggered)
        {
            GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
        }
    }
}
