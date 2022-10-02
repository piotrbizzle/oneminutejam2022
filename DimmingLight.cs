using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimmingLight : MonoBehaviour
{
    // related objects
    public DimmerDown dimmer;
    private float timeRemaining = 59f;
    
    void Update()
    {
	if (Settings.PumpkinsLeft <= 0) {
	    this.dimmer.StartDim();
	}
	
	this.timeRemaining -= Time.deltaTime;
	if (this.timeRemaining < 0) {
	    this.timeRemaining = 0;
	    return;
	}
	
	Light light = this.GetComponent<Light>();

	float timeRatio = (60f - this.timeRemaining) / 60f;
	light.transform.localPosition = new Vector3(0, 0, -5 + timeRatio * 4.5f);
	light.range = 40f - timeRatio * 36f;
    }
}
