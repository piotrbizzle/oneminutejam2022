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

    private float secondsRemaining;       
    private bool started;

    public void Start()
    {
	SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
	Color color = sr.color;
	sr.color = new Color(color.r, color.g, color.b, 0f);

	this.secondsRemaining = this.secondsToBlack;
    }
    
    public void Update() {
	if (!this.started) {
	    return;
	}
       
	SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
	Color color = sr.color;
	float alpha = 1f - (this.secondsRemaining / this.secondsToBlack);
	sr.color = new Color(color.r, color.g, color.b, alpha);

	this.secondsRemaining -= Time.deltaTime;
	
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
