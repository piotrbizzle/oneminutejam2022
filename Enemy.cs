using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // configurables
    private float LungeSpeed = 3.0f;
    private float LungeCooldown = 1.0f;
    private float LungeAggroDistance = 1.0f;    
    private float LungeDuration = 0.3f;
    private float WalkSpeed = 1.0f;
    private float WalkAggroDistance = 4.0f;
    private float WanderSpeed = 0.35f;
    private float WanderDuration = 0.5f;
    private int ScoreValue = 10;
    
    // related objects
    public Player player;
    public Throwable heldThrowableChild;
    public Light possessedLight;

    // animation
    public float walkAnimationDelay = 0.2f;
    public float currentWalkAnimationDelay = 0;
    public int currentWalkAnimationFrame = 1;
    public Sprite scorePlusEffectSprite;

    public AudioClip clip;
    
    // movement
    private float currentLungeCooldown;
    private float currentLungeDuration;
    private Vector3 wanderVector;
    private float currentWanderDuration;

    public Sprite[] sprites = new Sprite[5];
    
    void Start() {
	// rendering
	this.gameObject.AddComponent<SpriteRenderer>().sprite = this.sprites[1];
	this.GetComponent<SpriteRenderer>().material = Settings.SpriteMaterial;

	// lighting
	Light possessedLightGo = Instantiate(this.possessedLight) as Light;
	possessedLightGo.gameObject.transform.parent = this.transform;
	possessedLightGo.gameObject.transform.localPosition = new Vector3(0, 0, -0.5f);
	
	// collision
        this.gameObject.AddComponent<BoxCollider2D>();

	Rigidbody2D rb = this.gameObject.AddComponent<Rigidbody2D>();
	rb.gravityScale = 0.0f;
	rb.constraints = RigidbodyConstraints2D.FreezeRotation;
	rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
	rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

	this.WalkSpeed += Random.Range(-5, 5) * 0.1f;
	this.LungeSpeed += Random.Range(-5, 5) * 0.1f;
	this.WanderSpeed += Random.Range(-5, 5) * 0.05f;
    }

    void Update() {
	// fix missing pumpkin. this will doubtless be a source of bugs later
	if (this.transform.childCount == 1) {
	    this.heldThrowableChild = null;
	    this.GetComponent<SpriteRenderer>().sprite = this.sprites[1];
	}
	
	// do nothing if lunge on cooldown
	if (this.currentLungeCooldown > 0) {
	    this.currentLungeCooldown -= Time.deltaTime;
	    return;
	}

	// movement
	Vector3 moveVector = new Vector3(0, 0, 0);
	Vector3 normalizedToPlayer = Vector3.Normalize(this.player.transform.position - this.transform.position);
	
	// invert all moves if holding a pumpkin
	float chaseOrFleeModifier = this.heldThrowableChild == null ? 1 : -1;	

	if (this.currentLungeDuration > 0) {
	    // move fast if mid-lunge
	    moveVector = normalizedToPlayer * this.LungeSpeed * Time.deltaTime * chaseOrFleeModifier;
	    this.currentLungeDuration -= Time.deltaTime;

	    // go to cooldown if lunge ends
	    if (this.currentLungeDuration <= 0) {
		this.currentLungeCooldown = this.LungeCooldown;
	    }
	} else {	     
	    float distanceToPlayer = Vector3.Distance(this.transform.position, this.player.transform.position);
	
	    if (distanceToPlayer < this.LungeAggroDistance) {
		// start a lunge
		this.currentLungeDuration = this.LungeDuration;
	    } else if (distanceToPlayer < this.WalkAggroDistance) {
		// move normal speed toward player
		moveVector = normalizedToPlayer * this.WalkSpeed * Time.deltaTime * chaseOrFleeModifier;
	    } else {
		// enemy wanders
		if (this.currentWanderDuration < 0) {
		    // choose new wander vector
		    this.wanderVector = Vector3.Normalize(new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0.0f));
		    this.currentWanderDuration = this.WanderDuration;		
		}

		moveVector = this.wanderVector * this.WalkSpeed * Time.deltaTime;
		this.currentWanderDuration -= Time.deltaTime;
	    }
	}
	this.transform.Translate(moveVector);

	// animation
	if (this.heldThrowableChild == null) {
	    // adds to selected animation frame based on facing
	    int facingAdder = moveVector.x < 0 ? 0 : 2;
	    if (this.currentWalkAnimationDelay <= 0) {
		this.currentWalkAnimationDelay = this.walkAnimationDelay;
		// this only works for exactly 2 frames
		this.currentWalkAnimationFrame = facingAdder + this.currentWalkAnimationFrame % 2 + 1;
	    } else {
		this.currentWalkAnimationDelay -= Time.deltaTime;
	    }
	    this.GetComponent<SpriteRenderer>().sprite = this.sprites[this.currentWalkAnimationFrame];
	} else {
	    this.GetComponent<SpriteRenderer>().sprite = this.sprites[0];
	}
    }

    public void OnTriggerEnter2D(Collider2D collider) {
	Throwable throwable = collider.gameObject.GetComponent<Throwable>();
	if (throwable == null) {
	    return;
	}
	
	if (throwable.momentum > 0) {
	    this.GetHitBy(throwable);
	    return;
	}
	
	// TODO: better way to tell held pumpkin
	if (this.heldThrowableChild == null && throwable.GetComponent<SpriteRenderer>().sortingLayerName == "Held" && throwable.transform.parent.parent == this.player.transform && throwable.IsStealable()) {
	    this.StealThrowable(throwable);
	    return;
	}
    }    

    private void StealThrowable(Throwable throwable) {
	// sound
	GameObject soundEffectGo = new GameObject();	    
	SoundEffect soundEffect = soundEffectGo.AddComponent<SoundEffect>();
	soundEffect.clip = this.clip;
	soundEffect.distance = Vector3.Distance(this.transform.position, player.transform.position);

	// swap ownership of throwable
	throwable.GetStolen();
	this.heldThrowableChild = throwable;
	this.player.heldThrowableChild = null;
	throwable.gameObject.transform.parent = this.transform;

	// center
	throwable.gameObject.transform.localPosition = new Vector3(0, 0, 0);

	// disappear
	this.GetComponent<SpriteRenderer>().sprite = this.sprites[0];
    }
    
    private void GetHitBy(Throwable throwable) {
	// drop held throwable
	if (this.heldThrowableChild != null) {
	    this.heldThrowableChild.GetComponent<SpriteRenderer>().sortingLayerName = "Scenery";
	    this.heldThrowableChild.transform.parent = this.transform.parent;
	    this.heldThrowableChild = null;
	}

	// score plus effect
	GameObject scorePlusGo = new GameObject();
	scorePlusGo.AddComponent<SpriteRenderer>().sprite = this.scorePlusEffectSprite;
	scorePlusGo.AddComponent<ScorePlusText>();
	scorePlusGo.transform.position = this.transform.position;

	// scoring
	Settings.Score += this.ScoreValue;
	Settings.EnemiesKilled += 1;	
	
	// destroy enemy and throwable
	throwable.DestroyPumpkin();
	GameObject.Destroy(this.gameObject);	
    }
}
