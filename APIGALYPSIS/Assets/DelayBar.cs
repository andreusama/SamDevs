using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using TMPro;

public class DelayBar : MonoBehaviour
{
    public TextMeshProUGUI delayedText;
    public MMProgressBar bar;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetBar(float max)
    {
        bar.SetBar(max, 0, max);
    }
    public void UpdateBar(float newValue, float max, float min)
    {
        delayedText.text = newValue.ToString() + "s";
        bar.UpdateBar(newValue, max, min);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
