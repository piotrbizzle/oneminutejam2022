using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndText : MonoBehaviour
{
    
    public Text endingNumberText;

    public Image ribbon;
    public Image[] unlockRibbons = new Image[10];
    public Sprite[] dullRibbonSprites = new Sprite[10];
    public Sprite[] ribbonSprites = new Sprite[10];
    
    void Start() {	
	// f tier always unlocks
	this.ribbon.GetComponent<Image>().sprite = this.ribbonSprites[0];
	this.GetComponent<Text>().text ="You survived the Pumpkin Ghosts, but you only brought in a meager harvest.\n~\nYou'll have to scrimp and save to make the money last until next year...";
	this.endingNumberText.text = "- Ending 1 of 10 -";
	Settings.RibbonsUnlocked[0] = true;

	// c tier
	if (Settings.Score >= 1000) {
	    this.ribbon.GetComponent<Image>().sprite = this.ribbonSprites[1];
	    this.GetComponent<Text>().text ="You've contended with the Pumpkin Ghosts and collected a modest harvest!\n~\nYou have enough money for the next year, but you'll need to tighten your belt a little...";
	    this.endingNumberText.text = "- Ending 2 of 10 -";
	    Settings.RibbonsUnlocked[1] = true;
	}	

	// b tier
	if (Settings.Score >= 2000) {	
	    this.ribbon.GetComponent<Image>().sprite = this.ribbonSprites[2];
	    this.GetComponent<Text>().text = "You've outwitted the Pumpkin Ghosts and collected a respectable harvest!\n~\nYou'll be comfortable for the next year, but you didn't bring in enough money to retire...";
	    this.endingNumberText.text = "- Ending 3 of 10 -";
	    Settings.RibbonsUnlocked[2] = true;
	}
	
	// a tier
	if (Settings.Score >= 3000) {	
	    this.ribbon.GetComponent<Image>().sprite = this.ribbonSprites[3];
	    this.GetComponent<Text>().text = "You've bested the Pumpkin Ghosts and collected a glorious harvest!\n~\nWith this kind of money, you can sell your haunted pumpkin farm and retire!";
	    this.endingNumberText.text = "- Ending 4 of 10 -";
	    Settings.RibbonsUnlocked[3] = true;
	}
	
	// do nothing ending
	if (!Settings.PlayerDidAnything) {
	    this.ribbon.GetComponent<Image>().sprite = this.ribbonSprites[9];
	    this.GetComponent<Text>().text = "It's a game about ghosts, but you're not supposed to ghost the game!\n~\nYour failure to act has doomed the farm on a financial and mystical level.";
	    this.endingNumberText.text = "- Ending 10 of 10 -";
	    Settings.RibbonsUnlocked[9] = true;
	}

	// kilometer ending
	if (Settings.DistanceTravelled > 200) {	
	    this.ribbon.GetComponent<Image>().sprite = this.ribbonSprites[8];
	    this.GetComponent<Text>().text = "Okay then! You ran a whole kilometer for some reason!\n~\nThe farm is still haunted, but at least you got some good cardio in!";
	    this.endingNumberText.text = "- Ending 9 of 10 -";
	    Settings.RibbonsUnlocked[8] = true;
	}

	// pumpkin smasher ending
	if (Settings.PumpkinsSmashedOnBarriers > 30) {
	    this.ribbon.GetComponent<Image>().sprite = this.ribbonSprites[7];
	    this.GetComponent<Text>().text = "Why did you smash so many pumpkins? You were supposed to harvest them!\n~\nWhat, do you think you're in a band or something?";
	    this.endingNumberText.text = "- Ending 8 of 10 -";
	    Settings.RibbonsUnlocked[7] = true;
	}

	// pumpkin getter ending
	if (Settings.PumpkinsScored >= 24) {
	    this.ribbon.GetComponent<Image>().sprite = this.ribbonSprites[6];
	    this.GetComponent<Text>().text = "Great work, you gathered an absolute butt-load of pumpkins!\n~\nNot all of them were ripe, but sometimes quantity is better than quality.";
	    this.endingNumberText.text = "- Ending 7 of 10 -";
	    Settings.RibbonsUnlocked[6] = true;
	}

	// ghost killer ending
	if (Settings.EnemiesKilled >= 20) {
	    this.ribbon.GetComponent<Image>().sprite = this.ribbonSprites[5];
	    this.GetComponent<Text>().text = "You exterminated the Pumpkin Ghost infestation with your pumpkin-flinging skillz!\n~\nAlthough you didn't bring in much money this year, at least the farm is no longer cursed.";
	    this.endingNumberText.text = "- Ending 6 of 10 -";
	    Settings.RibbonsUnlocked[5] = true;
	}

	// s tier
	if (Settings.Score >= 4000) {
	    this.ribbon.GetComponent<Image>().sprite = this.ribbonSprites[4];
	    this.GetComponent<Text>().text = "How did you get this many points? That's actually ridiculous!\n~\nYou use your crazy profits to buy a yacht, but it's also haunted by Pumpkin Ghosts.";
	    this.endingNumberText.text = "- Ending 5 of 10 -";
	    Settings.RibbonsUnlocked[4] = true;
	}       

	// unlock ribbon icons
	for (int i = 0; i < 10; i++) {
	    unlockRibbons[i].sprite = Settings.RibbonsUnlocked[i] ? ribbonSprites[i] : dullRibbonSprites[i];
	}
    }
}
