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
    private bool inSwitch = false;
    private bool gravitySwap = false;

    public Text txt_collection;
    public Image img_health_fg;
    public float healthSubtractModifier = 0.1f;
    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        UpdateCollectedText();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(health);
        health = Mathf.Clamp(health, 0, 1);
        health -= Time.deltaTime * healthSubtractModifier;
        img_health_fg.fillAmount = health;

        InputDevice Controller = InputManager.ActiveDevice;
        xInput = Controller.Direction.X;
        bool isOnGround = OnGround();
        float jumpForce = 0;
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
        if (Controller.Action4.WasPressed && inSwitch)
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
        rigidBody.velocity = new Vector3(xInput * playerSpeed, rigidBody.velocity.y + jumpForce);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Electric":
                health -= 0.001f;
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
