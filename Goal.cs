using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    // placeholder
    public Text scoreText;

    public AudioClip[] scoreClips = new AudioClip[6];
    public Sprite[] scorePlusEffectSprites = new Sprite[6];
    private int[] scoreValues = new int[]{50, 100, 250, 50, 10, 10}; // rotten to fresh
    private float[] scoreParticleIntensities = new float[]{2f, 3f, 5f, 2f, 2f, 1.5f}; // rotten to fresh

    // related objects
    public Player player;
    
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
	Settings.DistanceTravelled = 0;
	Settings.PumpkinsSmashedOnBarriers = 0;
	Settings.PlayerDidAnything = false;
	Settings.EnemiesKilled = 0;
	Settings.Score = 0;
	Settings.PumpkinsScored = 0;
	this.scoreText.text = "$" + Settings.Score;
    }

    public void Update() {
	this.scoreText.text = "$" + Settings.Score;
    }

    public void OnTriggerEnter2D(Collider2D collider) {
	Throwable throwable = collider.gameObject.GetComponent<Throwable>();
	if (throwable != null) {
	    this.ScoreThrowable(throwable);
	}
    }

    private void ScoreThrowable(Throwable throwable) {
	int scoreBucket = throwable.GetScoreBucket();
	Settings.Score += this.scoreValues[scoreBucket];
	Settings.PumpkinsScored += 1;

	// sound
	GameObject soundEffectGo = new GameObject();	    
	SoundEffect soundEffect = soundEffectGo.AddComponent<SoundEffect>();
	soundEffect.clip = this.scoreClips[scoreBucket];
	soundEffect.distance = Vector3.Distance(this.transform.position, player.transform.position);

	
	// score plus effect
	GameObject scorePlusGo = new GameObject();
	scorePlusGo.AddComponent<SpriteRenderer>().sprite = this.scorePlusEffectSprites[scoreBucket];
	scorePlusGo.AddComponent<ScorePlusText>();
	scorePlusGo.transform.position = this.transform.position;
		
	throwable.DestroyPumpkin(false); // break but don't explode	
    }
}
