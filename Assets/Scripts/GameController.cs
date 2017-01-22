using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public PlayerController player;

	public Vector3 respanPoint;

	public GameObject EndScreen;
	public GameObject gameOverWrite;
	public GameObject gameWinWrite;

    public float wave_kappa = 0.001f;
    public float key_input_force = 500;
    public float max_speed = 500;
    public float min_force_intensity = 5f;
    public float minimumWaveCollisionDistance = 0.1f;
    [Range(0, 90)]
    public float spread = 30;
    [Range(5, 100)]
    public float maximumWaveRadius = 10;

	public float intensityToDamage = 1.0f;

    public GameObject wavePrefab;

    public Text coinCounter;
    public Text lifeCounter;

    public int coins;
   // [Range(0, 10)]
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

        //DontDestroyOnLoad(gameObject);
        coins = 0;
		playerLifes = 5;
        Respawn();
        coinCounter.text = coins.ToString();
		AudioController.Instance.PlaySFX (AudioController.SFX.LEVEL);
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
		sceneCamera.transform.position = new Vector3(respanPoint.x, respanPoint.y, -10);
		sceneCamera.GetComponent<CameraController> ().vertical_move = false;
		lifeCounter.text = "x " + playerLifes.ToString ();
		player.transform.position = respanPoint;
		player.transform.rotation = Quaternion.identity;
		player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;

		//TODO: respawn del giocatore;
	}

	public void PlayerDeath () {
		playerLifes -= 1;
		if (playerLifes <= 0)
			GameOver ();
		else
			Respawn ();
	}

	IEnumerator DiePlayerDie() {
		OpenEndScreen ();
		AudioController.Instance.PlaySFX (AudioController.SFX.DEATH);
		gameOverWrite.SetActive (true);
		yield return null;
	}

	void OpenEndScreen() {
		EndScreen.SetActive (true);
		player.input_ready = false;
		//time?
	}

	public void EndGame() {
		OpenEndScreen ();
		gameWinWrite.SetActive (true);
		AudioController.Instance.PlaySFX (AudioController.SFX.WIN);
	}

	public void GameOver () {
		StartCoroutine ("DiePlayerDie");
	}

	public void Retry() {
		EndScreen.SetActive (false);
		SceneManager.LoadScene ("DIOCANE");
	}

	public void Exit() {
		Application.Quit ();
	}
   
}
