using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanningCamera : MonoBehaviour
{
    // related objects
    public Player player;

    public float MaximumPan = 0.25f;
    
    void Update()
    {
	float x = player.transform.position.x / 5.0f * this.MaximumPan;
	float y = player.transform.position.y / 5.0f * this.MaximumPan;
        this.transform.position = new Vector3(x, y, this.transform.position.z);
    }
}
