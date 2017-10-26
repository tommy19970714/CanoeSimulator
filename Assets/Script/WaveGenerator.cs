using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour {

    // Use this for initialization
    void Start () {
        
}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play(float volume)
    {
        if (volume < 0) volume = 0;
        else if (volume > 1) volume = 1;
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume * volume;
        Debug.Log(audioSource.volume);
        audioSource.Play();
    }
}
