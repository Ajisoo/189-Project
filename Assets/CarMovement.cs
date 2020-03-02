using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{

    public TrackInterface track;
    public float t;

    // Start is called before the first frame update
    void Start()
    {
        t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        t += 0.002f;
        Vector2 position = track.GetPos(0, t);
        Vector2 deriv = track.GetDeriv(0, t);

        transform.position = new Vector3(position.x, 0.291f, position.y);

        float angle = (float)Mathf.Atan2(deriv.y, deriv.x);
        transform.rotation = Quaternion.AngleAxis(-angle * Mathf.Rad2Deg + 90, Vector3.up);
    }
}
