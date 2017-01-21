using UnityEngine;
using System.Collections;

public class Pushable : MonoBehaviour {

    private Rigidbody2D my_body;
    private GameController gc;
    private Collider2D coll;

    [Range(0, 1)]
    public float absorbedFraction;
    [Range(0, 1)]
    public float reflectedFraction; // this is a fraction of absorbedIntensiy

    public float hitPoints;

    void Start() {
        gc = GameController.Instance;
        my_body = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    void FixedUpdate() {
        var speed = my_body.velocity;
        if (speed.x > gc.max_speed)
            speed.x = gc.max_speed;
        else if (speed.x < -gc.max_speed)
            speed.x = -gc.max_speed;
        if (speed.y > gc.max_speed)
            speed.y = gc.max_speed;
        else if (speed.y < -gc.max_speed)
            speed.y = -gc.max_speed;
        my_body.velocity = speed;
    }

    public void ApplyForce(Vector2 impulse) {
        my_body.AddForce(impulse);
    }
		
    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Vector3 waveCenter = otherCollider.gameObject.transform.position;
        Vector3 contactPoint = coll.bounds.ClosestPoint(waveCenter);
        Debug.DrawLine(waveCenter, contactPoint);
        Wave wave = otherCollider.gameObject.GetComponent<Wave>();

        float absorbedIntensity = wave.intensity * absorbedFraction;

        if (absorbedIntensity > GameController.Instance.min_force_intensity)
        {
            float reflectedIntensity = absorbedIntensity * reflectedFraction;
            float impulseIntensity = absorbedIntensity - reflectedIntensity;
            if (!Mathf.Approximately(reflectedIntensity, 0) && wave.propagationDirection == WaveDirectionEnum.FORWARD)
            {
                GameController.Instance.SpawnWave(contactPoint, reflectedIntensity, 0.0f);
            }
            if (!Mathf.Approximately(impulseIntensity, 0))
            {
                Vector2 inpulseDirection = (contactPoint - waveCenter).normalized;
                if (wave.propagationDirection == WaveDirectionEnum.BACKWARD)
                    inpulseDirection = inpulseDirection * (-1);
                ApplyForce(inpulseDirection * impulseIntensity);
            }
        }
    }

    private IEnumerator destroyThisObject()
    {
        yield return null;
        DestroyObject(this.gameObject);
    } 

}
