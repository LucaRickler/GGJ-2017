using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

	public float propagationSpeed = 1;
    public WaveDirectionEnum propagationDirection = WaveDirectionEnum.FORWARD;

	private float _localRadius = 0;
	public float radius { get { return _localRadius + _initialRadius; } }

	private float _initialRadius;

	[SerializeField]
	private float _intensity;
	public float intensity { get { return _intensity; } private set { _intensity = value; } }

	private float _initialIntensity;
	private Vector3 _center;

	private float kappa { get { return GameController.Instance.wave_kappa; } }

    private bool initialized = false;

    private CircleCollider2D creatorSafeZone;
    private CircleCollider2D waveCollider;

    public void init (CircleCollider2D creatorSafeZone, Vector3 center, float initialIntensity, float spread, float radius = 0,
        WaveDirectionEnum propagationDirection = WaveDirectionEnum.FORWARD)
	{
		_initialIntensity = initialIntensity;
		_intensity = initialIntensity;
		_localRadius = 0.0f;
		_initialRadius = radius;
        this.propagationDirection = propagationDirection;
        _center = center;
		gameObject.transform.position = center;
		gameObject.transform.localScale = new Vector3(_localRadius, _localRadius, _localRadius);
        initialized = true;
        this.creatorSafeZone = creatorSafeZone;
    }

    // Use this for initialization
    void Start () {
        this.waveCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update () {
        if (initialized)
        {
            float deltaRadius = propagationSpeed * Time.deltaTime;
            if (propagationDirection == WaveDirectionEnum.FORWARD)
            {
                _localRadius += deltaRadius;
            }
            else
            {
                _localRadius -= deltaRadius;
                if (_localRadius <= 0.0f)
                {
                    StartCoroutine(destroyThisObjectCoroutine());
                    return;
                }
            }
            gameObject.transform.localScale = new Vector3(radius, radius, radius);
            _intensity = _initialIntensity - GameController.Instance.wave_kappa * _localRadius;
            if (_intensity < GameController.Instance.min_force_intensity)
            {
                _intensity = 0;
                StartCoroutine(destroyThisObjectCoroutine());
            }
        }
	}

    public bool isInCreatorSafeZone ()
    {
        return creatorSafeZone.radius <= waveCollider.radius;
    }

    public bool isCreator (CircleCollider2D creatorSafeZone)
    {
        return creatorSafeZone == this.creatorSafeZone;
    }

    IEnumerator destroyThisObjectCoroutine ()
    {
        yield return null;
		DestroyObject(gameObject);
    }

}

public enum WaveDirectionEnum 
{
	FORWARD,
	BACKWARD
}