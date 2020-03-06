using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSway : MonoBehaviour
{
    private Vector3 speed;
    private Vector3 local_local_pos;
    private LinkedList<Vector3> old_pos;
    private int size = 5;
    // Start is called before the first frame update
    void Start()
    {
        old_pos = new LinkedList<Vector3>();
        speed = new Vector3(0, 0, 0);
        local_local_pos = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (old_pos.Count < size)
        {
            if (old_pos.Count > 0)
            {
                speed += (transform.position - old_pos.Last.Value) / (size - 1);
            }
            old_pos.AddLast(transform.position);
            return;
        }
        Vector3 pos = transform.position;


        local_local_pos += Quaternion.Inverse(transform.parent.transform.rotation) * (speed - (pos - old_pos.Last.Value)) * 0.01f;
        local_local_pos *= 0.95f;

        speed += (pos - old_pos.Last.Value) / (size - 1);
        speed -= (old_pos.First.Next.Value - old_pos.First.Value) / 5;
        old_pos.AddLast(pos);
        old_pos.RemoveFirst();

        transform.localPosition = local_local_pos + new Vector3(0, 1.8f, -4f);
    }
}
