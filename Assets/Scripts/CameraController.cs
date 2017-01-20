using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public PlayerController player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GameController.Instance.isCameraFollowMode ()) {
			transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);
		}
	}
}
