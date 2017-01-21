using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

	public float propagationSpeed = 1;
    public WaveDirectionEnum propagationDirection = WaveDirectionEnum.FORWARD;

    private float _radius = 0;
	public float radius { get { return _radius; } private set { _radius = value; } }

	private float _intensity;
	public float intensity { get { return _intensity; } private set { _intensity = value; } }

	private float _initialIntensity;
	private Vector3 _center;

	private float kappa { get { return GameController.Instance.wave_kappa; } }

	public void init (Vector3 center, float initialIntensity, float spread, float radius = 0,
        WaveDirectionEnum propagationDirection = WaveDirectionEnum.FORWARD)
	{
		_initialIntensity = initialIntensity;
		_intensity = initialIntensity;
		_radius = radius;
        this.propagationDirection = propagationDirection;
        _center = center;
		gameObject.transform.position = center;
		gameObject.transform.localScale = new Vector3(_radius, _radius, _radius);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		float deltaRadius = propagationSpeed * Time.deltaTime;
        if (propagationDirection == WaveDirectionEnum.FORWARD)
        {
            radius += deltaRadius;
        }
        else
        {
            radius -= deltaRadius;
        }
		gameObject.transform.localScale = new Vector3(_radius, _radius, _radius);
        _intensity = GameController.Instance.wave_kappa / radius;
        if (_intensity < GameController.Instance.min_force_intensity)
        {
            _intensity = 0;
            StartCoroutine(destroyThisObjectCoroutine());
        }
	}

    IEnumerator destroyThisObjectCoroutine ()
    {
        yield return null;
        DestroyObject(this);
    }

}

public enum WaveDirectionEnum 
{
	FORWARD,
	BACKWARD
}