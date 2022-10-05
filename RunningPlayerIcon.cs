using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunningPlayerIcon : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[2];
    public float walkAnimationDelay = 0.1f;
    public float currentWalkAnimationDelay = 0;
    public int currentWalkAnimationFrame = 1;
    
    void Update()
    {
	if (this.currentWalkAnimationDelay <= 0) {
	    this.currentWalkAnimationDelay = this.walkAnimationDelay;
	    // this only works for exactly 2 frames
	    this.currentWalkAnimationFrame = (this.currentWalkAnimationFrame + 1) % 2;
	} else {
	    this.currentWalkAnimationDelay -= Time.deltaTime;
	}
	this.GetComponent<Image>().sprite = this.sprites[this.currentWalkAnimationFrame];    
    }
}
