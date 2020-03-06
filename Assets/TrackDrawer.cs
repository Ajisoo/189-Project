using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackDrawer : MonoBehaviour
{
    public TrackInterface track;
    public CarMovement car;
    public GameObject road;
    public GameObject sphere;

    private List<GameObject> spheres;

    public readonly float roadBehind = 25.0f;
    public readonly float roadAhead = 150.0f;
    public readonly float frequency = 0.002f;
    public readonly int flyInFreq = 5;
    private float flyInCounter;

    private LinkedList<GameObject> roadObjects;
    private LinkedList<float> distances;

    private float last_t;
    private int curve;
    private float distance_ahead;
    private float distance_behind;
    
    // Start is called before the first frame update
    void Start()
    {
        curve = 0;
        distance_ahead = 0;
        distance_behind = 0;
        flyInCounter = 0;
        spheres = new List<GameObject>();
        roadObjects = new LinkedList<GameObject>();
        distances = new LinkedList<float>();
        float t = 0;
        float distance = -roadBehind;
        float angle;
        Vector2 deriv;
        Vector2 old = track.GetPos(curve, t);
        while (distance < 0)
        {
            t -= frequency;
            Vector2 neW = track.GetPos(curve, t);
            distance += Vector2.Distance(old, neW);
            old = neW;
        }
        last_t = t;
        distance += roadBehind + roadAhead;
        old = track.GetPos(curve, t);
        while (distance > 0)
        {
            t += frequency;
            Vector2 neW = track.GetPos(curve, t);
            deriv = track.GetDeriv(curve, t);
            angle = (float)Mathf.Atan2(deriv.y, deriv.x);
            roadObjects.AddLast(Instantiate(road, new Vector3(neW.x, -0.122f, neW.y), Quaternion.AngleAxis(-angle * Mathf.Rad2Deg + 90, Vector3.up)));
            distance -= Vector2.Distance(old, neW);
            distances.AddLast(Vector2.Distance(old, neW));
            old = neW;
        }
        last_t = t - frequency;
    }

    // Update is called once per frame
    void Update()
    {
        flyInCounter += Time.deltaTime;
        distance_behind += car.speed * Time.deltaTime;
        distance_ahead += car.speed * Time.deltaTime;
        //Debug.Log(distance_behind + " " + distance_ahead);
        while (distance_behind > 0)
        {
            float distance = distances.First.Value;
            if (distance_behind - distance < 0) break;
            distance_behind -= distance;
            distances.RemoveFirst();
            Destroy(roadObjects.First.Value);
            roadObjects.RemoveFirst();
        }
        Vector2 old_pos = new Vector2(roadObjects.Last.Value.transform.position.x, roadObjects.Last.Value.transform.position.z);
        float t = last_t;
        //Debug.Log(last_t + " " + distance_ahead);
        while (distance_ahead > 0)
        {
            t += frequency;
            Vector2 neW = track.GetPos(curve, t);
            Vector2 deriv = track.GetDeriv(curve, t);
            float angle = (float)Mathf.Atan2(deriv.y, deriv.x);
            roadObjects.AddLast(Instantiate(road, new Vector3(neW.x, -0.122f, neW.y), Quaternion.AngleAxis(-angle * Mathf.Rad2Deg + 90, Vector3.up)));
            if (flyInCounter >= 1.0f/flyInFreq)
            {
                roadObjects.Last.Value.GetComponent<FlyIn>().offset = new Vector3(0, 10, 0);
                flyInCounter = 0;
            }
            roadObjects.Last.Value.GetComponent<FlyIn>().enabled = true;
            distance_ahead -= Vector2.Distance(old_pos, neW);
            distances.AddLast(Vector2.Distance(old_pos, neW));
            old_pos = neW;
        }
        last_t = t;

        //foreach (GameObject my_sphere in spheres)
        //{
        //    Destroy(my_sphere, 0);
        //}
        //spheres = new List<GameObject>();
        //for (int i = 0; i < track.trackGenerator.track.Length; i++)
        //{
        //    Vector3 pos = new Vector3(track.trackGenerator.track[i].x, 1, track.trackGenerator.track[i].y);
        //    spheres.Add(Instantiate(sphere, pos, Quaternion.identity));
        //}
    }
}
