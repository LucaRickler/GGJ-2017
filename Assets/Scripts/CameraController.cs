using UnityEngine;
using System.Collections;
using System;

public enum LevelSegment {
	FIRST,
	SECON,
	THIRD
}

public class CameraController : MonoBehaviour {

	public LevelSegment current_segment;

	public PlayerController player;
	public RectTransform coinPanel;
	public RectTransform lifePanel;

	public GameObject background1;
	public GameObject background2;

	public bool vertical_move = false;

//	public bool boss_started = false;

	public Vector3 first_waypoint;
	public Vector3 second_waypoint;
	public Vector3 third_waypoint;
	// Use this for initialization

	public GameObject overblock;
	public GameObject leftblock;
	void Start () {
		vertical_move = false;
		current_segment = LevelSegment.FIRST;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameController.Instance.isCameraFollowMode ()) {
			if(vertical_move)
				transform.position = new Vector3 (second_waypoint.x, player.transform.position.y <= 0 ? player.transform.position.y : 0, transform.position.z);
			else
				transform.position = new Vector3 (player.transform.position.x + 1 >= 0 ? player.transform.position.x + 1 : 0, transform.position.y, transform.position.z);
		}

		if (transform.position.x >= first_waypoint.x - 10 & !vertical_move) {
			vertical_move = true;
			leftblock.SetActive (true);
			current_segment = LevelSegment.SECON;
		}
		if (transform.position.y <= second_waypoint.y & vertical_move) {
			vertical_move = false;
			overblock.SetActive (true);
			current_segment = LevelSegment.THIRD;
		}
		if (transform.position.x > third_waypoint.x)
			GameController.Instance.EndGame ();
			//transform.position = third_waypoint;
		if (current_segment == LevelSegment.FIRST) {
			overblock.SetActive (false);
			leftblock.SetActive (false);
		} else if (current_segment == LevelSegment.SECON) {
			overblock.SetActive (false);
		}
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
