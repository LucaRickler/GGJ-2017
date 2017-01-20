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

	private Rigidbody2D my_body;

	public GameObject sphericWave;
	public GameObject conicWave;

	// Use this for initialization
	void Start () {
		gc = GameController.Instance;
		my_body = GetComponent<Rigidbody2D> ();
		input_ready = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		var speed = my_body.velocity;
		if (speed.x > gc.max_speed)
			speed.x = gc.max_speed;
		else if (speed.x < -gc.max_speed)
			speed.x = -gc.max_speed;
		if (speed.y > gc.max_speed)
			speed.y = gc.max_speed;
		else if (speed.y < -gc.max_speed)
			speed.y = -gc.max_speed;
		my_body.velocity = speed;
	}

	public void RecordInput (InputType type) {
		if (input_ready) {
			Debug.Log (type);
			switch (type) {
			case InputType.DOWN: 
				ApplyForce (new Vector2(0.0f,-gc.key_input_force));
				break;
			case InputType.LEFT:
				ApplyForce (new Vector2(-gc.key_input_force,0.0f));
				break;
			case InputType.RIGHT:
				ApplyForce (new Vector2(gc.key_input_force,0.0f));
				break;
			case InputType.UP:
				ApplyForce (new Vector2(0.0f,gc.key_input_force*5));
				break;
			case InputType.LEFT_M_DOWN:
				break;
			case InputType.LEFT_M_UP:
				break;
			case InputType.RIGHT_M_DOWN:
				break;
			case InputType.RIGHT_M_UP:
				break;
			}
		}
	}

	public void ApplyForce (Vector2 impulse) {
		my_body.AddForce (impulse);
	}
}
