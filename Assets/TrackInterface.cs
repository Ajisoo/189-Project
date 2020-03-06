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

        Vector4 coeff = new Vector4(offset * offset * offset, 3 * offset * offset * (1 - offset), 3 * offset * (1 - offset) * (1 - offset), (1 - offset) * (1 - offset) * (1 - offset));

        Vector2 p0 = trackGenerator.track[(3 * curve + 0) % trackGenerator.track.Length];
        Vector2 p1 = trackGenerator.track[(3 * curve + 1) % trackGenerator.track.Length];
        Vector2 p2 = trackGenerator.track[(3 * curve + 2) % trackGenerator.track.Length];
        Vector2 p3 = trackGenerator.track[(3 * curve + 3) % trackGenerator.track.Length];

        return p3 * coeff.x + p2 * coeff.y + p1 * coeff.z + p0 * coeff.w;

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
        Vector4 coeff = new Vector4(3*offset * offset, 3*(2-3*offset)*offset, 9 * offset * offset - 12 * offset + 3, -3*(1-offset)*(1-offset));

        Vector2 p0 = trackGenerator.track[(3 * curve + 0) % trackGenerator.track.Length];
        Vector2 p1 = trackGenerator.track[(3 * curve + 1) % trackGenerator.track.Length];
        Vector2 p2 = trackGenerator.track[(3 * curve + 2) % trackGenerator.track.Length];
        Vector2 p3 = trackGenerator.track[(3 * curve + 3) % trackGenerator.track.Length];

        return p3 * coeff.x + p2 * coeff.y + p1 * coeff.z + p0 * coeff.w;
    }

    //public (Vector2[], float, float) samplePoints(float curve, float offset, float distance_between_samples, float distance)
    //{
    //    if (offset < 0)
    //    {
    //        while (offset < 0)
    //        {
    //            offset += 1;
    //            curve -= 1;
    //        }
    //    }
    //    else
    //    {
    //        curve += (int)offset;
    //        offset %= 1;
    //    }
    //    curve %= trackGenerator.num_of_curves;
    //    if (curve < 0) curve += trackGenerator.num_of_curves;
    //}
}
