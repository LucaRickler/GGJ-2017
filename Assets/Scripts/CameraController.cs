using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour {

	public PlayerController player;
	public RectTransform coinPanel;
	public RectTransform lifePanel;

	public GameObject background1;
	public GameObject background2;

	private bool vertical_move = false;

//	public bool boss_started = false;

	public Vector3 first_waypoint;
	public Vector3 second_waypoint;
	public Vector3 third_waypoint;
	// Use this for initialization
	void Start () {
		vertical_move = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameController.Instance.isCameraFollowMode ()) {
			if(vertical_move)
				transform.position = new Vector3 (transform.position.x, player.transform.position.y <= 0 ? player.transform.position.y : 0, transform.position.z);
			else
				transform.position = new Vector3 (player.transform.position.x + 1 >= 0 ? player.transform.position.x + 1 : 0, transform.position.y, transform.position.z);
		}

		if (transform.position.x >= first_waypoint.x & !vertical_move)
			vertical_move = true;
		if (transform.position.y <= second_waypoint.y & vertical_move)
			vertical_move = false;
		if (transform.position.x > third_waypoint.x)
			transform.position = third_waypoint;
	}

	public void SetCameraPosition (Vector3 position) {
		transform.position = position;
	}

	public void FreeScreen (bool state) {
		StartCoroutine ("MoveUI", state);
	}

	IEnumerator MoveUI (bool state) {
		yield return null;
	}
}
