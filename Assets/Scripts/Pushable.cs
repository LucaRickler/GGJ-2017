using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        bool serveCollision = true;
        //serveCollision = getCollisionPoint((CircleCollider2D)otherCollider, out contactPoint);
        if (serveCollision)
        {
            Debug.DrawLine(waveCenter, contactPoint);
            Debug.Log(contactPoint);
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
    }

    private bool getCollisionPoint (CircleCollider2D otherCollider, out Vector3 point)
    {
        float alpha1 = Mathf.Tan(- transform.rotation.z * Mathf.Deg2Rad);
        float alpha2 = Mathf.Tan(-Mathf.PI + transform.rotation.z * Mathf.Deg2Rad);
        Vector3 otherColliderCenter = otherCollider.gameObject.transform.position + otherCollider.bounds.center;
        Quaternion rot = Quaternion.AngleAxis(transform.rotation.z, Vector3.forward);
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        float expectedDistance = otherCollider.radius * otherCollider.gameObject.transform.lossyScale.x;
        hits.AddRange(Physics2D.RaycastAll(otherColliderCenter, rot * Vector3.right, expectedDistance + 1));
        hits.AddRange(Physics2D.RaycastAll(otherColliderCenter, rot * Vector3.up, expectedDistance + 1));
        hits.AddRange(Physics2D.RaycastAll(otherColliderCenter, rot * Vector3.left, expectedDistance + 1));
        hits.AddRange(Physics2D.RaycastAll(otherColliderCenter, rot * Vector3.down, expectedDistance + 1));
        RaycastHit2D myCollision = new RaycastHit2D();
        bool found = false;
        foreach (RaycastHit2D h in hits)
        {
            if (h.collider == this.coll)
            {
                float dist = Vector3.Distance(h.point, otherColliderCenter);
                if (Mathf.Abs(dist - expectedDistance) < 0.3f)
                {
                    myCollision = h;
                    found = true;
                    break;
                }
            }
        }
        if (!found)
        {
            Debug.Log("Collision point not found.");
            point = myCollision.point;
            return false;
        }
        else
        {
            point = myCollision.point;
            return true;
        }
    }

    private IEnumerator destroyThisObject()
    {
        yield return null;
        DestroyObject(this.gameObject);
    } 

}
