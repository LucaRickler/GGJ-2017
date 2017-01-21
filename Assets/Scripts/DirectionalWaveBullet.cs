using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalWaveBullet : MonoBehaviour
{
    private Vector3 _startPosition;
    public Vector3 startPosition { get { return _startPosition; } }

    private Vector3 _direction;
    public Vector3 direction { get { return _direction; } }

    [SerializeField]
    private float _speed;
    public float speed { get { return _speed; } }

    private Collision2D _endCollision;
    public Collision2D endCollision { get { return _endCollision; } }

    private GameObject _bulletSpawner;
    public GameObject bulletSpawner { get { return _bulletSpawner; } }

    private Collider2D _waveSpawner;
    public Collider2D waveSpawner { get { return _waveSpawner; } }

    public bool hasEnded { get { return _endCollision == null; } }

    private System.Action<DirectionalWaveBullet> onBulletArrived;

    public void init(Vector3 startPosition, Vector3 direction, float speed, GameObject bulletSpawner,
        Collider2D waveSpawner, System.Action<DirectionalWaveBullet> onBulletArrived )
    {
        _startPosition = startPosition;
        _direction = direction.normalized;
        _speed = speed;
        _endCollision = null;
        _bulletSpawner = bulletSpawner;
        _waveSpawner = waveSpawner;
        this.onBulletArrived = onBulletArrived;
    }

    // Use this for initialization
	void Start () {
        transform.position = _startPosition;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += direction * speed * Time.deltaTime;
	}

    void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.collider != waveSpawner)
        {
            _endCollision = collision;
            if (this.onBulletArrived != null)
                this.onBulletArrived(this);
        }
    }

}
