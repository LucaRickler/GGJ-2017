using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bomb : MonoBehaviour {

	public float detectionRange = 10;

	private BombSpawner my_spawner;

	private bool small_fish;
	// Use this for initialization
	void Start () {
		//GetComponent<Rigidbody2D> ().gravityScale = 0.0f;
	}

	void update () {
		//if(Vector2.Distance(GameController.Instance.player.transform.position, transform.position))
	}
	
	public void init (bool isStatic, Vector3 startPosition, Vector3 objectivePoint, bool small_fish) {
		transform.position = startPosition;
		if(isStatic) { //Bomba
			//GetComponent<Rigidbody> ().useGravity = false;
			StartCoroutine("InitBomb", objectivePoint);
		}
		//Kamikaze
		this.small_fish = small_fish;
	}

	IEnumerator InitBomb (Vector3 objective) {
		while (Vector3.Distance (objective, transform.position) > 0.1) {
			transform.position = Vector3.Lerp (objective, transform.position, 0.2f);
			yield return new WaitForFixedUpdate();
		}
	}

	void OnCollisionEnter2D (Collision2D other) {
		//StartCoroutine ("Explode", other.gameObject.GetComponent<Pushable> ());
	}

	IEnumerator Explode (Pushable victim) {
		yield return null;
		my_spawner.BombHasExploded ();
		Destroy (gameObject);
	}
}	
