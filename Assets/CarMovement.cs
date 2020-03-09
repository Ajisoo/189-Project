using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{

    public TrackInterface track;
    public float speed = 40.0f;
    public float distanceRemainingToTravel;
    public CameraSway cameraSway;
    public FadeBlack black;
    public Stopwatch stopwatch;

    public float spinOutRatio;
    public bool spinOut;
    bool spinLeft;
    public Vector3 spinOutDeriv;
    public float spinOutDistanceRatio;
    public float t;

    public int startFrames;
    public readonly float timeUntilReset = 2f;
    public float timeRemaining;

    public readonly float maxSpeed = 60;

    // Start is called before the first frame update
    void Start()
    {
        stopwatch.StartStopwatch();
        startFrames = 50;
        spinOutRatio = 2.0f;
        spinOut = false;
        spinOutDistanceRatio = 1.0f;
        t = 0;
        distanceRemainingToTravel = 0;
        Vector2 position = track.GetPos(0, t);
        transform.position = new Vector3(position.x, 0.291f, position.y);
    }

    // Update is called once per frame
    void Update()
    {
        startFrames--;
        distanceRemainingToTravel += Time.deltaTime * speed;
        if (t >= TrackGenerator.num_of_curves)
        {
            transform.position += spinOutDistanceRatio * distanceRemainingToTravel * Vector3.Normalize(new Vector3(spinOutDeriv.x, 0, spinOutDeriv.y));
            cameraSway.UpdateSelf();
            spinOutDistanceRatio *= 0.99f - 0.04f * speed / maxSpeed;
            distanceRemainingToTravel = 0;
            return;
        }
        if (spinOut)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0)
            {
                startFrames = 50;
                spinOut = false;
                distanceRemainingToTravel = 0;
                Vector2 position1 = track.GetPos(0, t);
                transform.position = new Vector3(position1.x, 0.291f, position1.y); 
                Vector2 deriv1 = track.GetDeriv(0, t);
                float angle1 = (float)Mathf.Atan2(deriv1.y, deriv1.x);
                transform.rotation = Quaternion.AngleAxis(-angle1 * Mathf.Rad2Deg + 90, Vector3.up);
                cameraSway.ResetCamera();
                black.Undo();
                speed = 10.0f;
            }
            transform.position += spinOutDistanceRatio * distanceRemainingToTravel * Vector3.Normalize(new Vector3(spinOutDeriv.x, 0, spinOutDeriv.y));
            transform.rotation = transform.rotation *= Quaternion.AngleAxis( (spinLeft ? -1 : 1) * 20f * spinOutDistanceRatio, Vector3.up);
            cameraSway.UpdateSelf();
            spinOutDistanceRatio *= 0.99f - 0.04f * speed / maxSpeed;
            distanceRemainingToTravel = 0;
            return;
        }
        speed += Time.deltaTime * 3;
        Vector2 old = track.GetPos(0, t);
        while (true)
        {
            Vector2 next = track.GetPos(0, t + 0.002f);
            if (distanceRemainingToTravel - Vector2.Distance(old, next) <= 0) break;
            distanceRemainingToTravel -= Vector3.Distance(old, next);
            old = next;
            t += 0.002f;
        }
        if (t >= TrackGenerator.num_of_curves)
        {
            stopwatch.StopStopwatch();
            spinOutDeriv = track.GetDeriv(0, t);
        }
        Vector2 position = track.GetPos(0, t);
        Vector2 deriv = track.GetDeriv(0, t);
        float angle = (float)Mathf.Atan2(deriv.y, deriv.x);

        if (startFrames < 0 && Quaternion.Angle(transform.rotation, Quaternion.AngleAxis(-angle * Mathf.Rad2Deg + 90, Vector3.up)) > spinOutRatio * (100 - speed) / 20)
        {
            black.start();
            spinOutDistanceRatio = 1.0f;
            timeRemaining = timeUntilReset;
            spinLeft = Quaternion.Angle(transform.rotation, Quaternion.AngleAxis(-angle * Mathf.Rad2Deg + 90 + 1f, Vector3.up)) < Quaternion.Angle(transform.rotation, Quaternion.AngleAxis(-angle * Mathf.Rad2Deg + 90, Vector3.up));
            spinOut = true;
            //Debug.Log(spinLeft);
            spinOutDeriv = track.GetDeriv(0, t);
            distanceRemainingToTravel -= Time.deltaTime * speed;
            Update();
            return;
        }

        transform.position = new Vector3(position.x, 0.291f, position.y);
        //if (Quaternion.Angle(transform.rotation, Quaternion.AngleAxis(-angle * Mathf.Rad2Deg + 90, Vector3.up)) > speed * ) ;
        //Debug.Log(Quaternion.Angle(transform.rotation, Quaternion.AngleAxis(-angle * Mathf.Rad2Deg + 90, Vector3.up)));
        transform.rotation = Quaternion.AngleAxis(-angle * Mathf.Rad2Deg + 90, Vector3.up);
        cameraSway.UpdateSelf();
    }
}
