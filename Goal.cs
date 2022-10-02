using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    // related objects
    public Light goalLight;
    
    // placeholder
    public Text scoreText;
    private int score;

    private int[] scoreValues = new int[]{50, 100, 250, 100, 50, 10}; // rotten to fresh
    private float[] scoreLightIntensities = new float[]{2f, 3f, 5f, 3, 2f, 1.5f}; // rotten to fresh
    
    // Start is called before the first frame update
    public void Start() {
     	// collision
        this.gameObject.AddComponent<BoxCollider2D>();
	Rigidbody2D rb = this.gameObject.AddComponent<Rigidbody2D>();
	rb.gravityScale = 0.0f;
	rb.constraints = RigidbodyConstraints2D.FreezeAll;
	rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
	rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
	
	// rendering
	this.GetComponent<SpriteRenderer>().sortingLayerName = "Scenery";

	// score
	this.scoreText.text = "$" + this.score;
    }

    public void OnTriggerEnter2D(Collider2D collider) {
	Throwable throwable = collider.gameObject.GetComponent<Throwable>();
	if (throwable != null) {
	    this.ScoreThrowable(throwable);
	}
    }

    private void ScoreThrowable(Throwable throwable) {
	int scoreBucket = throwable.GetScoreBucket();
	this.score += this.scoreValues[scoreBucket];
	Settings.Score = this.score;
	this.scoreText.text = "$" + this.score;
	
	this.goalLight.intensity = this.scoreLightIntensities[scoreBucket];
	
	throwable.DestroyPumpkin(false); // break but don't explode	
    }
}
