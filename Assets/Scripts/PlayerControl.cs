using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private int direction = 1;
    private DoorSwitch doorSwitch = null;
    private Animator anim;
    private Vector3 camOrigin;
    private float camOrthoSizeOrigin;
    private bool leftTerminal;
    private bool inGenerator;
    private bool enableInput = true;
    private bool pullBackCamera = false;
    private bool win;

    public Text txt_collection;
    public Image img_health_fg;
    public float healthSubtractModifier = 0.1f;
    public AnimationCurve walkingAccel;
    public float rayDistance = 1;
    public GameObject genCatWalk;
    public GameObject[] finalSwitches;
    public Light lastLight;
    public GameObject[] electricArcs;
    public Image fade;
    public Text txt;
    public Image YButton;
    public AudioSource beep;
    // Use this for initialization
    void Start()
    {
        YButton.enabled = false;
        camOrigin = Camera.main.transform.localPosition;
        camOrthoSizeOrigin =  Camera.main.orthographicSize;
        anim = GetComponent<Animator>();
        coll2D = GetComponent<Collider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        UpdateCollectedText();
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, 1);
        img_health_fg.fillAmount = health;
        if (health == 0)
        {
            Debug.Log(fade.color.a);
            enableInput = false;
            rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            coll2D.enabled = false;
            Color fadeColor = fade.color;
            fadeColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, fadeColor.a + (Time.deltaTime * 0.5f));
            fade.color = fadeColor;
            if (fade.color.a > 1)
            {
                txt.text = "You died.\n\nPress any button to continue.";
                if (InputManager.ActiveDevice.AnyButtonWasPressed)
                {
                    SceneManager.LoadScene("Menu");
                }
            }
        }
        else if (win)
        {
            health += 0.025f;
            Color fadeColor = fade.color;
            fadeColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, fadeColor.a + (Time.deltaTime * 0.5f));
            fade.color = fadeColor;
            Camera.main.GetComponent<AudioListener>().enabled = false;
            if (fade.color.a > 1)
            {
                txt.text = "You did it!\n\nYou collected " + batteriesCollected + " power-cells.\n\nPress any button to continue.";
                if (InputManager.ActiveDevice.AnyButtonWasPressed)
                {
                    SceneManager.LoadScene("Menu");
                }
            }
        }

        InputDevice Controller = null;
        float jumpForce = 0;
        float curveValue = 0;
        if (enableInput)
        {
            bool isOnGround = OnGround();
            health -= Time.deltaTime * healthSubtractModifier;
            Controller = InputManager.ActiveDevice;
            xInput = Controller.DPadX.Value;

            if (Mathf.Abs(xInput) == 1 && isOnGround)
            {
                anim.SetBool("isRunning", true);
            }
            else
            {
                anim.SetBool("isRunning", false);
            }
            if (xInput == -1)
            {
                direction = -1;
            }
            else if (xInput == 1)
            {
                direction = 1;
            }
            curveLerp = Mathf.Lerp(curveLerp, Mathf.Abs(xInput), Time.deltaTime * playerSpeed);
            curveValue = walkingAccel.Evaluate(curveLerp);
            //Debug.LogFormat("XInput: {2}, Lerp: {0}, Curve Value: {1}", curveLerp, curveValue, xInput);

            if (!isOnGround)
            {
                anim.SetBool("isJumping", true);
                coll2D.sharedMaterial.friction = 0;
            }
            else
            {
                anim.SetBool("isJumping", false);
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
                    coll2D.offset = new Vector2(coll2D.offset.x, 0.57f);
                }
                else
                {
                    rigidBody.gravityScale = 1;
                    gravitySwap = false;
                    coll2D.offset = new Vector2(coll2D.offset.x, -0.68f);
                }
                inGravityPadTrigger = false;
                YButton.enabled = false;
            }
            else if (Controller.Action4.WasPressed && inDoorSwitchTrigger)
            {
                doorSwitch.triggered = true;
            }
            if (leftTerminal)
            {
                Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, camOrigin, Time.deltaTime);
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, camOrthoSizeOrigin, Time.deltaTime);

            }
            if (inGenerator)
            {
                genCatWalk.SetActive(true);
                bool bothSwitchesTriggered = false;
                foreach (GameObject switch_ in finalSwitches)
                {
                    bothSwitchesTriggered = switch_.GetComponent<DoorSwitch>().triggered;
                }
                if (bothSwitchesTriggered && Controller.Action4.WasPressed)
                {
                    lastLight.color = Color.blue;
                    foreach (GameObject arc in electricArcs)
                    {
                        arc.SetActive(true);
                    }
                    win = true;
                    enableInput = false;
                }
            }
            if (pullBackCamera)
            {
                Debug.Log("Pulling camera back...");
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 16, Time.deltaTime);
            }
        }
        rigidBody.velocity = new Vector3(direction * (playerSpeed * curveValue), rigidBody.velocity.y + jumpForce);
        transform.FindChild("Model").gameObject.transform.localScale = new Vector3(transform.localScale.x, rigidBody.gravityScale, direction);
    }

    IEnumerator LeftTerminalCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        leftTerminal = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Electric":
                health -= 0.0025f;
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
                beep.Play();
                break;
            case "GravityPad":
                Debug.Log("Press Y to toggle gravity");
                inGravityPadTrigger = true;
                YButton.enabled = true;
                break;
            case "DoorSwitch":
                Debug.Log("Press Y to open door");
                inDoorSwitchTrigger = true;
                doorSwitch = other.GetComponent<DoorSwitch>();
                YButton.enabled = true;
                break;
            case "Terminal":
                Debug.Log("In Terminal");
                leftTerminal = false;
                Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(10.15f, 30, 7.71f), Time.deltaTime);
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 33, Time.deltaTime);
                break;
            case "Generator":
                inGenerator = true;
                YButton.enabled = true;
                break;
            case "bloop":
                Debug.Log("Hello");
                pullBackCamera = true;
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
                YButton.enabled = false;
                break;
            case "DoorSwitch":
                inDoorSwitchTrigger = false;
                YButton.enabled = false;
                break;
            case "Terminal":
                leftTerminal = true;
                StartCoroutine(LeftTerminalCooldown(5f));
                break;
            case "Generator":
                inGenerator = false;
                YButton.enabled = false;
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance);
        coll2D.enabled = true;
        return hit;
    }

}
