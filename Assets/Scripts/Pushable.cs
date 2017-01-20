using UnityEngine;
using System.Collections;

public class Pushable : MonoBehaviour {

	private Rigidbody2D my_body;
	private GameController gc;

	void Start () {
		gc = GameController.Instance;
		my_body = GetComponent<Rigidbody2D> ();
	}

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

	public void ApplyForce (Vector2 impulse) {
		my_body.AddForce (impulse);
	}

	public Vector3 GetOrigin3D () {
		return new Vector3 (my_body.centerOfMass.x, my_body.centerOfMass.y, 0); 
	}
}
