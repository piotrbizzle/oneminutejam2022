using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour  {
    // configurables
    private float RunSpeed = 3.5f;
    private float WalkSpeed = 2f;
    private float Drag = 15.0f;
    private float StartingCharge = 0.2f;
    private float ChargeSpeed = 1.0f;
    private float ThrowSpeed = 10.0f;

    // related objects
    public GameObject eventSystemGo;
    public Pointer pointerChild;
    public Throwable heldThrowableChild;

    // animation
    public Sprite[] sprites = new Sprite[3];
    public float walkAnimationBurdenedDelay = 0.2f;    
    public float walkAnimationDelay = 0.1f;
    public float currentWalkAnimationDelay = 0;
    public int currentWalkAnimationFrame = 1;
    
    // controls and movement
    private float xMomentum;
    private float yMomentum;
    private float meterScale = 0.0f;

    private bool mouseClicked;
    private bool mousePressed;

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
	this.mousePressed = Input.GetMouseButton(0);
	this.mouseClicked = Input.GetMouseButtonDown(0);
	bool mouseReleased = Input.GetMouseButtonUp(0);
	bool escapePressed = Input.GetKey("escape");

	// do nothing ending
	if (!Settings.PlayerDidAnything && (up || down || left || right || this.mousePressed || this.mouseClicked || escapePressed)) {
	    Settings.PlayerDidAnything = true;
	}	
	
	// exit game early
	if (escapePressed) {
	    // turn off event system from old scene
	    GameObject.Destroy(this.eventSystemGo);
	    SceneManager.LoadScene("EndMenu");
	}

	// throw held item
	if (mouseReleased) {
	    this.Throw();
	}

	// charge meter
	if (this.mousePressed && this.heldThrowableChild != null) {
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
	float speed = this.heldThrowableChild == null ? this.RunSpeed : this.WalkSpeed;
	if (up && !down) {
	    this.yMomentum = speed;
	} else if (down && !up) {
	    this.yMomentum = speed * -1;
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
	    this.xMomentum = speed;
	} else if (right && !left) {
	    this.xMomentum = speed * -1;
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
	Vector3 previousPosition = this.transform.position;
	this.transform.Translate(Vector3.up * Time.deltaTime * this.yMomentum);
	this.transform.Translate(Vector3.left * Time.deltaTime * this.xMomentum);
	if (this.transform.position.x < -4.7f) {
	    this.transform.position = new Vector3(-4.7f, this.transform.position.y, 0);
	}
	if (this.transform.position.x > 4.7f) {
	    this.transform.position = new Vector3(4.7f, this.transform.position.y, 0);
	}
	if (this.transform.position.y < -4.6f) {
	    this.transform.position = new Vector3(this.transform.position.x, -4.6f, 0);
	}
	if (this.transform.position.y > 4.6f) {
	    this.transform.position = new Vector3(this.transform.position.x, 4.6f, 0);
	}
	Settings.DistanceTravelled += Vector3.Distance(previousPosition, this.transform.position);
	

	// animation
	if (this.xMomentum != 0 || this.yMomentum != 0) {
	    if (this.currentWalkAnimationDelay <= 0) {
		this.currentWalkAnimationDelay = this.heldThrowableChild == null ? this.walkAnimationDelay : this.walkAnimationBurdenedDelay;
		// this only works for exactly 2 frames
		this.currentWalkAnimationFrame = this.currentWalkAnimationFrame % 2 + 1;
	    } else {
		this.currentWalkAnimationDelay -= Time.deltaTime;
	    }
	    this.GetComponent<SpriteRenderer>().sprite = this.sprites[this.currentWalkAnimationFrame];
	} else {
	    this.GetComponent<SpriteRenderer>().sprite = this.sprites[0];
	}
	
    }

    public void OnTriggerStay2D(Collider2D collider) {
	Throwable throwable = collider.gameObject.GetComponent<Throwable>();
	if (throwable != null && this.mousePressed) {
	    this.PickUp(throwable);
	    return;
	}
    }

    private void PickUp(Throwable throwable) {
	// hold at most one throwable at a time
	if (this.heldThrowableChild != null) {
	    return;
	}

	// don't steal pumpkin back before cooldown is up
	// TODO: better way to tell held pumpkin
	if (throwable.GetComponent<SpriteRenderer>().sortingLayerName == "Held" && !throwable.IsStealable()) {
	    return;
	} else {
	    throwable.GetStolen();
	}
	    
	
	// pick up item
	throwable.gameObject.transform.parent = this.pointerChild.transform;
	throwable.gameObject.transform.localPosition = new Vector3(0, -0.1f, 0);
	throwable.GetComponent<SpriteRenderer>().sortingLayerName = "Held";
	this.heldThrowableChild = throwable;
	throwable.Throw(0);
    }

    private void Throw() {
	if (this.heldThrowableChild == null) {
	    return;
	}
	this.heldThrowableChild.transform.localPosition = new Vector3(0, 0.1f, 0);
	this.heldThrowableChild.Throw(this.ThrowSpeed * this.meterScale);
	this.heldThrowableChild.GetComponent<SpriteRenderer>().sortingLayerName = "Scenery";
	this.heldThrowableChild.transform.parent = this.transform.parent;
	this.heldThrowableChild.transform.rotation = this.pointerChild.transform.rotation;
	this.heldThrowableChild = null;
    }
}
