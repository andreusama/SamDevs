using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bioma", menuName = "Biomas/Spring")]
public class Spring : Snowy
{
    public override void SetBioma(bool isActive)
    {
        //this sets the bioma vfx and needed variables
        
    }
    public override void StartBioma(bool isActive)
    {
        //Debug.Log("STARTING Bioma! At second" + Time.time);
        
    }
    public override void StopBioma(bool isActive)
    {
        //Debug.Log("STOPING Bioma! : At second" + Time.time);
        
    }
    public override void EndBioma(bool isActive)
    {
        
    }

    public override bool HasDeco()
    {
        return false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Tick(bool updating, int step)
    {
        //the general step will be the step that the TimeController is in, we have to scale it to our pattern of steps
        //like a music sheet, if a pattern is 4/4 and we are in beat 39 we're on the 3rd step of the 10th iteration, we have to scale it.
        if (!updating)
        {
            //Debug.Log("Returning, it is NOT ACTIVE");
            return;
        }

        //Debug.Log("Setting the step in bioma:" + step);

        if (Pattern.Contains(step + 1))
        {
            //FeedbackController.MyFeedbackInstance.Notification(anticipationMessage);
        }
        if (Pattern.Contains(step))
        {
            //-1 in order to reach 0 index
            //Debugger.MyTowerInstance.SetPatternDebug(step - 1, true);
            //StartBioma(true);
        }
        else
        {
            //Debugger.MyTowerInstance.SetPatternDebug(step - 1, false);
            //StopBioma(false);
        }

    }
}
