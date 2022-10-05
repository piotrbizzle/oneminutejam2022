using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    void Start() {
	// collision
        this.gameObject.AddComponent<BoxCollider2D>();

	Rigidbody2D rb = this.gameObject.AddComponent<Rigidbody2D>();
	rb.gravityScale = 0.0f;
	rb.constraints = RigidbodyConstraints2D.FreezeAll;
	rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
	rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

	// rendering
	this.GetComponent<SpriteRenderer>().material = Settings.SpriteMaterial;
    }

    public void OnTriggerEnter2D(Collider2D collider) {
	Throwable throwable = collider.gameObject.GetComponent<Throwable>();
	if (throwable != null && throwable.momentum > 0) {
	    this.GetHitBy(throwable);
	}
    }

    private void GetHitBy(Throwable throwable) {
	// break throwables that collide
	if (!throwable.markedForDestroy) {
	    Settings.PumpkinsSmashedOnBarriers += 1;
	}
	throwable.DestroyPumpkin();
    }
}
