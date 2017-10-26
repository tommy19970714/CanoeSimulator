using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DecisionInstructor : NetworkBehaviour {

    public Material instructorMaterial;
    public Material whiteMaterial;
    public Color instructorColor;

	// Use this for initialization
	void Start () {
		if(!isLocalPlayer)
        {
            GameObject right = transform.Find("Right").gameObject;
            foreach (Transform n in right.transform) GameObject.Destroy(n.gameObject);
            
            GameObject left = transform.Find("Left").gameObject;
            foreach (Transform n in left.transform) GameObject.Destroy(n.gameObject);
            
            GameObject cylinder = transform.Find("Cylinder").gameObject;
            foreach (Transform n in cylinder.transform) GameObject.Destroy(n.gameObject);

            InstructorColor();
        }
        else
        {
            White();
        }   
	}

    public void InstructorColor()
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.material = instructorMaterial;
        }
    }

    public void White()
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.material = whiteMaterial;
        }
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        base.OnDeserialize(reader, initialState);
    }

}
