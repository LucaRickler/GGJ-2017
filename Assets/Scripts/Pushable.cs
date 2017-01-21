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

    public CircleCollider2D safeZoneCollider;

	public bool isPC;

    void Start() {
        gc = GameController.Instance;
        my_body = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        if (safeZoneCollider != null)
        {
            safeZoneCollider.enabled = false;
        }
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
        //my_body.AddForce(impulse);
		my_body.AddForce(impulse/100, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Vector3 waveCenter = otherCollider.gameObject.transform.position;
        Wave wave = otherCollider.gameObject.GetComponent<Wave>();
        if (!wave.isCreator(safeZoneCollider) || !wave.isInCreatorSafeZone())
        {
            Vector3 contactPoint = coll.bounds.ClosestPoint(waveCenter);
            bool serveCollision = true;
            serveCollision = getCollisionPoint((CircleCollider2D)otherCollider, out contactPoint);
            if (serveCollision)
            {
                //Debug.DrawLine(waveCenter, contactPoint, Color.red, 100);

                float absorbedIntensity = wave.intensity * absorbedFraction;

                if (absorbedIntensity > GameController.Instance.min_force_intensity)
                {
                    float reflectedIntensity = absorbedIntensity * reflectedFraction;
                    float impulseIntensity = absorbedIntensity - reflectedIntensity;
					if (isPC)
						impulseIntensity *= 3;
                    if (!Mathf.Approximately(reflectedIntensity, GameController.Instance.min_force_intensity) && wave.propagationDirection == WaveDirectionEnum.FORWARD)
                    {
                        if (wave.firstSpawn || Vector3.Distance(contactPoint, waveCenter) > GameController.Instance.minimumWaveCollisionDistance)
                        {
                            wave.duplicate(safeZoneCollider, contactPoint, reflectedIntensity);
                        }
                    }
                    if (!Mathf.Approximately(impulseIntensity, GameController.Instance.min_force_intensity))
                    {
                        Vector2 inpulseDirection = (contactPoint - waveCenter).normalized;
                        if (wave.propagationDirection == WaveDirectionEnum.BACKWARD)
                            inpulseDirection = inpulseDirection * (-1);
                        ApplyForce(inpulseDirection * impulseIntensity);
                    }
                }
            }
        }
    }

    private bool getCollisionPoint (CircleCollider2D otherCollider, out Vector3 point)
    {
        Vector3 otherColliderCenter = otherCollider.gameObject.transform.position;
        Vector3 fromOtherToThisObjectVector = gameObject.transform.position - otherColliderCenter;
        Wave wave = otherCollider.gameObject.GetComponent<Wave>();
        //Debug.DrawLine(otherColliderCenter, fromOtherToThisObjectVector, Color.red, 100);
        Quaternion rot = Quaternion.AngleAxis(gameObject.transform.rotation.eulerAngles.z, Vector3.forward);
        //Quaternion rot = Quaternion.identity; // da modificare per girare i collider
        Vector3[] axis = new Vector3[]
        {
            rot * Vector3.up,
            rot * Vector3.down,
            rot * Vector3.right,
            rot * Vector3.left
        };
        float[] bestAngles = new float[] { 360, 360 };
        Vector3[] bestDirections = new Vector3[] { Vector3.zero, Vector3.zero };
        foreach (Vector3 dir in axis)
        {
            float a = angleBetween(dir, fromOtherToThisObjectVector);
            if (a < bestAngles[0])
            {
                bestAngles[1] = bestAngles[0];
                bestDirections[1] = bestDirections[0];
                bestAngles[0] = a;
                bestDirections[0] = dir;
            }
            else if (a < bestAngles[1])
            {
                bestAngles[1] = a;
                bestDirections[1] = dir;
            }
        }
        RaycastHit2D myCollision = new RaycastHit2D();
        bool found = false;
        //Debug.DrawLine(Vector3.zero, bestDirections[0], Color.red, 100);
        //Debug.DrawLine(Vector3.zero, bestDirections[1], Color.yellow, 100);
        for (int i=0; i<2; i++)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(otherColliderCenter, bestDirections[i]);
            float minDistance = float.PositiveInfinity;
            foreach (RaycastHit2D h in hits)
            {
                if (h.collider == this.coll)
                {
                    float dist = Vector3.Distance(h.point, otherColliderCenter);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        myCollision = h;
                        found = true;
                    }
                }
            }
            if (found)
                break;
        }
        if (!found)
        {
            //Debug.Log("Collision point not found.");
            point = Vector3.zero;
            return false;
        }
        else
        {
            point = myCollision.point;
            Debug.DrawLine(otherColliderCenter, point, Color.yellow, 100);
            return true;
        }
    }

    private float angleBetween (Vector3 a, Vector3 b)
    {
        Quaternion aRot = Quaternion.FromToRotation(Vector3.right, a);
        Quaternion bRot = Quaternion.FromToRotation(Vector3.right, b);
        float angle = Quaternion.Angle(aRot, bRot);
        if (angle < 0)
        {
            angle *= -1;
        }
        while (angle > 360)
        {
            angle -= 360;
        }
        if (angle > 180)
        {
            angle = 360 - angle;
        }
        return angle;
    }

    private IEnumerator destroyThisObject()
    {
        yield return null;
        DestroyObject(this.gameObject);
    } 

}
