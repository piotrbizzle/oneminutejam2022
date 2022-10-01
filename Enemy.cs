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

    // related objects
    public Player player;

    // movement
    private float currentLungeCooldown;
    private float currentLungeDuration;
    
    void Start() {	
	// collision
        this.gameObject.AddComponent<BoxCollider2D>();

	Rigidbody2D rb = this.gameObject.AddComponent<Rigidbody2D>();
	rb.gravityScale = 0.0f;
	rb.constraints = RigidbodyConstraints2D.FreezeRotation;
	rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
	rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
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

	// otherwise, move normal speed
	this.transform.Translate(normalizedToPlayer * this.WalkSpeed * Time.deltaTime);
	
	// then, maybe start a lunge
	float distanceToPlayer = Vector3.Distance(this.transform.position, this.player.transform.position);
	if (distanceToPlayer < this.LungeAggroDistance) {
	    this.currentLungeDuration = this.LungeDuration;
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
