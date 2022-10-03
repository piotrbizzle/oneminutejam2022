using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    // configurables
    public int NumberOfBarriers = 12;
    public int[] NumberOfPumpkins = new int[]{0, 10, 10, 10, 10, 10};  // rotten to fresh
    public float PumpkinJiggleAmount = 0.1f;
    public int PumpkinJiggleRange = 3;
    
    // related objects
    public Player player;
    
    // prototyped objects
    public Barrier barrierPrototype;
    public Throwable pumpkinPrototype;
    
    public void Start()
    {
	// init randomizer
	if (Settings.SeedString == "") {
	    Settings.SeedString = ((int)DateTime.Now.Ticks).ToString();
	}
	UnityEngine.Random.InitState(Settings.SeedString.GetHashCode());
	
	// make a list of all allowed spaces for objects on a grid
	// grid origin in bottom left
	List<Vector2> availableSpaces = new List<Vector2>();
	for (int x = 0; x < 9; x++) {
	    for (int y = 0; y < 9; y++) {
		if (GameSetup.IsValidSpace(x, y)) {
		    availableSpaces.Add(new Vector2(x, y));
		}
	    }
	}

	// distribute barriers to empty spaces
	for (int i = 0; i < this.NumberOfBarriers; i++) {
	    // new barrier
	    GameObject barrierGo = new GameObject();

	    // copy properties from prototype
	    barrierGo.AddComponent<SpriteRenderer>().sprite = barrierPrototype.GetComponent<SpriteRenderer>().sprite;
	    Barrier barrier = barrierGo.AddComponent<Barrier>();

	    // place randomly on available space
	    barrierGo.transform.position = GameSetup.GridPointToWorldPoint(GameSetup.PopRandomSpace(availableSpaces));

	    // randomly rotate barriers	    		
	    barrierGo.transform.rotation = new Quaternion(0, 0, UnityEngine.Random.Range(0, 3) / 2.0f, 1.0f);
	}

	// distribute pumpkins to random spaces
	Settings.PumpkinsLeft = 0;
	for (int freshness = 0; freshness < NumberOfPumpkins.Length; freshness++) {
	    for (int i = 0; i < NumberOfPumpkins[freshness]; i++) {
		// new pumpkin
		GameObject pumpkinGo = new GameObject();
		Settings.PumpkinsLeft += 1;

		// copy properties from prototype
		// TODO: use prefabs next time
		pumpkinGo.AddComponent<SpriteRenderer>().sprite = pumpkinPrototype.GetComponent<SpriteRenderer>().sprite;
		Throwable throwable = pumpkinGo.AddComponent<Throwable>();
		throwable.currentRot = 10.0f * freshness + UnityEngine.Random.Range(1, 20) * 0.5f;
		throwable.player = this.pumpkinPrototype.player;
		throwable.enemySprites = this.pumpkinPrototype.enemySprites;
		throwable.sprites = this.pumpkinPrototype.sprites;
		throwable.particles = this.pumpkinPrototype.particles;
		throwable.enemyLight = this.pumpkinPrototype.enemyLight;
		
		// place randomly on available space
		pumpkinGo.transform.position = GameSetup.GridPointToWorldPoint(GameSetup.PopRandomSpace(availableSpaces));
		pumpkinGo.transform.Translate(
		    new Vector3(
			UnityEngine.Random.Range(
			    -1 * this.PumpkinJiggleRange,
			    this.PumpkinJiggleRange
			) * this.PumpkinJiggleAmount,
			UnityEngine.Random.Range(
			    -1 * this.PumpkinJiggleRange,
			    this.PumpkinJiggleRange
			) * this.PumpkinJiggleAmount,
			0
		    )
		);
	    }
	}
	
	// remove prototypes
	GameObject.Destroy(this.barrierPrototype.gameObject);
	GameObject.Destroy(this.pumpkinPrototype.gameObject);
    }

    private static bool IsValidSpace(int x, int y) {
	// player square
	if (x == 5 && y == 3) {
	    return false;
	}
	
	// goal squares
	if (x > 2 && x < 6 && y < 3) {
	    return false;
	}

	// score display
	if (x < 2 && y == 0) {
	    return false;
	}

	return true;
    }

    private static Vector3 GridPointToWorldPoint(Vector2 gridPoint) {
	return new Vector3(gridPoint.x - 4.0f, gridPoint.y - 4.0f, 0);
    }

    private static Vector2 PopRandomSpace(List<Vector2> availableSpaces) {
	int index = UnityEngine.Random.Range(0, availableSpaces.Count - 1);
	Vector2 space = availableSpaces[index];
	availableSpaces.RemoveAt(index);
	return space;
    }
}
