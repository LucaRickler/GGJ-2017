using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

	public AudioClip[] clips = new AudioClip[(int)SFX.NUMBER_OF_SFX];

	//TODO: diminuzione volume su effetti sonori

	public AudioSource music_source;
	public AudioSource sfx_source;

	public enum SFX	{
		BOLLICINE, //post forziere, anzi spawn bomba
		CANNONE_GRANDE, //dopo soglia potenza
		CANNONE_PICCOLO, //normale, sotto soglia
		CIGOLIO_LEGNO, //apertura forziere
		CHECKPOINT, //mai, anzi meglio di bollicine
		COINS, //ovvio
		DEATH, //ovvio
		SIGLA_BOSS, //ovvio 
		LEVEL, //
		EARTHQUAKE, //prima del boss, caduta muro
		ESPLOSION, //ovvio
		NUMBER_OF_SFX
	};

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

		//DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlaySFX(SFX sound) {
		switch (sound) {
		case SFX.BOLLICINE:
			sfx_source.Stop ();
			sfx_source.clip = clips [(int)sound];
			sfx_source.Play ();
			break;
		case SFX.SIGLA_BOSS:
			music_source.Stop ();
			music_source.clip = clips [(int)sound];
			music_source.Play ();
			break;
		case SFX.CANNONE_GRANDE:
			sfx_source.Stop ();
			sfx_source.clip = clips [(int)sound];
			sfx_source.Play ();
			break;
		case SFX.CANNONE_PICCOLO:
			sfx_source.Stop ();
			sfx_source.clip = clips [(int)sound];
			//sfx_source.Play ();
			break;
		case SFX.CHECKPOINT:
			sfx_source.Stop ();
			sfx_source.clip = clips [(int)sound];
			sfx_source.Play ();
			break;
		case SFX.CIGOLIO_LEGNO:
			sfx_source.Stop ();
			sfx_source.clip = clips [(int)sound];
			sfx_source.Play ();
			break;
		case SFX.COINS:
			sfx_source.Stop ();
			sfx_source.clip = clips [(int)sound];
			sfx_source.Play ();
			break;
		case SFX.DEATH:
			sfx_source.Stop ();
			sfx_source.clip = clips [(int)sound];
			sfx_source.Play ();
			break;
		case SFX.LEVEL:
			music_source.Stop ();
			music_source.clip = clips [(int)sound];
			music_source.Play ();
			break;
		case SFX.EARTHQUAKE:
			sfx_source.Stop ();
			sfx_source.clip = clips [(int)sound];
			sfx_source.Play ();
			break;
		case SFX.ESPLOSION:
			sfx_source.Stop ();
			sfx_source.clip = clips [(int)sound];
			sfx_source.Play ();
			break;
		default:
			break;
		}
	}
}
