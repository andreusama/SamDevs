using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnlockFeedbackHolder : MonoBehaviour
{
    public TextMeshProUGUI content;
    public GameObject bufferBillboard;
    public GameObject textObject;
    // Start is called before the first frame update
    public void Flip(MenuUI.AnchorPresets anchor)
    {
        switch (anchor)
        {
            case MenuUI.AnchorPresets.MiddleLeft:
                bufferBillboard.transform.rotation = Quaternion.Euler(0, 0, 0);
                textObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case MenuUI.AnchorPresets.MiddleRight:
                bufferBillboard.transform.rotation = Quaternion.Euler(0, -180, 0);
                textObject.transform.rotation = Quaternion.Euler(0, -180, 0);
                break;
            default:
                break;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
