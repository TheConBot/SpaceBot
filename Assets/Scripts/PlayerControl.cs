using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{

    public float playerSpeed;
    public float jumpSpeed;

    private Rigidbody2D rigidBody;
    private float xInput;
    private bool doJump;
    private int batteriesCollected;
    private float health = 1;
    private bool inGravityPadTrigger = false;
    private bool inDoorSwitchTrigger = false;
    private bool gravitySwap = false;
    private Collider2D coll2D;
    private float curveLerp = 0;
    private int direction;
    private DoorSwitch doorSwitch = null;

    public Text txt_collection;
    public Image img_health_fg;
    public float healthSubtractModifier = 0.1f;
    public AnimationCurve walkingAccel;
    // Use this for initialization
    void Start()
    {
        coll2D = GetComponent<Collider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        UpdateCollectedText();
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, 1);
        health -= Time.deltaTime * healthSubtractModifier;
        img_health_fg.fillAmount = health;

        InputDevice Controller = InputManager.ActiveDevice;
        xInput = Controller.DPadX.Value;
        if(xInput == -1)
        {
            direction = -1;
        }
        else if(xInput == 1)
        {
            direction = 1;
        }
        curveLerp = Mathf.Lerp(curveLerp, Mathf.Abs(xInput), Time.deltaTime * playerSpeed);
        float curveValue = walkingAccel.Evaluate(curveLerp);
        //Debug.LogFormat("XInput: {2}, Lerp: {0}, Curve Value: {1}", curveLerp, curveValue, xInput);

        bool isOnGround = OnGround();
        float jumpForce = 0;
        if (!isOnGround)
        {
            coll2D.sharedMaterial.friction = 0;
        }
        else
        {
            coll2D.sharedMaterial.friction = 0.4f;
        }
        if (Controller.Action1.WasPressed && isOnGround)
        {
            if (gravitySwap)
            {
                jumpForce = -jumpSpeed;
            }
            else
            {
                jumpForce = jumpSpeed;
            }

        }
        if (Controller.Action4.WasPressed && inGravityPadTrigger)
        {
            if (rigidBody.gravityScale == 1)
            {
                rigidBody.gravityScale = -1;
                gravitySwap = true;
            }
            else
            {
                rigidBody.gravityScale = 1;
                gravitySwap = false;
            }
        }
        else if(Controller.Action4.WasPressed && inDoorSwitchTrigger)
        {
            doorSwitch.triggered = true;
        }
        rigidBody.velocity = new Vector3(direction * (playerSpeed * curveValue), rigidBody.velocity.y + jumpForce);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Electric":
                health -= 0.0075f;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Pickup":
                other.gameObject.SetActive(false);
                batteriesCollected++;
                UpdateCollectedText();
                health += 0.25f;
                break;
            case "GravityPad":
                Debug.Log("Press Y to toggle gravity");
                inGravityPadTrigger = true;
                break;
            case "DoorSwitch":
                Debug.Log("Press Y to open door");
                inDoorSwitchTrigger = true;
                doorSwitch = other.GetComponent<DoorSwitch>();
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "GravityPad":
                Debug.Log("No longer in gravity sphere");
                inGravityPadTrigger = false;
                break;
            case "DoorSwitch":
                inDoorSwitchTrigger = false;
                break;
        }
    }

    private void UpdateCollectedText()
    {
        txt_collection.text = batteriesCollected.ToString();
    }

    private bool OnGround()
    {
        coll2D.enabled = false;
        Vector2 direction = Vector2.down;
        if (gravitySwap)
        {
            direction = Vector2.up;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f);
        coll2D.enabled = true;
        return hit;
    }

}
