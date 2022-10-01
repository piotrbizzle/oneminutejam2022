using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {
    public GameObject eventSystemGo;
    public Text seedInput;

    
    public void Start() {
	Button button = this.GetComponent<Button>();
	button.onClick.RemoveAllListeners();
	button.onClick.AddListener(this.LoadGameScene);
    }
    
    private void LoadGameScene() {
	// turn off event system from old scene
	GameObject.Destroy(this.eventSystemGo);
	SceneManager.LoadScene("GameplayScene");
    }
}
