using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public PlayerController player;

    public float wave_kappa = 0.001f;
    public float key_input_force = 500;
    public float max_speed = 500;
    public float min_force_intensity = 5f;
    public float minimumWaveCollisionDistance = 0.1f;
    [Range(0, 90)]
    public float spread = 30;
    [Range(5, 100)]
    public float maximumWaveRadius = 10;

    public GameObject wavePrefab;

    public Text coinCounter;
    public Text lifeCounter;

    public int coins;
    [Range(0, 10)]
    public int playerLifes;

    public bool cameraCentered;

    public Camera sceneCamera;

    // Static singleton property
    private static GameController instance;


    //----------------------------------------------------------------//

    public static GameController Instance {
        get {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<GameController>();
                //DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    //----------------------------------------------------------------//

    void Awake() {
        instance = GameObject.FindObjectOfType<GameController>();
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
        coins = 0;
        Respawn();
        coinCounter.text = coins.ToString();
    }

    // Update is called once per frame
    void Update() {

    }

    public bool isCameraFollowMode() {
        return cameraCentered;
    }

    public void SpawnDiretionalWave(CircleCollider2D creatorSafeZone, Vector3 point, Vector3 direction, float intensity, float spread, float radius, WaveDirectionEnum propagationDirection, bool firstSpawn)
    {
        GameObject newElement = Instantiate(wavePrefab) as GameObject;
        newElement.transform.localScale = new Vector3(1, 1, 1);
        Wave w = newElement.GetComponent<Wave>();
        w.initDirectionalWave(creatorSafeZone, point, direction, intensity, radius, propagationDirection, firstSpawn);
    }

    public void SpawnSphericWave (CircleCollider2D creatorSafeZone, Vector3 point, float intensity, float radius, WaveDirectionEnum propagationDirection, bool firstSpawn)
    {
        GameObject newElement = Instantiate(wavePrefab) as GameObject;
        newElement.transform.localScale = new Vector3(1, 1, 1);
        newElement.GetComponent<Wave>().initSphericWave(creatorSafeZone, point, intensity, radius, propagationDirection, firstSpawn);
    }


    public void CollectCoin () {
		coins++;
		coinCounter.text = coins.ToString ();
	}

	public void Respawn () {
		lifeCounter.text = "x " + playerLifes.ToString ();
		//TODO: respawn del giocatore;
	}
   
}
