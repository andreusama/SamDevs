using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class QuestHolder : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI objectiveDescriptionText;
    public TextMeshProUGUI objectiveProgressText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI idText;

    public Quest internalQuest;

    public GameObject rewardPanel;

    public MMFeedbacks progressFeedbacks;
    public MMFeedbacks goldFeedbacks;
    public MMFeedbacks rewardFeedbacks;

    public MMFeedbacks finishFeedbacks;

    public Transform tutorialAnchor;

    //create two textmeshproGUI with a header named "Reward Text" and a body named "Reward Description Text"
    public TextMeshProUGUI goldRewardText;
    public TextMeshProUGUI xpRewardText;


    public void SetQuest(Quest newQuest)
    {
        internalQuest = newQuest;
        title.text = internalQuest.title;
        objectiveDescriptionText.text = internalQuest.objective;
        objectiveProgressText.text = internalQuest.currentProgress + "/" + internalQuest.aimProgress;

        Debug.Log("Setting quests");
        goldText.text = StatController.Aproximation(internalQuest.goldReward);
        xpText.text = StatController.Aproximation(internalQuest.xpReward);
        
        idText.text = internalQuest.typeIndex.ToString();
        goldFeedbacks.Initialization();
        rewardFeedbacks.Initialization();
    }

    public void RewardFeedback()
    {
        Debug.Log("Reward Feedbacks");
        rewardFeedbacks.PlayFeedbacks();
    }

    public void RefreshQuest()
    {
        title.text = internalQuest.title;
        objectiveDescriptionText.text = internalQuest.objective;
        objectiveProgressText.text = internalQuest.currentProgress + "/" + internalQuest.aimProgress;
        
        goldText.text = StatController.Aproximation(internalQuest.goldReward);

        xpText.text = StatController.Aproximation(internalQuest.xpReward);

        idText.text = internalQuest.typeIndex.ToString();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (internalQuest.Completed == true)
            return;
        
        if (internalQuest.Dirty == true)
        {
            if (internalQuest.currentProgress >= internalQuest.aimProgress)
            {
                RewardButton();
                finishFeedbacks.PlayFeedbacks();
                internalQuest.Completed = true;
                return;
            }
            RefreshQuest();
            internalQuest.Dirty = false;
        }
    }

    public void RewardButton()
    {
        //Debug.Log("Rewarding");
        internalQuest.Reward();
        internalQuest.Active = false;
        goldRewardText.text = StatController.Aproximation(internalQuest.goldReward);
        Debug.Log("Setting the value of the xp in reward " + internalQuest.xpReward);
        xpRewardText.text = StatController.Aproximation(internalQuest.xpReward);
    }
}
