using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScoreText : MonoBehaviour
{
    void Start()
    {
	this.GetComponent<Text>().text = "Total Score: $" + Settings.Score + "\nPumpkins Collected: " + Settings.PumpkinsScored + "\nPumpkin Ghosts Destroyed: " + Settings.EnemiesKilled + "\nPumpkins Splatted on Fences: " + Settings.PumpkinsSmashedOnBarriers + "\nMeters Travelled: " + Math.Round(Settings.DistanceTravelled * 5f) + "\nSeed: '" + Settings.SeedString + "'";
    }
}
