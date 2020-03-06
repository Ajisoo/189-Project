using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopIn : MonoBehaviour
{
    public float startingSize;
    public float endingSize;
    public float scale;
    private float size;
    // Start is called before the first frame update
    void Start()
    {
        size = startingSize;
    }

    // Update is called once per frame
    void Update()
    {
        size = (size - endingSize) * scale + endingSize;    
        transform.localScale = new Vector3(size, size, size);
    }
}
