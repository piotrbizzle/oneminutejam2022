using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndText : MonoBehaviour
{
    void Start() {
        this.GetComponent<Text>().text = "You harvested " + Settings.PumpkinsScored + " pumpkins worth $" + Settings.Score + " altogether on seed '" + Settings.SeedString + "'";
    }
}
