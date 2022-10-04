using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ribbon : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[5];
    
    void Start()
    {
 	if (Settings.Score >= 4000) {
	    this.GetComponent<Image>().sprite = this.sprites[4];
	} else if (Settings.Score >= 3000) {
	    this.GetComponent<Image>().sprite = this.sprites[3];
	} else if (Settings.Score >= 2000) {
	    this.GetComponent<Image>().sprite = this.sprites[2];
	} else if (Settings.Score >= 1000) {
	    this.GetComponent<Image>().sprite = this.sprites[1];
	} else {
	    this.GetComponent<Image>().sprite = this.sprites[0];
	}       
    }
}
