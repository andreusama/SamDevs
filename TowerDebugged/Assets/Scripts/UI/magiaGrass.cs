using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class magiaGrass : MonoBehaviour
{

    public RawImage magiaImage;
    // Use this for initialization
    void Awake()
    {
        //magiaImage = transform.Find("Magia").GetComponent<RawImage>();
    }


    // Update is called once per frame
    void Update()
    {
        Rect uvRect = magiaImage.uvRect;
        uvRect.x += 0.2f * Time.deltaTime;
        magiaImage.uvRect = uvRect;
    }
}
