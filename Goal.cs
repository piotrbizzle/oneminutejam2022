using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    // placeholder
    public Text scoreText;
    private int score;
    
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
	this.score += throwable.GetScoreValue();
	Settings.Score = this.score;
	this.scoreText.text = "$" + this.score;

	throwable.DestroyPumpkin();
	return;
    }
}
