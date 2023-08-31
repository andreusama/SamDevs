using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class UIArt : MonoBehaviour
{
    public MMFeedbacks RollFeedback;
    public MMFeedbacks TurnChangeFeedbacks;
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
}
