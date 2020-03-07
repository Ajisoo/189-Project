using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{

    public TrackInterface track;
    public float speed = 40.0f;
    private float distanceRemainingToTravel;
    public CameraSway cameraSway;


    public float t;

    // Start is called before the first frame update
    void Start()
    {
        autoDriving = false;
        t = 0;
        distanceRemainingToTravel = 0;
        aimT = lookAheadT;
        Vector2 position = track.GetPos(0, t);
        transform.position = new Vector3(position.x, 0.291f, position.y);
    }

    // Update is called once per frame
    void AUpdate()
    {
        distanceRemainingToTravel += Time.deltaTime * speed;
        Vector2 old = track.GetPos(0, t);
        while (true)
        {
            Vector2 next = track.GetPos(0, t + 0.002f);
            if (distanceRemainingToTravel - Vector2.Distance(old, next) <= 0) break;
            distanceRemainingToTravel -= Vector3.Distance(old, next);
            old = next;
            t += 0.002f;
        }
        Vector2 position = track.GetPos(0, t);
        Vector2 deriv = track.GetDeriv(0, t);

        transform.position = new Vector3(position.x, 0.291f, position.y);

        float angle = (float)Mathf.Atan2(deriv.y, deriv.x);
        transform.rotation = Quaternion.AngleAxis(-angle * Mathf.Rad2Deg + 90, Vector3.up);
        cameraSway.UpdateSelf();
    }

    public float aimT;

    bool autoDriving;
    float angleCoeff = 0.000005f;
    float angleBase = 0.9999f;
    public readonly float lookAheadT = 0.01f;

    void Update()
    {
        distanceRemainingToTravel += Time.deltaTime * speed;
        float distanceToMove = distanceRemainingToTravel;
        Vector2 old = track.GetPos(0, aimT);
        while (true)
        {
            Vector2 next = track.GetPos(0, aimT + 0.002f);
            if (distanceRemainingToTravel - Vector2.Distance(old, next) <= 0) break;
            distanceRemainingToTravel -= Vector3.Distance(old, next);
            old = next;
            aimT += 0.002f;
        }
        Vector2 position = track.GetPos(0, aimT);
        Debug.Log(position + " " + transform.position);

        Vector3 deriv = new Vector3(position.x - transform.position.x, 0, position.y - transform.position.z);
        Vector3 normalized = Vector3.Normalize(deriv);
        Debug.Log(deriv);

        transform.position += new Vector3(deriv.x < 0 ? Mathf.Max(deriv.x, normalized.x * distanceToMove) : Mathf.Min(deriv.x, normalized.x * distanceToMove), 0, 
            deriv.z < 0 ? Mathf.Max(deriv.z, normalized.z * distanceToMove) : Mathf.Min(deriv.z, normalized.z * distanceToMove));
        //transform.position += new Vector3(normalized.x * distanceToMove, 0, normalized.z * distanceToMove);

        float angle = (float)Mathf.Atan2(deriv.z, deriv.x);
        Debug.Log(angle);
        if (deriv != Vector3.zero) transform.rotation = Quaternion.AngleAxis(-angle * Mathf.Rad2Deg + 90, Vector3.up);
        cameraSway.UpdateSelf();

    }
}
