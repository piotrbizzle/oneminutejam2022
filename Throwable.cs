using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    // configurables
    private float Drag = 10.0f;
    private float MomentumToBreakRatio = 0.6f;
    private float BaseScoreValue = 100.0f;
    private float SecondsToRot = 30;

    private float currentRot;
    private float initialMomentum;
    private float momentum;
    
    public void Start() {
 	// collision
        this.gameObject.AddComponent<BoxCollider2D>();
	this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

	// rendering
	this.GetComponent<SpriteRenderer>().sortingLayerName = "Scenery";

	// rot
	this.currentRot = this.SecondsToRot;
    }

    public void Update() {
	// rot
	this.currentRot -= Time.deltaTime;
	if (this.currentRot < 0) {
	    this.RotAway();
	}

	// placeholder
	float shade = this.currentRot / this.SecondsToRot;
	this.GetComponent<SpriteRenderer>().color = new Color(shade, shade, shade, 1.0f);

	// check if object thrown
	if (this.initialMomentum == 0) {
	    return;
	}

	// move object
	this.transform.Translate(Vector3.up * this.momentum * Time.deltaTime);

	// slow down object
	this.momentum -= Time.deltaTime * this.Drag;

	// break once the object slows down enough
	if (this.momentum < this.initialMomentum * this.MomentumToBreakRatio) {
	    this.momentum = 0;
	    this.initialMomentum = 0;
	}
    }

    public void Throw(float initialMomentum) {
	this.initialMomentum = initialMomentum;
	this.momentum = initialMomentum;
    }

    public int GetScoreValue() {
	return (int)(this.currentRot / this.SecondsToRot * this.BaseScoreValue);
    }
    
    private void RotAway() {
	GameObject.Destroy(this.gameObject);
    }
}
