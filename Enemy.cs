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
    
    // related objects
    public Player player;

    // movement
    private float currentLungeCooldown;
    private float currentLungeDuration;
    private Vector3 wanderVector;
    private float currentWanderDuration;

    void Start() {	
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
	// do nothing if lunge on cooldown
	if (this.currentLungeCooldown > 0) {
	    this.currentLungeCooldown -= Time.deltaTime;
	    return;
	}

	Vector3 normalizedToPlayer = Vector3.Normalize(this.player.transform.position - this.transform.position);

	// move fast if mid-lunge
	// TODO: ease in and out
	if (this.currentLungeDuration > 0) {
	    this.transform.Translate(normalizedToPlayer * this.LungeSpeed * Time.deltaTime);
	    this.currentLungeDuration -= Time.deltaTime;

	    // go to cooldown if lunge ends
	    if (this.currentLungeDuration <= 0) {
		this.currentLungeCooldown = this.LungeCooldown;
	    }
	    return;
	}
	     
	float distanceToPlayer = Vector3.Distance(this.transform.position, this.player.transform.position);
	
	if (distanceToPlayer < this.LungeAggroDistance) {
	    // maybe start a lunge
	    this.currentLungeDuration = this.LungeDuration;
	} else if (distanceToPlayer < this.WalkAggroDistance) {
	    // maybe move normal speed toward player
	    this.transform.Translate(normalizedToPlayer * this.WalkSpeed * Time.deltaTime);	
	} else {
	    // enemy wanders
	    if (this.currentWanderDuration < 0) {
		// choose new wander vector
		this.wanderVector = Vector3.Normalize(new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0.0f));
		this.currentWanderDuration = this.WanderDuration;		
	    }

	    this.transform.Translate(this.wanderVector * this.WalkSpeed * Time.deltaTime);
	    this.currentWanderDuration -= Time.deltaTime;
	}
    }

    public void OnTriggerEnter2D(Collider2D collider) {
	Throwable throwable = collider.gameObject.GetComponent<Throwable>();
	if (throwable != null && throwable.momentum > 0) {
	    this.GetHitBy(throwable);
	}
    }

    private void GetHitBy(Throwable throwable) {
	// destroy enemy and throwable
	GameObject.Destroy(throwable.gameObject);
	GameObject.Destroy(this.gameObject);
    }
}
