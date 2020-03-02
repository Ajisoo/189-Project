using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackInterface : MonoBehaviour
{
    public TrackGenerator trackGenerator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetTrackGenerator(TrackGenerator generator)
    {
        trackGenerator = generator;
    }

    public Vector2 GetPos(int curve, float offset)
    {
        if (offset < 0)
        {
            while (offset < 0)
            {
                offset += 1;
                curve -= 1;
            }
        }
        else
        {
            curve += (int)offset;
            offset %= 1;
        }
        curve %= trackGenerator.num_of_curves;
        if (curve < 0) curve += trackGenerator.num_of_curves;
        Vector2 r0 = (1 - offset) * trackGenerator.track[(3 * curve + 0) % trackGenerator.track.Length] + offset * trackGenerator.track[(3 * curve + 1) % trackGenerator.track.Length];
        Vector2 r1 = (1 - offset) * trackGenerator.track[(3 * curve + 1) % trackGenerator.track.Length] + offset * trackGenerator.track[(3 * curve + 2) % trackGenerator.track.Length];
        Vector2 r2 = (1 - offset) * trackGenerator.track[(3 * curve + 2) % trackGenerator.track.Length] + offset * trackGenerator.track[(3 * curve + 3) % trackGenerator.track.Length];

        Vector2 q0 = (1 - offset) * r0 + offset * r1;
        Vector2 q1 = (1 - offset) * r1 + offset * r2;

        return (1 - offset) * q0 + offset * q1;
    }

    public Vector2 GetDeriv(int curve, float offset, float precision = 0.01f)
    {
        if (offset < 0)
        {
            while (offset < 0)
            {
                offset += 1;
                curve -= 1;
            }
        }
        else
        {
            curve += (int)offset;
            offset %= 1;
        }
        curve %= trackGenerator.num_of_curves;
        if (curve < 0) curve += trackGenerator.num_of_curves;
        Vector2 r0 = (1 - offset) * trackGenerator.track[(3 * curve + 0) % trackGenerator.track.Length] + offset * trackGenerator.track[(3 * curve + 1) % trackGenerator.track.Length];
        Vector2 r1 = (1 - offset) * trackGenerator.track[(3 * curve + 1) % trackGenerator.track.Length] + offset * trackGenerator.track[(3 * curve + 2) % trackGenerator.track.Length];
        Vector2 r2 = (1 - offset) * trackGenerator.track[(3 * curve + 2) % trackGenerator.track.Length] + offset * trackGenerator.track[(3 * curve + 3) % trackGenerator.track.Length];

        Vector2 q0 = (1 - offset) * r0 + offset * r1;
        Vector2 q1 = (1 - offset) * r1 + offset * r2;

        Vector2 start = (1 - offset) * q0 + offset * q1;

        offset += precision;
        curve += (int)offset;
        offset %= 1;
        curve %= trackGenerator.num_of_curves;
        if (curve < 0) curve += trackGenerator.num_of_curves;
        r0 = (1 - offset) * trackGenerator.track[(3 * curve + 0) % trackGenerator.track.Length] + offset * trackGenerator.track[(3 * curve + 1) % trackGenerator.track.Length];
        r1 = (1 - offset) * trackGenerator.track[(3 * curve + 1) % trackGenerator.track.Length] + offset * trackGenerator.track[(3 * curve + 2) % trackGenerator.track.Length];
        r2 = (1 - offset) * trackGenerator.track[(3 * curve + 2) % trackGenerator.track.Length] + offset * trackGenerator.track[(3 * curve + 3) % trackGenerator.track.Length];

        q0 = (1 - offset) * r0 + offset * r1;
        q1 = (1 - offset) * r1 + offset * r2;

        return ((1 - offset) * q0 + offset * q1) - start;
    }
}
