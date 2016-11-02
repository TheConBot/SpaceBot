using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartUI : MonoBehaviour {

    public Image fade;
    public Text txt;
    [Multiline]
    public string[] lines;
    // Use this for initialization
    private bool isDone;

	void Start () {
        StartCoroutine("DisplayStrings");
	}
	
	// Update is called once per frame
	void Update () {
        if (isDone)
        {
            Color fadeColor = fade.color;
            fadeColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, fadeColor.a - (Time.deltaTime * 0.5f));
            fade.color = fadeColor;
            if(fade.color.a < 0)
            {
                isDone = false;
            }
        }
	}

    IEnumerator DisplayStrings()
    {
        foreach(string item in lines)
        {
            txt.text = item;
            if(item == "")
            {
                break;
            }
            yield return new WaitForSeconds(5f);
        }
        isDone = true;
    }
}
