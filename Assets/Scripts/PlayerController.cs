using UnityEngine;
using System.Collections;

public enum InputType {
	LEFT,
	RIGHT,
	UP,
	DOWN,
	LEFT_M_DOWN,
	LEFT_M_UP,
	RIGHT_M_DOWN,
	RIGHT_M_UP,
	NUMBER_OF_INPUTS
};

public class PlayerController : MonoBehaviour {

	private bool input_ready;

	private GameController gc;

	// Use this for initialization
	void Start () {
		gc = GameController.Instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RecordInput (InputType type) {
		//if(input_ready)
	}
}
