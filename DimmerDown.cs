using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DimmerDown : MonoBehaviour
{
    public float secondsToBlack = 1.0f;
    public string nextScene; 
    
    // related game objects
    public GameObject eventSystemGo;

    // this also plays the music for some reason haha
    public AudioClip clip;
    
    private float secondsRemaining;       
    private bool started;

    public void Start()
    {
	// rendering
	SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
	Color color = sr.color;
	sr.color = new Color(color.r, color.g, color.b, 0f);

	// music
	AudioSource audioSource = this.gameObject.AddComponent<AudioSource>();
	audioSource.clip = this.clip;
	audioSource.loop = true;
	audioSource.Play();
	
	this.secondsRemaining = this.secondsToBlack;
    }
    
    public void Update() {
	if (!this.started) {
	    return;
	}
	// rendering
	SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
	Color color = sr.color;
	float alpha = 1f - (this.secondsRemaining / this.secondsToBlack);
	sr.color = new Color(color.r, color.g, color.b, alpha);

	// music
	this.GetComponent<AudioSource>().volume = this.secondsRemaining / this.secondsToBlack;
	
	this.secondsRemaining -= Time.deltaTime;
	
	// change scene
	if (this.secondsRemaining < 0) {
	    // turn off event system from old scene
	    GameObject.Destroy(this.eventSystemGo);
	    SceneManager.LoadScene(this.nextScene);
	    return;
	}
    }

    public void StartDim() {
	this.started = true;
    }
}
