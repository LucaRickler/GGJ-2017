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

	private Pushable my_pushable;

	public GameObject sphericWave;
	public GameObject conicWave;

	private Time click_timer;
	private bool _charging_wave;
	public bool isChargingWave { get { return _charging_wave; } }

	// Use this for initialization
	void Start () {
		//charging_wave = false;
		my_pushable = GetComponent<Pushable> ();
		gc = GameController.Instance;
		input_ready = true;
	}
	
	// Update is called once per frame

	public void RecordInput (InputType type) {
		if (input_ready) {
			Debug.Log (type);
			switch (type) {
			case InputType.DOWN: 
				my_pushable.ApplyForce (new Vector2(0.0f,-gc.key_input_force));
				break;
			case InputType.LEFT:
				my_pushable.ApplyForce (new Vector2(-gc.key_input_force,0.0f));
				break;
			case InputType.RIGHT:
				my_pushable.ApplyForce (new Vector2(gc.key_input_force,0.0f));
				break;
			case InputType.UP:
				my_pushable.ApplyForce (new Vector2(0.0f,gc.key_input_force*5));
				break;
			case InputType.LEFT_M_DOWN:
				//click_timer = Time.time;
				GameController.Instance.SpawnWave (transform.position, /*sphericWave, Input.mousePosition, 0.0f,*/200, 1.0f);
				break;
			case InputType.LEFT_M_UP:
				//float intensity = (Time.time - click_timer) * gc.player_wave_convertion;

				//GameController.Instance.SpawnWave (transform.position, /*sphericWave, Input.mousePosition, 0.0f,*/intensity, 1.0f);
				break;
			case InputType.RIGHT_M_DOWN:
				break;
			case InputType.RIGHT_M_UP:
				break;
			}
		}
	}
}
