using UnityEngine;
using System.Collections;
using InControl;

public class PlayerControl : MonoBehaviour {

    public float playerSpeed;
    public float jumpSpeed;

    private Rigidbody rigidBody;
    private float xInput;
    private bool doJump;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        InputDevice Controller = InputManager.ActiveDevice;
        xInput = Controller.Direction.X;
        if (Controller.Action1.WasPressed)
        {
            doJump = true;
        }
        else
        {
            doJump = false;
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
        rigidBody.velocity = new Vector3(xInput * playerSpeed, rigidBody.velocity.y + jumpForce, rigidBody.velocity.z);
    }
}
