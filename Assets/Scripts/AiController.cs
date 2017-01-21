using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour {

	public AIType ai_type;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		switch (ai_type) {
		case AIType.SMALL_MOB:
			break;
		case AIType.BIG_MOB:
			break;
		case AIType.WELL:
			break;
		}
	}
}

public enum AIType {
	SMALL_MOB,
	BIG_MOB,
	WELL,
	NUMBER_OF_TYPES
}