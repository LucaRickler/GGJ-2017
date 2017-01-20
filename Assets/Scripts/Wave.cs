using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

	public float propagationSpeed = 1;
	public WaveDirectionEnum propagationDirection = WaveDirectionEnum.FORWARD;

	private float _radius = 0;
	public float radius { get { return _radius; } private set { _radius = value; } }

	private float _intesity;
	public float intensity { get { return _intesity; } private set { _intesity = value; } }

	private float _initialIntensity;
	private Vector3 _center;

	private float kappa { get { return GameController.Instance.wave_kappa; } }

	public void init (Vector3 center, float initialIntensity, float spread)
	{
		_initialIntensity = initialIntensity;
		_intesity = initialIntensity;
		_radius = 0;
		_center = center;
		gameObject.transform.position = center;
		gameObject.transform.localScale = new Vector3(_radius, _radius, _radius);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		radius += propagationSpeed * Time.deltaTime;
		gameObject.transform.localScale = new Vector3(_radius, _radius, _radius);
	}

	void OnTriggerEnter (Collider other)
	{
		Debug.Log (other.gameObject.name + ": OnCollisionEnter");
	}

	void OnTriggerStay (Collider other)
	{
		Debug.Log (other.name + ": OnCollisionSty");
	}

	void OnTriggerExit (Collider other)
	{
		Debug.Log (other.gameObject.name + ": OnCollisionExit");
	}

}

public enum WaveDirectionEnum 
{
	FORWARD,
	BACKWARD
}