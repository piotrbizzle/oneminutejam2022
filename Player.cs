using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour  {
    // configurables
    private float MaxSpeed = 3.0f;
    private float Drag = 15.0f;
    private float StartingCharge = 0.2f;
    private float ChargeSpeed = 1.0f;
    private float ThrowSpeed = 10.0f;

    // related objects
    public Pointer pointerChild;
    public Throwable heldThrowableChild;

    // controls and movement
    private float xMomentum;
    private float yMomentum;
    private float meterScale = 0.0f;

    private bool spacePressed;

    public void Start() {
	// collision
        this.gameObject.AddComponent<BoxCollider2D>();

	Rigidbody2D rb = this.gameObject.AddComponent<Rigidbody2D>();
	rb.gravityScale = 0.0f;
	rb.constraints = RigidbodyConstraints2D.FreezeRotation;
	rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
	rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
    }

    public void Update() {
	// get inputs
	bool up = Input.GetKey("w");
	bool down = Input.GetKey("s");
	bool left = Input.GetKey("a");
	bool right = Input.GetKey("d");
	bool mousePressed = Input.GetMouseButton(0);
	bool mouseReleased = Input.GetMouseButtonUp(0);
	this.spacePressed = Input.GetKey("space");

	// throw held item
	if (mouseReleased) {
	    this.Throw();
	}

	// charge meter
	if (mousePressed && this.heldThrowableChild != null) {
	    if (this.meterScale < this.StartingCharge) {
		this.meterScale = this.StartingCharge;
	    }
	    else {
		this.meterScale += this.ChargeSpeed * Time.deltaTime;
		if (this.meterScale > 1.0f) {
		    this.meterScale = 1.0f;
		}
	    }
	} else {
	    this.meterScale = 0.0f;
	}

	// scale meter
	this.pointerChild.meterChild.transform.localScale = new Vector3(1.0f, this.meterScale, 1.0f);
	this.pointerChild.meterChild.transform.localPosition = new Vector3(0.0f, this.meterScale / 3.0f, 0.0f);

	// set momentum
	if (up && !down) {
	    this.yMomentum = this.MaxSpeed;
	} else if (down && !up) {
	    this.yMomentum = this.MaxSpeed * -1;
	} else if (this.yMomentum > 0) {
	    this.yMomentum -= this.Drag * Time.deltaTime;
	    if (this.yMomentum < 0) {
		this.yMomentum = 0;
	    }
	} else if (this.yMomentum < 0) {
	    this.yMomentum += this.Drag * Time.deltaTime;
	    if (this.yMomentum > 0) {
		this.yMomentum = 0;
	    }
	}

	if (left && !right) {
	    this.xMomentum = this.MaxSpeed;
	} else if (right && !left) {
	    this.xMomentum = this.MaxSpeed * -1;
	} else if (this.xMomentum > 0) {
	    this.xMomentum -= this.Drag * Time.deltaTime;
	    if (this.xMomentum < 0) {
		this.xMomentum = 0;
	    }
	} else if (this.xMomentum < 0) {
	    this.xMomentum += this.Drag * Time.deltaTime;
	    if (this.xMomentum > 0) {
		this.xMomentum = 0;
	    }
	}

	if ((up || down) && (left || right)) {
	    this.xMomentum *= 0.7f;
	    this.yMomentum *= 0.7f;
	}
	
	// apply momentum
	this.transform.Translate(Vector3.up * Time.deltaTime * this.yMomentum);
	this.transform.Translate(Vector3.left * Time.deltaTime * this.xMomentum);
    }

    public void OnTriggerStay2D(Collider2D collider) {
	Throwable throwable = collider.gameObject.GetComponent<Throwable>();
	if (throwable != null && this.spacePressed) {
	    this.PickUp(throwable);
	    return;
	}
    }

    private void PickUp(Throwable throwable) {
	// hold at most one throwable at a time
	if (this.heldThrowableChild != null) {
	    return;
	}
	
	// pick up item
	throwable.gameObject.transform.parent = this.pointerChild.transform;
	throwable.gameObject.transform.localPosition = new Vector3(0, 0, 0);
	throwable.GetComponent<SpriteRenderer>().sortingLayerName = "Held";
	this.heldThrowableChild = throwable;
    }

    private void Throw() {
	if (this.heldThrowableChild == null) {
	    return;
	}
	this.heldThrowableChild.Throw(this.ThrowSpeed * this.meterScale);
	this.heldThrowableChild.transform.parent = this.transform.parent;
	this.heldThrowableChild.transform.rotation = this.pointerChild.transform.rotation;
	this.heldThrowableChild = null;
    }
}

