using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stopwatch : MonoBehaviour
{
    public float time;
    private bool running;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (running) time += Time.deltaTime;
    }

    public void StartStopwatch()
    {
        running = true;
        time = 0;
    }

    public void ContinueStopwatch()
    {
        running = true;
    }

    public void StopStopwatch()
    {
        running = false;
    }
}
