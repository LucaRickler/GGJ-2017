using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour {


	public GameObject bombTemplate;
	public Vector3 spawnObjective;
	public Vector3 spawnPosition;

	private bool bombExploded;


	// Use this for initialization
	void Start () {
		Vector3 direction = Vector3.Lerp (transform.position, spawnObjective, 1.0f);
		spawnPosition = transform.position + direction.normalized * 1.0f; 
	}
	
	// Update is called once per frame
	void Update () {
		if (bombExploded) {
			GameObject newElement = Instantiate(bombTemplate) as GameObject;
			newElement.transform.localScale = new Vector3(1, 1, 1);
			//newElement.GetComponent<Bomb>().init(true, spawnPosition, spawnObjective);
			bombExploded = false;
		}
	}

	public void BombHasExploded () {
		if (!bombExploded)
			bombExploded = true;
	}
}
