using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DecisionInstructor : NetworkBehaviour {

    public Material instructorMaterial;
    public Color instructorColor;

	// Use this for initialization
	void Start () {
		if(!isLocalPlayer)
        {
            //GetComponent<Renderer>().material = instructorMaterial;
            GameObject right = transform.Find("Right").gameObject;
            foreach (Transform n in right.transform) GameObject.Destroy(n.gameObject);

            //right.GetComponent<Renderer>().material = instructorMaterial;
            GameObject left = transform.Find("Left").gameObject;
            foreach (Transform n in left.transform) GameObject.Destroy(n.gameObject);

            //left.GetComponent<Renderer>().material = instructorMaterial;
            GameObject cylinder = transform.Find("Cylinder").gameObject;
            foreach (Transform n in cylinder.transform) GameObject.Destroy(n.gameObject);
            //cylinder.GetComponent<Renderer>().material = instructorMaterial;

            ColorChange();
        }
	}

    public void ColorChange()
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.material = instructorMaterial;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
