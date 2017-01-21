using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

    public float propagationSpeed = 1;

    public GameObject directionalWaveBulletPrefab;

    private GameObject directionalWaveBullet;

    public WaveDirectionEnum propagationDirection = WaveDirectionEnum.FORWARD;

	[Range(5, 30)]
	public float maximumWaveRadius = 100;

    private float _localRadius = 0;
    public float radius { get { return _localRadius + _initialRadius; } }

    private float _spread;
    public float spread { get { return _spread; } }

    private float _initialRadius;

    private float _intensity;
    public float intensity { get { return _intensity; } private set { _intensity = value; } }

    private float _initialIntensity;
    private Vector3 _center;

    private Vector2 _direction;
    public Vector2 direction { get { return _direction; } }

    private float kappa { get { return GameController.Instance.wave_kappa; } }

    private bool initialized = false;

    private CircleCollider2D creatorSafeZone;
    private CircleCollider2D waveCollider;

    private bool _firstSpawn;
    public bool firstSpawn { get{ return _firstSpawn; } }

    private WaveType _waveType;
    public WaveType waveType { get { return _waveType; } }

    public void duplicate (CircleCollider2D creatorSafeZone, Vector3 center, float initialIntensity)
    {
        if (waveType == WaveType.DIRETIONAL)
        {
            GameController.Instance.SpawnDiretionalWave(null, center, direction * (-1), initialIntensity, spread, 0.5f, propagationDirection, false);
        }
        else
        {
            GameController.Instance.SpawnSphericWave(null, center, initialIntensity, 0, propagationDirection, false);
        }
    }

    private void init (WaveType type, CircleCollider2D creatorSafeZone, Vector3 center, Vector3 direction, float initialIntensity, float spread, float radius,
        WaveDirectionEnum propagationDirection, bool firstSpawn)
	{
        _waveType = type;
		_initialIntensity = initialIntensity;
		_intensity = initialIntensity;
		_localRadius = 0.0f;
		_initialRadius = radius;
        _spread = spread;
        this.propagationDirection = propagationDirection;
        _center = center;
        _direction = direction;
        _firstSpawn = firstSpawn;
		gameObject.transform.position = center;
		gameObject.transform.localScale = new Vector3(_localRadius, _localRadius, _localRadius);
        initialized = true;
        this.creatorSafeZone = creatorSafeZone;
    }

    public void initDirectionalWave (CircleCollider2D creatorSafeZone, Vector3 center, Vector3 direction, float initialIntensity, float radius, WaveDirectionEnum propagationDirection, bool firstSpawn)
    {
        init(WaveType.DIRETIONAL, creatorSafeZone, center, direction.normalized, initialIntensity, GameController.Instance.spread, radius, propagationDirection, firstSpawn);
        //Debug.DrawLine(center, center + 3*direction, Color.red, 100);
        GetComponent<CircleCollider2D>().enabled = false;
        directionalWaveBullet = Instantiate(directionalWaveBulletPrefab);
        DirectionalWaveBullet directionalWaveBulletScript = directionalWaveBullet.GetComponent<DirectionalWaveBullet>();
        Collider2D safeZoneTemp = null;
        if (creatorSafeZone != null)
            safeZoneTemp = creatorSafeZone.gameObject.transform.parent.gameObject.GetComponent<Collider2D>();
        directionalWaveBulletScript.init(center + direction * radius, direction, propagationSpeed/10, gameObject, safeZoneTemp, onBulletArrived);
    }

    public void initSphericWave(CircleCollider2D creatorSafeZone, Vector3 center, float initialIntensity, float radius,
        WaveDirectionEnum propagationDirection, bool firstSpawn)
    {
        init(WaveType.SPHERIC, creatorSafeZone, center, Vector3.zero, initialIntensity, 0, radius, propagationDirection, firstSpawn);
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
                if (_localRadius > maximumWaveRadius)
                {
                    StartCoroutine(destroyThisObjectCoroutine());
                    return;
                }
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
        return creatorSafeZone != null && creatorSafeZone.radius >= waveCollider.radius;
    }

    public bool isCreator (CircleCollider2D creatorSafeZone)
    {
        return creatorSafeZone != null && creatorSafeZone == this.creatorSafeZone;
    }

    IEnumerator destroyThisObjectCoroutine ()
    {
        yield return null;
        yield return null;
        DestroyObject(gameObject);
        if (directionalWaveBullet != null)
            Destroy(directionalWaveBullet.gameObject);
            DestroyObject(gameObject);
    }

    public void onBulletArrived (DirectionalWaveBullet bullet)
    {
        Pushable pushable = bullet.endCollision.collider.gameObject.GetComponent<Pushable>();
        if (pushable != null)
        {
            Vector2 contactPointAVG = Vector2.zero;
            StartCoroutine(destroyThisObjectCoroutine());
            //foreach (ContactPoint2D p in bullet.endCollision.contacts)
            //    contactPointAVG += p.point / bullet.endCollision.contacts.Length;
            //pushable.serveCollision(this, contactPointAVG, gameObject.transform.position);
            pushable.serveCollision(this, bullet.endCollision.contacts[0].point, gameObject.transform.position);
        }
    }

}

public enum WaveDirectionEnum 
{
	FORWARD,
	BACKWARD
}

public enum WaveType
{
    DIRETIONAL,
    SPHERIC
}