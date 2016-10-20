using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {

    public float playerSpeed;
    public float jumpSpeed;

    private Rigidbody2D rigidBody;
    private float xInput;
    private bool doJump;
    private int batteriesCollected;
    private float health = 1;
    private bool inSwitch = false;

    public Text txt_collection;
    public Image img_health_fg;
    public float healthSubtractModifier = 0.1f;
	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        UpdateCollectedText();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(health);
        Mathf.Clamp(health, 0, 1);
        health -= Time.deltaTime * healthSubtractModifier;
        img_health_fg.fillAmount = health;

        InputDevice Controller = InputManager.ActiveDevice;
        xInput = Controller.Direction.X;
        bool isOnGround = OnGround();
        Debug.Log(isOnGround);
        if (Controller.Action1.WasPressed && isOnGround)
        {
            doJump = true;
        }
        else
        {
            doJump = false;
        }

        if(Controller.Action4.WasPressed && inSwitch)
        {
            if (rigidBody.gravityScale == 1)
            {
                rigidBody.gravityScale = -1;
            }
            else
            {
                rigidBody.gravityScale = 1;
            }
        }
	}

    void FixedUpdate()
    {
        float jumpForce;
        if (doJump)
        {
            jumpForce = jumpSpeed;
        }
        else
        {
            jumpForce = 0;
        }
        rigidBody.velocity = new Vector3(xInput * playerSpeed, rigidBody.velocity.y + jumpForce);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Pickup":
                other.gameObject.SetActive(false);
                other.gameObject.transform.parent.gameObject.SetActive(false);
                batteriesCollected++;
                UpdateCollectedText();
                health += 0.1f;
                break;
            case "Switch":
                Debug.Log("Press Y to toggle gravity");
                inSwitch = true;
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Switch":
                Debug.Log("No longer in gravity sphere");
                inSwitch = false;
                break;
        }
    }

    private void UpdateCollectedText()
    {
        txt_collection.text = batteriesCollected.ToString();
    }

    private bool OnGround()
    {
        Collider2D coll2D = gameObject.GetComponent<Collider2D>();
        coll2D.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1);
        coll2D.enabled = true;
        return hit;
    }
}
