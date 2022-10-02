using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {
    public Text seedInput;
    public DimmerDown dimmer;
    
    public void Start() {
	Button button = this.GetComponent<Button>();
	button.onClick.RemoveAllListeners();
	button.onClick.AddListener(this.LoadGameScene);
    }
    
    private void LoadGameScene() {
	if (this.seedInput == null) {
	    // clear seed if this is a 'restart random game' button
	    Settings.SeedString = "";
	} else {
	    // save seed if supplied
	    Settings.SeedString = this.seedInput.text;
	}
	
	// activate dimmer
	this.dimmer.StartDim();
    }
}
