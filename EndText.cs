using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndText : MonoBehaviour
{
    void Start() {
	if (Settings.Score >= 4000) {
	    this.GetComponent<Text>().text = "How did you get this many points? That's actually ridiculous!\n~\nYou use your crazy profits to buy a yacht, but it's also haunted by pumpkins ghosts.";
	} else if (Settings.Score >= 3000) {
	    this.GetComponent<Text>().text = "You've bested the Pumpkin Ghosts and collected a glorious harvest!\n~\nWith this kind of money, you can sell your haunted pumpkin farm and retire!";
	} else if (Settings.Score >= 2000) {
	    this.GetComponent<Text>().text = "You've outwitted the Pumpkin Ghosts and collected a respectable harvest!\n~\nYou'll be comfortable for the next year, but you didn't bring in enough money to retire...";
	} else if (Settings.Score >= 1000) {
	    this.GetComponent<Text>().text ="You've contended with the Pumpkin Ghosts and collected a modest harvest!\n~\nYou'll have enough money for the next year, but you'll have to tighten your belt a little...";
	} else {
	    this.GetComponent<Text>().text ="You survived the Pumpkin Ghosts, but you only brought in a meager harvest.\n~\nYou'll have to scrimp and save to make the money last until next year...";
	}		
    }
}
