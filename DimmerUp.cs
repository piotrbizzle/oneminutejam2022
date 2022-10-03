using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimmerUp : MonoBehaviour
{
    public float secondsToClear = 1.0f;
    private float secondsRemaining;
    
    // Start is called before the first frame update
    void Start()
    {
	SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
	Color color = sr.color;
	sr.color = new Color(color.r, color.g, color.b, 1f);

	this.secondsRemaining = this.secondsToClear;
    }

    // Update is called once per frame
    void Update()
    {	
	SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
	Color color = sr.color;
	float alpha = this.secondsRemaining / this.secondsToClear;
	sr.color = new Color(color.r, color.g, color.b, alpha);

	this.secondsRemaining -= Time.deltaTime;
	
	if (this.secondsRemaining < 0) {
	    GameObject.Destroy(this.gameObject);
	}
    }
}
