using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour {

	public PlayerController player;
	public RectTransform coinPanel;
	public RectTransform lifePanel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GameController.Instance.isCameraFollowMode ()) {
			transform.position = new Vector3 (player.transform.position.x, transform.position.y, transform.position.z);
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
