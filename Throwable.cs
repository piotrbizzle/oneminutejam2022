using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Throwable : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[6];
    public Sprite enemySprite;
    
    // configurables
    private float Drag = 10.0f;
    private float MomentumToBreakRatio = 0.6f;

    // colors
    private static Color Green = new Color(16f / 256f, 118f / 256f, 84 / 256f, 1f);
    private static Color DarkGreen = new Color(8f / 256f, 78f / 256f, 49 / 256f, 1f);
    private static Color Yellow = new Color(238f / 255f, 186f / 256f, 59 / 256f, 1f);
    private static Color Orange = new Color(221f / 256f, 135f / 256f, 33 / 256f, 1f);
    private static Color Ochre = new Color(135f / 256f, 85f / 256f, 25 / 256f, 1f);
    private static Color Brown = new Color(102f / 256f, 32f / 256f, 0f, 1f);

    
    // related objects
    public Player player;

    // related prefabs
    public ParticleSystem particles;
    
    private float initialMomentum;
    public float currentRot;
    public float momentum;
    private bool markedForDestroy;
    
    public void Start() {
 	// collision
        this.gameObject.AddComponent<BoxCollider2D>();
	this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

	// rendering
	this.GetComponent<SpriteRenderer>().sortingLayerName = "Scenery";
	this.GetComponent<SpriteRenderer>().material = Settings.SpriteMaterial;
	if (this.currentRot < 10f) {
	    this.GetComponent<SpriteRenderer>().sprite = this.sprites[0];
	    this.GetComponent<SpriteRenderer>().color = Throwable.Ochre;
	} else if (this.currentRot < 20f) {
	    this.GetComponent<SpriteRenderer>().sprite = this.sprites[1];
	    this.GetComponent<SpriteRenderer>().color = Throwable.Orange;
	} else if (this.currentRot < 30f) {
	    this.GetComponent<SpriteRenderer>().sprite = this.sprites[2];
	    this.GetComponent<SpriteRenderer>().color = Throwable.Yellow;
	} else if (this.currentRot < 40f) {
	    this.GetComponent<SpriteRenderer>().sprite = this.sprites[3];
	    this.GetComponent<SpriteRenderer>().color = Throwable.DarkGreen;
	} else if (this.currentRot < 50f) {
	    this.GetComponent<SpriteRenderer>().sprite = this.sprites[4];
	    this.GetComponent<SpriteRenderer>().color = Throwable.Green;
	} else {
	    this.GetComponent<SpriteRenderer>().sprite = this.sprites[5];
	    this.GetComponent<SpriteRenderer>().color = Throwable.Green;
	}
    }

    public void Update() {
	// rendering
	this.SetColor();

	// rot
	this.currentRot -= Time.deltaTime;
	if (this.currentRot < 0) {
	    this.RotAway();
	}

	// check if object thrown
	if (this.initialMomentum == 0) {
	    return;
	}

	// move object
	this.transform.Translate(Vector3.up * this.momentum * Time.deltaTime);

	// slow down object
	this.momentum -= Time.deltaTime * this.Drag;

	// break once the object slows down enough
	if (this.momentum < this.initialMomentum * this.MomentumToBreakRatio) {
	    this.momentum = 0;
	    this.initialMomentum = 0;
	}
    }

    private void SetColor() {
	SpriteRenderer sr = this.GetComponent<SpriteRenderer>();

	// determine target color
	Color targetColor;
	if (this.currentRot < 10f) {
	    this.GetComponent<SpriteRenderer>().sprite = this.sprites[0];
	    targetColor = Throwable.Brown;
	} else if (this.currentRot < 20f) {
	    this.GetComponent<SpriteRenderer>().sprite = this.sprites[1];
	    targetColor = Throwable.Ochre;
	} else if (this.currentRot < 30f) {
	    this.GetComponent<SpriteRenderer>().sprite = this.sprites[2];
	    targetColor = Throwable.Orange;
	} else if (this.currentRot < 40f) {
	    this.GetComponent<SpriteRenderer>().sprite = this.sprites[3];
	    targetColor = Throwable.Yellow;
	} else if (this.currentRot < 50f) {
	    this.GetComponent<SpriteRenderer>().sprite = this.sprites[4];
	    targetColor = Throwable.DarkGreen;
	} else {
	    this.GetComponent<SpriteRenderer>().sprite = this.sprites[5];
	    targetColor = Throwable.Green;
	}

	// average target and current color to get halfway step
	Color colorStep = new Color(
	    ((sr.color.r + targetColor.r) / 2 - sr.color.r) * Time.deltaTime,
	    ((sr.color.g + targetColor.g) / 2 - sr.color.g) * Time.deltaTime,
	    ((sr.color.b + targetColor.b) / 2 - sr.color.b) * Time.deltaTime
	);

	// step toward averaged target color
	sr.color += colorStep;
    }

    public void Throw(float initialMomentum) {
	this.initialMomentum = initialMomentum;
	this.momentum = initialMomentum;
    }

    public int GetScoreBucket() {       
	return ((int)Math.Floor(this.currentRot / 10.0f));
    }
    
    private void RotAway() {
	// spawn an enemy
	GameObject enemyGo = new GameObject();
	enemyGo.AddComponent<SpriteRenderer>().sprite = this.enemySprite;;
	enemyGo.AddComponent<Enemy>().player = this.player;
	enemyGo.transform.position = this.transform.position;

	// delete the throwable
	this.DestroyPumpkin();
    }

    public void DestroyPumpkin(bool doExplode=true) {
	if (this.markedForDestroy) {
	    return;
	}
	this.markedForDestroy = true;	
	Settings.PumpkinsLeft -= 1;
	GameObject.Destroy(this.gameObject);

	// particles
	if (doExplode) {
	    ParticleSystem particles = Instantiate(this.particles) as ParticleSystem;	
	    particles.transform.position = this.transform.position;	    

	    // this has to be done in two lines for arcane reasons
	    var main = particles.main;
	    main.startColor = this.GetComponent<SpriteRenderer>().color;
	}
    }
}
