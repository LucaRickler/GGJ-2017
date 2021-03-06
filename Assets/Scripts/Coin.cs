﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coin : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other) {
		if(other.gameObject.tag == "Player" || other.gameObject.tag == "Bullets")
			StartCoroutine ("Collect");
	}

	IEnumerator Collect () {
		yield return null;
		AudioController.Instance.PlaySFX (AudioController.SFX.COINS);
		GameController.Instance.CollectCoin ();
		Destroy (gameObject);
	}

	void Update () {
		//transform.rotation.eulerAngles.z = transform.rotation.eulerAngles.z + 10 * Time.deltaTime;
	}
}
