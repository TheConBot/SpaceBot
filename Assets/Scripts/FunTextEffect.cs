using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;
using UnityEngine.SceneManagement;

public class FunTextEffect : MonoBehaviour {

    private Color newC;
    public Outline outline;
    public Image fade;

    private bool startFade;

    void Start()
    {
        newC = Color.black;
        StartCoroutine("BackNForth");
    }

	void Update () {
        outline.effectColor = newC;
        if (InputManager.ActiveDevice.AnyButtonWasPressed && !startFade)
        {
            startFade = true;
            GetComponent<AudioSource>().Play();
        }
        if (startFade)
        {
            Color fadeColor = fade.color;
            fadeColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, fadeColor.a + (Time.deltaTime * 0.5f));
            fade.color = fadeColor;
            Camera.main.GetComponent<AudioSource>().volume -= Time.deltaTime * 0.5f;
            if (fade.color.a > 1)
            {
                SceneManager.LoadScene("Level");
            }
        }
	}

    IEnumerator BackNForth()
    {
        while (true)
        {
            while(!isApproximate(newC.r, Color.red.r, 0.02f))
            {
                newC = Color.Lerp(newC, Color.red, Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            while(!isApproximate(newC.r, Color.black.r, 0.02f))
            {
                newC = Color.Lerp(newC, Color.black, Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    bool isApproximate(float a, float b, float tolerance)
    {
        return Mathf.Abs(a - b) < tolerance;
    }
}
