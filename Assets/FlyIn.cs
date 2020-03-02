using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyIn : MonoBehaviour
{

    public Vector3 offset;
    private Vector3 origPos;
    public Quaternion rotation;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 1f;
        origPos = transform.position;
        transform.position = origPos + offset;
    }

    // Update is called once per frame
    void Update()
    {
        speed *= 0.95f;
        if (speed < 0.0001f)
        {
            speed = 0f;
            transform.position = origPos;
        }
        //transform.rotation *= Quaternion.AngleAxis(3.5f * speed * Mathf.PI * 1.71f, Vector3.up);
        transform.rotation *= Quaternion.AngleAxis(speed * 360 / 19 * 5, Vector3.up);
        transform.position = origPos + offset * speed;
    }
}
