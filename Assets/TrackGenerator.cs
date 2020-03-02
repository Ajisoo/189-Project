using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
    public readonly int num_of_curves = 10;
    private readonly int push_iterations = 3;
    public Vector2[] track;

    void Awake()
    {
        track = new Vector2[num_of_curves * 3];
        generateNewTrack((int)(Random.value * int.MaxValue));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void generateNewTrack(int new_seed)
    {
        Random.InitState(new_seed);
        Vector2[] cp1 = new Vector2[num_of_curves];

        for (int i = 0; i < cp1.Length; i++)
        {
            cp1[i] = new Vector2(Random.Range(-125, 125), Random.Range(-125, 125));
        }

        for (int i = 0; i < push_iterations; i++)
        {
            pushApart(cp1);
        }

        Vector2[] rSet = new Vector2[cp1.Length * 2];
        Vector2 disp = new Vector2();
        for (int i = 0; i < cp1.Length; ++i)
        {
            float dispLen = (float)System.Math.Pow(Random.Range(int.MinValue, int.MaxValue) / 2f / int.MaxValue + 0.5f, 0.5f) * 20f;
            disp.Set(0, 1);
            float degrees = (Random.Range(int.MinValue, int.MaxValue) / 2f / int.MaxValue + 0.5f) * 360;
            float sin = (float)Mathf.Sin((float)(degrees * System.Math.PI / 180));
            float cos = (float)Mathf.Cos((float)(degrees * System.Math.PI / 180));
            float tx = disp.x;
            float ty = disp.y;
            disp.x = (cos * tx) - (sin * ty);
            disp.y = (sin * tx) + (cos * ty);
            disp.x *= dispLen;
            disp.y *= dispLen;
            rSet[i * 2] = cp1[i];
            rSet[i * 2 + 1] = new Vector2(cp1[i].x + cp1[(i + 1) % cp1.Length].x / 2 + disp.x, cp1[i].y + cp1[(i + 1) % cp1.Length].y / 2 + disp.y);
            //Explaining: a mid point can be found with (dataSet[i]+dataSet[i+1])/2.  
            //Then we just add the displacement.  
        }
        Vector2[] cp = rSet;

        for (int i = 0; i < push_iterations; ++i)
        {
            pushApart(cp);
        }

        for (int i = 0; i < 10; ++i)
        {
            fixAngles(cp);
            pushApart(cp);
        }

        for (int i = 0; i < num_of_curves; i++)
        {
            track[3 * i + 1] = cp[2 * i];
            track[3 * i + 2] = cp[2 * i + 1];
        }

        for (int i = 0; i < num_of_curves; i++)
        {
            int prev = (3 * i + track.Length - 1) % track.Length;
            int next = (3 * i + 1) % track.Length;
            track[3 * i] = (track[prev] + track[next]) / 2;
        }
    }

    private void fixAngles(Vector2[] dataSet)
    {
        for (int i = 0; i < dataSet.Length; ++i)
        {
            int previous = (i - 1 < 0) ? dataSet.Length - 1 : i - 1;
            int next = (i + 1) % dataSet.Length;
            float px = dataSet[i].x - dataSet[previous].x;
            float py = dataSet[i].y - dataSet[previous].y;
            float pl = (float)System.Math.Sqrt(px * px + py * py);
            px /= pl;
            py /= pl;

            float nx = dataSet[i].x - dataSet[next].x;
            float ny = dataSet[i].y - dataSet[next].y;
            nx = -nx;
            ny = -ny;
            float nl = (float)System.Math.Sqrt(nx * nx + ny * ny);
            nx /= nl;
            ny /= nl;
            //I got a vector going to the next and to the previous points, normalised.  

            float a = (float)System.Math.Atan2(px * ny - py * nx, px * nx + py * ny); // perp dot product between the previous and next point. Google it you should learn about it!  

            if (System.Math.Abs(a * 180 / System.Math.PI) <= 100) continue;

            float nA = (100 * (float)System.Math.Sign((float)a)) * (float)System.Math.PI / 180;
            float diff = nA - a;
            float cos = (float)System.Math.Cos((float)diff);
            float sin = (float)System.Math.Sin((float)diff);
            float newX = nx * cos - ny * sin;
            float newY = nx * sin + ny * cos;
            newX *= nl;
            newY *= nl;
            dataSet[next].x = dataSet[i].x + newX;
            dataSet[next].y = dataSet[i].y + newY;
            //I got the difference between the current angle and 100degrees, and built a new vector that puts the next point at 100 degrees.  
        }
    }

    private void pushApart(Vector2[] dataSet)
    {
        float dst = 15; //I found that 15 is a good value, though maybe, depending on your scale you'll need other value.  
        float dst2 = dst * dst;
        for (int i = 0; i < dataSet.Length; ++i)
        {
            for (int j = i + 1; j < dataSet.Length; ++j)
            {
                if ((dataSet[i].x - dataSet[j].x) * (dataSet[i].x - dataSet[j].x) + (dataSet[i].y - dataSet[j].y) * (dataSet[i].y - dataSet[j].y) < dst2)
                {
                    float hx = dataSet[j].x - dataSet[i].x;
                    float hy = dataSet[j].y - dataSet[i].y;
                    float hl = (float)System.Math.Sqrt(hx * hx + hy * hy);
                    hx /= hl;
                    hy /= hl;
                    float dif = dst - hl;
                    hx *= dif;
                    hy *= dif;
                    dataSet[j].x += hx;
                    dataSet[j].y += hy;
                    dataSet[i].x -= hx;
                    dataSet[i].y -= hy;
                }
            }
        }
    }
}
