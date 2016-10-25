using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class EditorTest : MonoBehaviour {

    public List<GameObject> pickups = new List<GameObject>();

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        pickups = GameObject.FindGameObjectsWithTag("Pickup").ToList();
    }
}
