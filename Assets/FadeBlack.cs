using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBlack : MonoBehaviour
{
    public CarMovement car;
    public readonly float timeDelay = 0.5f;
    public float remainingDelay;
    bool fade;
    // Start is called before the first frame update
    void Start()
    {
        fade = false;
        remainingDelay = timeDelay;
        GetComponent<CanvasGroup>().alpha = 0;
    }

    public void start()
    {
        fade = true;
        remainingDelay = timeDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (fade)
        {
            if (car.spinOut)
            {
                if (remainingDelay > 0)
                {
                    remainingDelay -= Time.deltaTime;
                    return;
                }
                GetComponent<CanvasGroup>().alpha = Mathf.Min(1f, GetComponent<CanvasGroup>().alpha + Time.deltaTime);
            }
        }
        else
        {
            GetComponent<CanvasGroup>().alpha = Mathf.Max(0f, GetComponent<CanvasGroup>().alpha - Time.deltaTime);

        }
    }
    public void Undo()
    {
        fade = false;
    }
}
