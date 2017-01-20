using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

	// Static singleton property
	private static AudioController instance;

	//----------------------------------------------------------------//

	public static AudioController Instance{
		get{
			if(instance == null){
				instance = GameObject.FindObjectOfType<AudioController>();
				//DontDestroyOnLoad(instance.gameObject);
			}

			return instance;
		}
	}

	//----------------------------------------------------------------//

	void Awake(){
		instance = GameObject.FindObjectOfType<AudioController>();
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
