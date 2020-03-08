using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player1Bar : MonoBehaviour
{
    public Image bar1;
    public float fill;
    // Start is called before the first frame update
    void Start()
    {
        fill = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        bar1.fillAmount = fill;
        fill += Time.deltaTime * 0.1f;
    }
}
