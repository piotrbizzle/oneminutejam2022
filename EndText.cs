using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndText : MonoBehaviour
{
    void Start() {
        this.GetComponent<Text>().text = "You got $" + Settings.Score + " points on seed " + Settings.SeedString;
    }
}
