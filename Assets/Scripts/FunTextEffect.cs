using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FunTextEffect : MonoBehaviour {

    private Color newC;
    public Outline outline;

    void Start()
    {
        newC = Color.black;
        StartCoroutine("BackNForth");
    }

	void Update () {
        outline.effectColor = newC;
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
