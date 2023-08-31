using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class UIArt : MonoBehaviour
{
    public MMFeedbacks RollFeedback;
    public MMFeedbacks TurnChangeFeedbacks;

    public MMFeedbacks loseFeedback;
    public MMFeedbacks winFeedback;

    public MMFeedbacks showDelayBar;
    public MMFeedbacks hideDelayBar;

    public DelayBar delayBarScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayRollFeedback()
    {
        RollFeedback?.PlayFeedbacks();
    }

    public void PlayTurnChangeFeedbacks()
    {
        TurnChangeFeedbacks?.PlayFeedbacks();
    }

    public void SetBar(float max)
    {
        Debug.Log("Setting the bar to true");
        delayBarScript.SetBar(max);
    }
    public void UpdateDelayedBar(float newValue, float max)
    {
        delayBarScript.transform.gameObject.SetActive(true);
        delayBarScript.UpdateBar(newValue,  max, 0);
    }
    public void ShowDelayBar()
    {
        showDelayBar?.PlayFeedbacks();
    }

    public void HideDelayBar()
    {
        hideDelayBar?.PlayFeedbacks();
    }

}
