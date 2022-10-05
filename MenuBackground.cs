using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackground : MonoBehaviour
{
    private float closeEnough = 0.1f;
    private float maxSize = 1.5f;
    private bool isGrowing = true;
    
    // Update is called once per frame
    void Update()
    {
	float currentSize = this.transform.localScale.x;
	if (this.isGrowing && currentSize > this.maxSize - this.closeEnough) {
	    this.isGrowing = false;
	} else if (!this.isGrowing && currentSize < 1f + this.closeEnough) {
	    this.isGrowing = true;
	}
	
	float targetSize = this.isGrowing ? this.maxSize : 1f;
	float nextSize = (currentSize + targetSize) / 2;
	float sizeStep = currentSize + (nextSize - currentSize) * Time.deltaTime;
	
	this.transform.localScale = new Vector3(sizeStep, sizeStep, 1f);
    }
}
