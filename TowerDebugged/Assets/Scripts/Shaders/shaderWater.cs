using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class shaderWater : MonoBehaviour
{

    public RawImage magiaImage;
    public float speed = 0.1f;
    // Use this for initialization
    void Awake()
    {
        //magiaImage = transform.Find("Magia").GetComponent<RawImage>();
    }


    // Update is called once per frame
    void Update()
    {
        Rect uvRect = magiaImage.uvRect;
        uvRect.x -= speed * Time.deltaTime;
        magiaImage.uvRect = uvRect;
    }
}

