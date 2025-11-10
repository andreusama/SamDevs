using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;

public class GoldFeedback : MonoBehaviour
{

    //public TextMesh text;
    public MMFeedbacks feedbacks;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoldFeedbacks(int value)
    {
        feedbacks.Initialization();
        this.transform.gameObject.SetActive(true);
        //text.text = "+" + value.ToString();
        feedbacks.PlayFeedbacks();
    }
}
