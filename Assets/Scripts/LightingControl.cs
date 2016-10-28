using UnityEngine;
using System.Collections.Generic;

public class LightingControl : MonoBehaviour {

    public List<Light> pointLights = new List<Light>();
    private float intensityValue;
    private float startingIntensityValue;
    private float targetIntensityValue;
    private bool up;

    // Use this for initialization
    void Start () {
        startingIntensityValue = pointLights[0].intensity;
        intensityValue = startingIntensityValue;
        targetIntensityValue = startingIntensityValue - 4;
	}
	
	// Update is called once per frame
	void Update () {
        if(intensityValue >= startingIntensityValue - 0.02f) { up = false; }
        else if (intensityValue <= targetIntensityValue + 0.02f) { up = true; }
        if (!up) { intensityValue = Mathf.Lerp(intensityValue, targetIntensityValue, Time.deltaTime); }
        else { intensityValue = Mathf.Lerp(intensityValue, startingIntensityValue, Time.deltaTime); }
        foreach(Light light in pointLights)
        {
            light.intensity = intensityValue;
        }
	}
}
