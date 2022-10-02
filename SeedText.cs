using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedText : MonoBehaviour
{
    void Start() {
	this.GetComponent<InputField>().text = Settings.SeedString;
    }
}
