using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLight : MonoBehaviour
{
    public float fadeDownSpeed = 5f;
    
    // Update is called once per frame
    void Update() {
	Light light = this.GetComponent<Light>();
	if (light.intensity <= 1f) {
	    return;
	}
	light.intensity -= Time.deltaTime * this.fadeDownSpeed;	
	if (light.intensity < 1f) {
	    light.intensity = 1f;
	}
    }
}
