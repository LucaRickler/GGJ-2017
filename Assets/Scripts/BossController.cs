using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartBattle () {
		StartCoroutine ("BossBattle");
	}

	IEnumerator BossBattle () {
		yield return null;
	}
}
