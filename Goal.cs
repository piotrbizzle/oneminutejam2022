using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    // related prefabs
    public ParticleSystem particles;
    
    // placeholder
    public Text scoreText;
    private int score;
    private int pumpkinsScored;

    public Sprite[] scorePlusEffectSprites = new Sprite[6];
    private int[] scoreValues = new int[]{50, 100, 250, 50, 50, 10}; // rotten to fresh
    private float[] scoreParticleIntensities = new float[]{2f, 3f, 5f, 2f, 2f, 1.5f}; // rotten to fresh
    
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
	Settings.Score = 0;
	Settings.PumpkinsScored = 0;
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
	this.pumpkinsScored += 1;
	Settings.Score = this.score;
	Settings.PumpkinsScored = this.pumpkinsScored;
	this.scoreText.text = "$" + this.score;

	// score plus effect
	GameObject scorePlusGo = new GameObject();
	scorePlusGo.AddComponent<SpriteRenderer>().sprite = this.scorePlusEffectSprites[scoreBucket];
	scorePlusGo.AddComponent<ScorePlusText>();
	scorePlusGo.transform.position = this.transform.position;
	
	// particles
	/*
	ParticleSystem particles = Instantiate(this.particles) as ParticleSystem;	
	particles.transform.position = this.transform.position;	    
	*/
	// this has to be done in two lines for arcane reasons
	var main = particles.main;
	main.startLifetime = 0.1f * this.scoreParticleIntensities[scoreBucket];
	main.startSpeed = this.scoreParticleIntensities[scoreBucket];
	main.startColor = throwable.GetComponent<SpriteRenderer>().color;
	
	throwable.DestroyPumpkin(false); // break but don't explode	
    }
}
