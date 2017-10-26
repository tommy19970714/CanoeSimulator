using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour {

    // Use this for initialization
    void Start () {
        StartCoroutine("WaitInit");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play(float value)
    {
        float volume = value / 20.0f;
        if (volume < 0) volume = 0;
        else if (volume > 1) volume = 1;
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume * volume;
        Debug.Log(audioSource.volume);
        audioSource.Play();

        EllipsoidParticleEmitter emitter = GetComponent<EllipsoidParticleEmitter>();
        emitter.Emit((int)value * 10);

        
    }

    IEnumerator WaitInit()
    {
        yield return new WaitForSeconds(1.2f);
        Destroy(this.gameObject);
    }
}
