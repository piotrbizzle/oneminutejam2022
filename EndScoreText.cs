using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScoreText : MonoBehaviour
{
    void Start()
    {
	this.GetComponent<Text>().text = "Total Score: $" + Settings.Score + "\nPumpkins Collected: " + Settings.PumpkinsScored + "\nEnemies Destroyed: " + Settings.EnemiesKilled + "\nSeed: '" + Settings.SeedString + "'";
    }
}
