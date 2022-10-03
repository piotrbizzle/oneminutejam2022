using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePlusText : MonoBehaviour
{
    public float speed = 0.01f;
    public float timeToLive = 0.5f;
    public float upOffset = 0.2f;
    private float currentTimeToLive;

    
    public void Start() {
	this.currentTimeToLive = this.timeToLive;
	this.transform.Translate(Vector3.up * this.upOffset);
    }
   
    void Update() {
	this.currentTimeToLive -= Time.deltaTime;
	if (this.currentTimeToLive < 0) {
	    GameObject.Destroy(this.gameObject);
	}
	
	this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, this.currentTimeToLive / this.timeToLive);
	this.transform.Translate(Vector3.up * this.speed);
    }
}
