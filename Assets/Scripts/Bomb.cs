﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bomb : MonoBehaviour {

	public float detectionRange = 10;

	private BombSpawner my_spawner;

	private bool small_fish;
	private bool chasing;
	public bool amBomb;

	public GameObject explosion;

	public float speed = 10.0f;

	// Use this for initialization
	void Start () {
		chasing = false;
		//GetComponent<Rigidbody2D> ().gravityScale = 0.0f;
	}

	void Update () {
		if(!amBomb){
			if (Vector2.Distance (GameController.Instance.player.transform.position, transform.position) < detectionRange)
				chasing = true;
			if (chasing) {
				
				//Vector3 direction = GameController.Instance.player.gameObject.transform.position - transform.position;
				//transform.position = transform.position + (direction.normalized * speed * Time.deltaTime);
				transform.position = Vector3.MoveTowards(transform.position, GameController.Instance.player.transform.position, 5*Time.deltaTime);
			}
		}
	}
	
	public void init (bool isStatic, Vector3 startPosition, Vector3 objectivePoint, bool small_fish, BombSpawner spawner) {
		transform.position = startPosition;
		if(isStatic) { //Bomba
			my_spawner = spawner;
			amBomb = true;
			//GetComponent<Rigidbody> ().useGravity = false;
			StartCoroutine("InitBomb", objectivePoint);
		} else {
			//Kamikaze
			this.small_fish = small_fish;
			amBomb = false;
		}
	}
	IEnumerator InitBomb (Vector3 objective) {
		while (Vector3.Distance (objective, transform.position) > 0.5) {
			transform.position = Vector3.Lerp (objective, transform.position, 0.9f);
			yield return new WaitForSeconds(0.02f);
		}
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.tag != "Bullets" || !amBomb) && other.gameObject.tag != "MainCamera")
            StartCoroutine("Explode", other.gameObject);
    }

    IEnumerator Explode(GameObject victim)
    {
        yield return null;
        //TODO: Damage!
        explosion.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        if (victim != null)
		{
			if (!(victim.tag == "Terrain") & !(victim.tag == "Player") && (victim.tag != "Bullets"))
                Destroy(victim);
            else if (victim.tag == "Player")
                GameController.Instance.PlayerDeath();
        }
		if (amBomb)
			my_spawner.BombHasExploded();
		//AudioController.Instance.PlaySFX(AudioController.SFX.ESPLOSION);
		Destroy(gameObject);

    }
}	
