using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bioma", menuName = "Biomas/Snowy")]
public class Snowy : Bioma
{
    public override void SetBioma(bool isActive)
    {
        //this sets the bioma vfx and needed variables
        UIController.MyUiInstance.freezeVfx.SetActive(true);
    }
    public override void StartBioma(bool isActive)
    {
        TimeController.MyTimeInstance.SetPatternState(TimeController.PatternState.DANGER);
        TimeController.MyTimeInstance.freezeEventOn = isActive;
    }
    public override void StopBioma(bool isActive)
    {
        TimeController.MyTimeInstance.SetPatternState(TimeController.PatternState.SAFENESS);
        TimeController.MyTimeInstance.freezeEventOn = isActive;
    }
    public override void EndBioma(bool isActive)
    {
        UIController.MyUiInstance.freezeVfx.SetActive(TimeController.MyTimeInstance.freezeEventOn);
    }

    public override void Tick(bool updating, int step)
    {
        //the general step will be the step that the TimeController is in, we have to scale it to our pattern of steps
        //like a music sheet, if a pattern is 4/4 and we are in beat 39 we're on the 3rd step of the 10th iteration, we have to scale it.
        if (!updating)
        {
            StopBioma(false);
            //Debug.Log("Returning, it is NOT ACTIVE");
            return;
        }

        //Debug.Log("Setting the step in bioma:" + step);

        if (Pattern.Contains(step+2))
        {
            FeedbackController.MyFeedbackInstance.Notification(anticipationMessage);
        }
        if (Pattern.Contains(step))
        {
            //-1 in order to reach 0 index
            Debugger.MyTowerInstance.SetPatternDebug(step - 1, true);
            StartBioma(true);
        }
        else
        {
            Debugger.MyTowerInstance.SetPatternDebug(step - 1, false);
            StopBioma(false);
        }
        
    }

    public override bool HasDeco()
    {
        return true;
    }

}
