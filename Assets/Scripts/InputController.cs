using UnityEngine;
using System.Collections;
using System;

public class InputController : MonoBehaviour {

	public PlayerController player;

	// Static singleton property
	private static InputController instance;

	//----------------------------------------------------------------//

	public static InputController Instance{
		get{
			if(instance == null){
				instance = GameObject.FindObjectOfType<InputController>();
				//DontDestroyOnLoad(instance.gameObject);
			}

			return instance;

		}
	}

	//----------------------------------------------------------------//

	void Awake(){
		instance = GameObject.FindObjectOfType<InputController>();
		if (instance != null && instance != this) {
			Destroy (gameObject);
		}
		instance = this;

		DontDestroyOnLoad(gameObject);
	}
	

	void Update () {
		if (Input.GetKeyDown (KeyCode.UpArrow))
			player.RecordInput (InputType.UP);
		else if (Input.GetKeyDown (KeyCode.RightArrow))
			player.RecordInput (InputType.RIGHT);
		else if (Input.GetKeyDown (KeyCode.DownArrow))
			player.RecordInput (InputType.DOWN);
		else if (Input.GetKeyDown (KeyCode.LeftArrow))
			player.RecordInput (InputType.LEFT);
		else if (Input.GetMouseButtonDown(0))
			player.RecordInput (InputType.LEFT_M_DOWN);
		else if (Input.GetMouseButtonUp(0))
			player.RecordInput (InputType.LEFT_M_UP);
		else if (Input.GetMouseButtonDown(1))
			player.RecordInput (InputType.RIGHT_M_DOWN);
		else if (Input.GetMouseButtonUp(1))
			player.RecordInput (InputType.RIGHT_M_UP);
	}
}
