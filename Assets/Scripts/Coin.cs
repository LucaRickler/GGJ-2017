using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coin : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D other) {
		if(other.gameObject.tag != "Player")
			StartCoroutine ("Collect", other);
	}

	IEnumerator Collect () {
		yield return null;
		GameController.Instance.CollectCoin ();
		Destroy (gameObject);
	}

	void Update () {
		//transform.rotation.eulerAngles.z = transform.rotation.eulerAngles.z + 10 * Time.deltaTime;
	}
}
