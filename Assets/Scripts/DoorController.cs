using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {

    public AnimationCurve animCurve;
    public DoorSwitch doorSwitch;
    private float doorLerp;
    private float origonalDoorY;

    void Start()
    {
        origonalDoorY = transform.position.y;
    }

    void Update()
    {
        if (doorSwitch.triggered)
        {
            doorLerp = Mathf.Lerp(doorLerp, 1, Time.deltaTime * 0.25f);
            transform.position = new Vector2(transform.position.x, origonalDoorY + (animCurve.Evaluate(doorLerp) * 10));
        }
    }
}
