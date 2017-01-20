using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public PlayerController player;

	public float wave_kappa = 1.0f;

	public float key_input_force = 500;

	public float max_speed = 500;

	// Static singleton property
	private static GameController instance;

	//----------------------------------------------------------------//

	public static GameController Instance{
		get{
			if(instance == null){
				instance = GameObject.FindObjectOfType<GameController>();
				//DontDestroyOnLoad(instance.gameObject);
			}

			return instance;
		}
	}

	//----------------------------------------------------------------//

	void Awake(){
		instance = GameObject.FindObjectOfType<GameController>();
		if (instance != null && instance != this) {
			Destroy (gameObject);
		}
		instance = this;

		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool isCameraFollowMode () {
		return true;
	}
}
