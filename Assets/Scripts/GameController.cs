using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public PlayerController player; 

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
}
