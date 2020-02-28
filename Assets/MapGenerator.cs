using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject grass;
    private float grassHeight = -0.122f;
    private GameObject[] current;
    private GameObject[] nextCurrent;
    private int t;
    private int period;
    // Start is called before the first frame update
    void Start()
    {
        spheres = new List<GameObject>();
        GenerateMap(352989324);
        t = 0;
        period = 500;
        current = new GameObject[period];
        nextCurrent = new GameObject[period];
        InitGrass();
    }

    // Update is called once per frame
    void Update()
    {
        t += 1;
        Draw();
    }

    public void InitGrass()
    {
        int index = 0;
        for (int i = 0; i < current.Length; i++)
        {

            float offset = (float)i / period;

            Vector2 r0 = (1 - offset) * dataset[index] + offset * dataset[index + 1];
            Vector2 r1 = (1 - offset) * dataset[index + 1] + offset * dataset[index + 2];
            Vector2 r2 = (1 - offset) * dataset[index + 2] + offset * dataset[index + 3];

            Vector2 q0 = (1 - offset) * r0 + offset * r1;
            Vector2 q1 = (1 - offset) * r1 + offset * r2;

            Vector2 finalPoint = (1 - offset) * q0 + offset * q1;

            Vector3 position = new Vector3(finalPoint.x, grassHeight, finalPoint.y);



            offset = (float)(i + 1) / period;

            r0 = (1 - offset) * dataset[index] + offset * dataset[index + 1];
            r1 = (1 - offset) * dataset[index + 1] + offset * dataset[index + 2];
            r2 = (1 - offset) * dataset[index + 2] + offset * dataset[index + 3];

            q0 = (1 - offset) * r0 + offset * r1;
            q1 = (1 - offset) * r1 + offset * r2;

            Vector2 finalPoint2 = (1 - offset) * q0 + offset * q1;

            Vector2 diff = finalPoint2 - finalPoint;

            float angle = (float)System.Math.Atan2(diff.y, diff.x);
            Quaternion rotation = Quaternion.AngleAxis(-(float)(angle * 180 / System.Math.PI) + 90, new Vector3(0, 1, 0));
            current[i] = Instantiate(grass, position, rotation);
            current[i].GetComponent<Renderer>().enabled = true;
        }
        index = 3;
        for (int i = 0; i < current.Length; i++)
        {

            float offset = (float)i / period;

            Vector2 r0 = (1 - offset) * dataset[index] + offset * dataset[index + 1];
            Vector2 r1 = (1 - offset) * dataset[index + 1] + offset * dataset[index + 2];
            Vector2 r2 = (1 - offset) * dataset[index + 2] + offset * dataset[index + 3];

            Vector2 q0 = (1 - offset) * r0 + offset * r1;
            Vector2 q1 = (1 - offset) * r1 + offset * r2;

            Vector2 finalPoint = (1 - offset) * q0 + offset * q1;

            Vector3 position = new Vector3(finalPoint.x, grassHeight, finalPoint.y);



            offset = (float)(i + 1) / period;

            r0 = (1 - offset) * dataset[index] + offset * dataset[index + 1];
            r1 = (1 - offset) * dataset[index + 1] + offset * dataset[index + 2];
            r2 = (1 - offset) * dataset[index + 2] + offset * dataset[index + 3];

            q0 = (1 - offset) * r0 + offset * r1;
            q1 = (1 - offset) * r1 + offset * r2;

            Vector2 finalPoint2 = (1 - offset) * q0 + offset * q1;

            Vector2 diff = finalPoint2 - finalPoint;

            float angle = (float)System.Math.Atan2(diff.y, diff.x);
            Quaternion rotation = Quaternion.AngleAxis(-(float)(angle * 180 / System.Math.PI) + 90, new Vector3(0, 1, 0));
            nextCurrent[i] = Instantiate(grass, position, rotation);
            nextCurrent[i].GetComponent<Renderer>().enabled = true;
        }
    }

    public void UpdateGrass(int index)
    {
        for (int i = 0; i < current.Length; i++)
        {
            Destroy(current[i], 0);
            current[i] = nextCurrent[i];

            float offset = (float)i / period;

            Vector2 r0 = (1 - offset) * dataset[index] + offset * dataset[index + 1];
            Vector2 r1 = (1 - offset) * dataset[index + 1] + offset * dataset[index + 2];
            Vector2 r2 = (1 - offset) * dataset[index + 2] + offset * dataset[index + 3];

            Vector2 q0 = (1 - offset) * r0 + offset * r1;
            Vector2 q1 = (1 - offset) * r1 + offset * r2;

            Vector2 finalPoint = (1 - offset) * q0 + offset * q1;

            Vector3 position = new Vector3(finalPoint.x, grassHeight, finalPoint.y);



            offset = (float)(i + 1) / period;

            r0 = (1 - offset) * dataset[index] + offset * dataset[index + 1];
            r1 = (1 - offset) * dataset[index + 1] + offset * dataset[index + 2];
            r2 = (1 - offset) * dataset[index + 2] + offset * dataset[index + 3];

            q0 = (1 - offset) * r0 + offset * r1;
            q1 = (1 - offset) * r1 + offset * r2;

            Vector2 finalPoint2 = (1 - offset) * q0 + offset * q1;

            Vector2 diff = finalPoint2 - finalPoint;

            float angle = (float)System.Math.Atan2(diff.y, diff.x);
            Quaternion rotation = Quaternion.AngleAxis(-(float)(angle * 180 / System.Math.PI) + 90, new Vector3(0, 1, 0));
            nextCurrent[i] = Instantiate(grass, position, rotation);
            nextCurrent[i].GetComponent<Renderer>().enabled = true;
        }
    }

    private Vector2[] dataset;

    //https://www.gamasutra.com/blogs/GustavoMaciel/20131229/207833/Generating_Procedural_Racetracks.php

    float GetT(float t, Vector2 p0, Vector2 p1)
    {
        float a = Mathf.Pow((p1.x - p0.x), 2.0f) + Mathf.Pow((p1.y - p0.y), 2.0f);
        float b = Mathf.Pow(a, 0.5f);
        float c = Mathf.Pow(b, 0.5f);

        return (c + t);
    }

    void GenerateMap(int seed)
    {
        int initialPoints = 8;
        int pushIterations = 3;
        float difficulty = 1f; //the closer the value is to 0, the harder the track should be. Grows exponentially.  
        float maxDisp = 20f; // Again, this may change to fit your units.  
        int fixIterations = 10;
        int numberOfPoints = 50;

        System.Random random = new System.Random(seed);
        dataset = new Vector2[initialPoints];
        for (int i = 0; i < initialPoints; i++)
        {
            dataset[i] = new Vector2((float)random.Next() / (float)int.MaxValue * 125f, (float)random.Next() / (float)int.MaxValue * 125f);
        }

        for (int i = 0; i < pushIterations; ++i)
        {
            pushApart(dataset);
        }

        Vector2[] rSet = new Vector2[dataset.Length * 2];
        Vector2 disp = new Vector2();
        for (int i = 0; i < dataset.Length; ++i)
        {
            float dispLen = (float)System.Math.Pow(random.Next() / 2f / int.MaxValue + 0.5f, difficulty) * maxDisp;
            disp.Set(0, 1);
            float degrees = (random.Next() / 2f / int.MaxValue + 0.5f) * 360;
            float sin = (float)Mathf.Sin((float)(degrees * System.Math.PI/180));
            float cos = (float)Mathf.Cos((float)(degrees * System.Math.PI/180));
            float tx = disp.x;
            float ty = disp.y;
            disp.x = (cos * tx) - (sin * ty);
            disp.y = (sin * tx) + (cos * ty);
            disp.x *= dispLen;
            disp.y *= dispLen;
            rSet[i * 2] = dataset[i];
            rSet[i * 2 + 1] = new Vector2(dataset[i].x + dataset[(i + 1) % dataset.Length].x / 2 + disp.x, dataset[i].y + dataset[(i + 1) % dataset.Length].y / 2 + disp.y);
            //Explaining: a mid point can be found with (dataSet[i]+dataSet[i+1])/2.  
            //Then we just add the displacement.  
        }
        dataset = rSet;


        rSet = new Vector2[dataset.Length * 2];
        disp = new Vector2();
        for (int i = 0; i < dataset.Length; ++i)
        {
            float dispLen = (float)System.Math.Pow(random.Next() / 2f / int.MaxValue + 0.5f, difficulty) * maxDisp;
            disp.Set(0, 1);
            float degrees = (random.Next() / 2f / int.MaxValue + 0.5f) * 360;
            float sin = (float)Mathf.Sin((float)(degrees * System.Math.PI / 180));
            float cos = (float)Mathf.Cos((float)(degrees * System.Math.PI / 180));
            float tx = disp.x;
            float ty = disp.y;
            disp.x = (cos * tx) - (sin * ty);
            disp.y = (sin * tx) + (cos * ty);
            disp.x *= dispLen;
            disp.y *= dispLen;
            rSet[i * 2] = dataset[i];
            rSet[i * 2 + 1] = new Vector2(dataset[i].x + dataset[(i + 1) % dataset.Length].x / 2 + disp.x, dataset[i].y + dataset[(i + 1) % dataset.Length].y / 2 + disp.y);
            //Explaining: a mid point can be found with (dataSet[i]+dataSet[i+1])/2.  
            //Then we just add the displacement.  
        }
        dataset = rSet;
        //push apart again, so we can stabilize the points distances.  
        for (int i = 0; i < pushIterations; ++i)
        {
            pushApart(dataset);
        }

        for (int i = 0; i < fixIterations; ++i)
        {
            fixAngles(dataset);
            pushApart(dataset);
        }

        Vector2[] finalPoints = new Vector2[dataset.Length + (dataset.Length - 4) / 2];
        finalPoints[0] = dataset[0];
        finalPoints[finalPoints.Length - 3] = dataset[dataset.Length - 3];
        finalPoints[finalPoints.Length - 2] = dataset[dataset.Length - 2];
        finalPoints[finalPoints.Length - 1] = dataset[dataset.Length - 1];

        for (int i = 0; i < (dataset.Length - 4) / 2; i++)
        {
            finalPoints[1 + 3 * i] = dataset[1 + 2 * i];
            finalPoints[2 + 3 * i] = dataset[2 + 2 * i];
            finalPoints[3 + 3 * i] = (dataset[2 + 2 * i] + dataset[3 + 2 * i]) / 2;
        }
        dataset = finalPoints;

    }

    List<GameObject> spheres;
    public GameObject sphere;

    // Visualize the points
    void Draw()
    {
        int index = (t / period) * 3;
        float offset = (float)(t % period) / period;
        if (offset == 0) UpdateGrass(index + 3);

        Vector2 r0 = (1 - offset) * dataset[index] + offset * dataset[index + 1];
        Vector2 r1 = (1 - offset) * dataset[index + 1] + offset * dataset[index + 2];
        Vector2 r2 = (1 - offset) * dataset[index + 2] + offset * dataset[index + 3];

        Vector2 q0 = (1 - offset) * r0 + offset * r1;
        Vector2 q1 = (1 - offset) * r1 + offset * r2;

        Vector2 finalPoint = (1 - offset) * q0 + offset * q1;

        transform.position = new Vector3(finalPoint.x, 0.291f, finalPoint.y);



        offset = (float)((t + 1) % period) / period;
        if ((t % period) + 1 == period)
        {
            offset = 0;
            index += 3;
        }

        r0 = (1 - offset) * dataset[index] + offset * dataset[index + 1];
        r1 = (1 - offset) * dataset[index + 1] + offset * dataset[index + 2];
        r2 = (1 - offset) * dataset[index + 2] + offset * dataset[index + 3];

        q0 = (1 - offset) * r0 + offset * r1;
        q1 = (1 - offset) * r1 + offset * r2;

        Vector2 finalPoint2 = (1 - offset) * q0 + offset * q1;

        Vector2 diff = finalPoint2 - finalPoint;

        float angle = (float)System.Math.Atan2(diff.y, diff.x);
        transform.rotation = Quaternion.AngleAxis(-(float)(angle * 180 / System.Math.PI) + 90, new Vector3(0, 1, 0));


        foreach (GameObject my_sphere in spheres)
        {
            Destroy(my_sphere, 0);
        }
        spheres = new List<GameObject>();
        for (int i = 0; i < dataset.Length; i++)
        {
            Vector3 pos = new Vector3(dataset[i].x, 1, dataset[i].y);
            spheres.Add(Instantiate(sphere, pos, Quaternion.identity));
        }
    }

    void pushApart(Vector2[] dataSet)
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

    void fixAngles(Vector2[] dataSet)
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

            if (System.Math.Abs(a * 180/System.Math.PI) <= 100) continue;

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

}
