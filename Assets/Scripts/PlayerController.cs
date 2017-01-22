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

	public GameObject Cone;
	public SpriteRenderer backwave;

	public bool input_ready;

	private GameController gc;

	private Pushable my_pushable;

    private float click_timer_left;
    private float click_timer_right;
    private bool _charging_left;
	public bool isChargingLeft { get { return _charging_left; } }
    private bool _charging_right;
    public bool isChargingRight { get { return _charging_right; } }

    public WaveChargingCongig leftMouseWave;
    public WaveChargingCongig rightMouseWave;

	public float rotation_speed = 1000.0f;

    // Use this for initialization
    void Start () {
		//charging_wave = false;
		my_pushable = GetComponent<Pushable> ();
		gc = GameController.Instance;
		input_ready = true;
        //my_pushable.isPC = true;
    }

    // Update is called once per frame
    public void FixedUpdate() {
		if ((isChargingLeft) && click_timer_left < leftMouseWave.max_charge_time)
			click_timer_left += Time.fixedDeltaTime;
		if ((isChargingRight) && click_timer_right < rightMouseWave.max_charge_time) {
			click_timer_right += Time.fixedDeltaTime;
			Color c = backwave.color;
			c.a = click_timer_right / rightMouseWave.max_charge_time;
			backwave.color = c;
		}
		if (Cone.activeSelf) {
			Vector3 mouseposition = gc.sceneCamera.ScreenToWorldPoint (Input.mousePosition);
            mouseposition.z = transform.position.z;
            Vector3 dir = mouseposition - transform.position;
            transform.localRotation = Quaternion.Inverse(Quaternion.FromToRotation(dir, Vector3.left));
            //transform.localRotation = Quaternion.FromToRotation (Vector3.forward, dir);
            //(new Vector3 (0, 0, Mathf.Atan2 (dir.y, dir.x)) * Time.deltaTime * rotation_speed);
            //Cone.transform.localRotation = Quaternion.FromToRotation (Vector3.forward, dir);// (new Vector3 (0, 0, Mathf.Atan2 (dir.y, dir.x))); //* Time.deltaTime * rotation_speed);
        }
    }

	public void RecordInput (InputType type) {
		if (input_ready) {
			Debug.Log (type);
            Vector3 mousePosition = Input.mousePosition;
            Vector3 playerScreenCoord = GameController.Instance.sceneCamera.WorldToScreenPoint(gameObject.transform.position);
            Vector3 waveDir = (mousePosition - playerScreenCoord).normalized;
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

				click_timer_left = 0;
				_charging_left = true;
				leftMouseWave.chargingDebug = true;
				if (leftMouseWave.type == WaveType.SPHERIC){
					if (rightMouseWave.type == WaveType.DIRETIONAL && !isChargingRight)
						Cone.SetActive (false);
					//GameController.Instance.SpawnSphericWave(my_pushable.safeZoneCollider, transform.position, intensity, leftMouseWave.startRadius, WaveDirectionEnum.FORWARD, true);
				} else {
					Cone.SetActive (true);
				}
                break;
            case InputType.RIGHT_M_DOWN:
                click_timer_right = 0;
                _charging_right = true;
                rightMouseWave.chargingDebug = true;
				//if (leftMouseWave.type == WaveType.SPHERIC){
					if (leftMouseWave.type == WaveType.DIRETIONAL && !isChargingLeft)
						Cone.SetActive (false);
					backwave.gameObject.SetActive (true);
					//GameController.Instance.SpawnSphericWave(my_pushable.safeZoneCollider, transform.position, intensity, leftMouseWave.startRadius, WaveDirectionEnum.FORWARD, true);
				//} else {
					//Cone.SetActive (true);
				//}
                break;
			case InputType.LEFT_M_UP:
				float intensity = click_timer_left * leftMouseWave.player_wave_convertion;
				intensity = intensity < leftMouseWave.min_wave_intensity ? leftMouseWave.min_wave_intensity : intensity;
				intensity = intensity > leftMouseWave.max_wave_intensity ? leftMouseWave.max_wave_intensity : intensity;
				leftMouseWave.IntensityDebug = intensity;
				_charging_left = false;
				leftMouseWave.chargingDebug = false;
				if (leftMouseWave.type == WaveType.SPHERIC) {
					AudioController.Instance.PlaySFX (AudioController.SFX.CANNONE_GRANDE);
					GameController.Instance.SpawnSphericWave (my_pushable.safeZoneCollider, transform.position, intensity, leftMouseWave.startRadius, WaveDirectionEnum.FORWARD, true);
				}else
                {	
					Cone.SetActive (false);
					AudioController.Instance.PlaySFX(AudioController.SFX.CANNONE_PICCOLO);
                    GameController.Instance.SpawnDiretionalWave(my_pushable.safeZoneCollider, transform.position, waveDir, intensity, GameController.Instance.spread, leftMouseWave.startRadius, WaveDirectionEnum.FORWARD, true);
                }
                break;
			case InputType.RIGHT_M_UP:
				intensity = click_timer_right * rightMouseWave.player_wave_convertion;
				intensity = intensity < rightMouseWave.min_wave_intensity ? rightMouseWave.min_wave_intensity : intensity;
				intensity = intensity > rightMouseWave.max_wave_intensity ? rightMouseWave.max_wave_intensity : intensity;
				rightMouseWave.IntensityDebug = intensity;
				_charging_right = false;
				rightMouseWave.chargingDebug = false;
				if (rightMouseWave.type == WaveType.SPHERIC) {
					Color c = backwave.color;
					c.a = 0.0f;
					backwave.color = c;
					backwave.gameObject.SetActive (false);
					AudioController.Instance.PlaySFX (AudioController.SFX.CANNONE_GRANDE);
					GameController.Instance.SpawnSphericWave (my_pushable.safeZoneCollider, transform.position, intensity, rightMouseWave.startRadius, WaveDirectionEnum.FORWARD, true);
				}else
                {
					Cone.SetActive (false);
					AudioController.Instance.PlaySFX(AudioController.SFX.CANNONE_PICCOLO);
					GameController.Instance.SpawnDiretionalWave(my_pushable.safeZoneCollider, transform.position, waveDir, intensity, GameController.Instance.spread, rightMouseWave.startRadius, WaveDirectionEnum.FORWARD, true);
                }
                break;
			}
		}
	}
}

[System.Serializable]
public class WaveChargingCongig
{
    public WaveType type;
    public float player_wave_convertion = 200; //200
    public float min_wave_intensity = 150; //150
    public float max_wave_intensity = 300; //300
    public float max_charge_time = 5; //5
    public float startRadius = 1;
    [Header("===== DEBUG =====")]
    public float IntensityDebug = 0;
    public bool chargingDebug = false;
}