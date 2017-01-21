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

	private float click_timer;
	private bool _charging_wave;
	public bool isChargingWave { get { return _charging_wave; } }

	public float player_wave_convertion = 200;
	public float min_wave_intensity = 150;
	public float max_wave_intensity = 300;
	public float max_charge_time = 5;

	// Use this for initialization
	void Start () {
		//charging_wave = false;
		my_pushable = GetComponent<Pushable> ();
		gc = GameController.Instance;
		input_ready = true;
	}
	
	// Update is called once per frame
	public void FixedUpdate() {
		if (isChargingWave && click_timer < max_charge_time)
			click_timer += Time.fixedDeltaTime;
	}

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
				click_timer = 0;
				_charging_wave = true;
				break;
			case InputType.LEFT_M_UP:
				float intensity = click_timer * player_wave_convertion;
				intensity = intensity < min_wave_intensity ? min_wave_intensity : intensity;
				intensity = intensity > max_wave_intensity ? max_wave_intensity : intensity;
				_charging_wave = false;
                GameController.Instance.SpawnWave(my_pushable.safeZoneCollider, transform.position, intensity, 0, 1, WaveDirectionEnum.FORWARD, true);
				break;
			case InputType.RIGHT_M_DOWN:
				break;
			case InputType.RIGHT_M_UP:
				break;
			}
		}
	}
}
