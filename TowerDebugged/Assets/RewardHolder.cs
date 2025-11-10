using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Feedbacks;

public class RewardHolder : MonoBehaviour
{
    private Rewards reward;
    public enum rewardState
    {
        REWARD,
        OBJECTIVE
    }

    [SerializeField]
    private rewardState actualState = rewardState.REWARD;

    public void SetState(rewardState newState)
    {
        actualState = newState;

        switch (actualState)
        {
            case rewardState.REWARD:
                rewardBillboard.SetActive(true);
                objectiveBillboard.SetActive(false);

                break;
            case rewardState.OBJECTIVE:
                rewardBillboard.SetActive(false);
                objectiveBillboard.SetActive(true);
                break;
            default:
                break;
        }
    }
        
    public GameObject rewardBillboard;

    [SerializeField]
    private TextMeshProUGUI rewardQuantity;
    [SerializeField]
    private Image rewardSprite;

    public GameObject objectiveBillboard;

    [SerializeField]
    private MMFeedbacks revealFeedback;

    private float visualReward;
    private string sufijo;
    public GameObject questPrefab;

    public List<QuestHolder> internalHolders = new List<QuestHolder>();
    public void SetReward(Rewards reward)
    {
        this.reward = reward;
        
        //rewardQuantity.text = "x" + Mathf.Round(visualReward).ToString("") + sufijo;
        rewardSprite.sprite = reward.GetSprite;
    }

    public void SetRewardVisuals()
    {
        StartCoroutine(CountingNumbersEffect());
    }

    //create a coroutine that makes a number up effect to the reward
    public IEnumerator CountingNumbersEffect()
    {
        float actualNumber = 0;
        float targetNumber = reward.GetAmount;
        float speed = 0.05f;
        float actualSpeed = 0;
        float acceleration = Mathf.Sqrt(visualReward);
        while (actualNumber < targetNumber)
        {
            actualNumber += actualSpeed;
            actualSpeed += acceleration;
            rewardQuantity.text =  StatController.Aproximation(actualNumber);
            yield return new WaitForSeconds(speed);
        }
        rewardQuantity.text = StatController.Aproximation(targetNumber);
    }
    public void SetQuestsCompleted(Quest[] quests)
    {
        foreach (Quest quest in quests)
        {
            GameObject newQuest = Instantiate(questPrefab, objectiveBillboard.transform);
            newQuest.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            newQuest.GetComponent<QuestHolder>().SetQuest(quest);
            internalHolders.Add(newQuest.GetComponent<QuestHolder>());
            newQuest.SetActive(false);
        }
    }

    public void RevealFeedback()
    {
        revealFeedback.PlayFeedbacks();
    }

    public void InternalFeedbacks()
    {
        if (actualState == rewardState.OBJECTIVE)
        {
            StartCoroutine(PlayFeedbacksCoroutine());
        }
        else
        {
            SetRewardVisuals();
        }
    }

    private IEnumerator PlayFeedbacksCoroutine()
    {
        foreach (QuestHolder quest in internalHolders)
        {
            quest.gameObject.SetActive(true);
            quest.rewardFeedbacks.PlayFeedbacks();
            yield return new WaitForSeconds(1f);
        }
    }
}
