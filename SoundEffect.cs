using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioClip clip;
    public float distance;
    public float maxDistance = 4.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        AudioSource audioSource = this.gameObject.AddComponent<AudioSource>();
	audioSource.clip = this.clip;
	audioSource.volume = Math.Max(0, (this.maxDistance - this.distance) / this.maxDistance);
	audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
	AudioSource audioSource = this.gameObject.GetComponent<AudioSource>();
	if (!audioSource.isPlaying) {
	    GameObject.Destroy(this.gameObject);
	}
	
    }
}
