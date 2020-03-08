using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSway : MonoBehaviour
{
    private Vector3 speed;
    private Vector3 local_local_pos;
    private LinkedList<Vector3> old_pos;
    public CarMovement car;
    private int size = 50;
    private float weight = 0.01f;
    private Quaternion originalRot;
    // Start is called before the first frame update
    void Start()
    {
        originalRot = transform.rotation;
        //old_pos = new LinkedList<Vector3>();
        speed = new Vector3(0, 0, 0);
        local_local_pos = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    public void UpdateSelf()
    {
        if (car.spinOut)
        {
            transform.position += car.spinOutDistanceRatio * car.distanceRemainingToTravel * Vector3.Normalize(new Vector3(car.spinOutDeriv.x, 0, car.spinOutDeriv.y)) * 0.9f;
            return;
        }
        if (old_pos == null)
        {
            old_pos = new LinkedList<Vector3>();
            return;
        }
        if (old_pos.Count < size)
        {
            transform.position = (car.transform.position + car.transform.rotation * new Vector3(0, 1.8f, -4f));
            transform.rotation = car.transform.rotation * originalRot;
            if (old_pos.Count > 0)
            {
                speed += (transform.position - old_pos.Last.Value);
            }
            old_pos.AddLast(transform.position);
            return;
        }
        transform.position = (old_pos.Last.Value + speed) * weight + (car.transform.position + car.transform.rotation * new Vector3(0, 1.8f, -4f)) * (1 - weight);
        transform.rotation = car.transform.rotation * originalRot;

        Vector3 pos = transform.position;
        speed += (pos - old_pos.Last.Value);
        speed *= 0.999f;
        old_pos.AddLast(pos);
        old_pos.RemoveFirst();
    }

    public void ResetCamera()
    {
        transform.position = car.transform.position + car.transform.rotation * new Vector3(0, 1.8f, -4f);
        transform.rotation = car.transform.rotation * originalRot;
    }
}
