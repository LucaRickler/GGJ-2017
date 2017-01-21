using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public PlayerController player;

	public float wave_kappa = 0.001f;
	public float key_input_force = 500;
	public float max_speed = 500;
	public float min_force_intensity = 0.1f;

	public float player_wave_convertion = 1.0f;

    public GameObject wavePrefab;

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
		return false;//true;
	}

	public void SpawnWave(Vector3 point, /*Vector3 direction, float spread, */float intensity, float radius)
    {
        GameObject newElement = Instantiate(wavePrefab) as GameObject;
        newElement.transform.localScale = new Vector3(1, 1, 1);
        //StartCoroutine(initWaveEnumerator(newElement.GetComponent<Wave>(), point, intensity, radius));
		newElement.GetComponent<Wave>().init(point, intensity, 0, radius); // TODO rimettere lo spread
    }

    //public IEnumerator initWaveEnumerator(Wave wave, Vector3 point, /*Vector3 direction, float spread, */float intensity, float radius)
    //{
   //     yield return null;
   //     
   // }
}
