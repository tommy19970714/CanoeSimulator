using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimation3D : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        Color color = this.GetComponent<MeshRenderer>().material.color;
        color.a = 0;
        GetComponent<MeshRenderer>().material.color = color;
        this.transform.localPosition = new Vector3(-150f, 5.0f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Color color = this.GetComponent<MeshRenderer>().material.color;
        color.a -= 0.014f;
        GetComponent<MeshRenderer>().material.color = color;
        if(color.a < 0) {
            gameObject.SetActive(false);
        }

        Vector3 target = new Vector3(0, 0, 0);
        this.transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, 2);
    }

    public void StartAnimation(string text)
    {
        this.GetComponent<TextMesh>().text = text;
        gameObject.SetActive(true);

        this.transform.localPosition = new Vector3(-150f, 5.0f, 0);
        
        Color color = this.GetComponent<MeshRenderer>().material.color;
        color.a = 1.0f;
        GetComponent<MeshRenderer>().material.color = color;

        Debug.Log("animation start");
    }
}
