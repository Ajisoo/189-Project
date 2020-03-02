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

    private readonly int numRoadBehind = 50;
    private readonly int numRoadAhead = 300;
    private readonly float frequency = 0.002f;

    private LinkedList<GameObject> roadObjects;

    private float last_t;
    
    // Start is called before the first frame update
    void Start()
    {
        spheres = new List<GameObject>();
        roadObjects = new LinkedList<GameObject>();
        last_t = 0;
        for (int i = -numRoadBehind; i < numRoadAhead; i++)
        {
            Vector2 position = track.GetPos(0, frequency * i);
            Vector2 deriv = track.GetDeriv(0, frequency * i, frequency);
            float angle = (float)Mathf.Atan2(deriv.y, deriv.x);

            roadObjects.AddLast(Instantiate(road, new Vector3(position.x, -0.122f, position.y), Quaternion.AngleAxis(-angle * Mathf.Rad2Deg + 90, Vector3.up)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        float t = car.t;
        for (float f = (int)(last_t / frequency + 1) * frequency; f < t; f += frequency)
        {
            Vector2 position = track.GetPos(0, f + numRoadAhead * frequency);
            Vector2 deriv = track.GetDeriv(0, f + numRoadAhead * frequency, frequency);
            float angle = (float)Mathf.Atan2(deriv.y, deriv.x);

            roadObjects.AddLast(Instantiate(road, new Vector3(position.x, -0.122f, position.y), Quaternion.AngleAxis(-angle * Mathf.Rad2Deg + 90, Vector3.up)));
            Destroy(roadObjects.First.Value);
            roadObjects.RemoveFirst();
        }

        foreach (GameObject my_sphere in spheres)
        {
            Destroy(my_sphere, 0);
        }
        spheres = new List<GameObject>();
        for (int i = 0; i < track.trackGenerator.track.Length; i++)
        {
            Vector3 pos = new Vector3(track.trackGenerator.track[i].x, 1, track.trackGenerator.track[i].y);
            spheres.Add(Instantiate(sphere, pos, Quaternion.identity));
        }
        last_t = t;
    }
}
