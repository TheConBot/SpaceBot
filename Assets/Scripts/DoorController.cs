using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {

    public AnimationCurve animCurve;
    public DoorSwitch doorSwitch;
    private float doorLerp;
    private float origonalDoorY;
    public float distanceUp = 10;
    private bool playOnce = true;

    private AudioSource doorSound;

    void Start()
    {
        origonalDoorY = transform.position.y;
        doorSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (doorSwitch.triggered)
        {
            doorLerp = Mathf.Lerp(doorLerp, 1, Time.deltaTime * 0.25f);
            transform.position = new Vector2(transform.position.x, origonalDoorY + (animCurve.Evaluate(doorLerp) * distanceUp));
            if (playOnce)
            {
                doorSound.Play();
                playOnce = false;
            }
        }
    }
}
