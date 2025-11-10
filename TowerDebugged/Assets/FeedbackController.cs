using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class FeedbackController : MonoBehaviour
{
    private static FeedbackController feedbackInstance;

    public static FeedbackController MyFeedbackInstance
    {
        get
        {
            if (feedbackInstance == null)
            {
                feedbackInstance = FindObjectOfType<FeedbackController>();
            }
            return feedbackInstance;
        }


    }

    public GameObject EventSpace;

    public MMFeedbacks criticFeedback;
    public MMFeedbacks spellFeedback;
    public MMFeedbacks spellFailureFeedback;
    public MMFeedbacks hitFeedback;

    //fx
    public MMFeedbacks buttonFxFeedback;
    public MMFeedbacks purchaseFxFeedback;
    public MMFeedbacks arrowFxFeedback;
    public MMFeedbacks craftFxFeedback;
    public MMFeedbacks movementFxFeedback;
    public MMFeedbacks newFxFeedback;
    public MMFeedbacks secondaryButtonFxFeedback;
    public MMFeedbacks onFinishCraftFeedback;
    public MMFeedbacks enviromentFeedback;
    public MMFeedbacks closeFxFeedback;
    public MMFeedbacks destroyFxFeedback;
    public MMFeedbacks buildFxFeedback;
    public MMFeedbacks iraFeedback;
    public MMFeedbacks flipEquip;
    public MMFeedbacks signSetUpFeedbacks;
    public MMFeedbacks signFailFeedbacks;
    public MMFeedbacks tutorialSignFeedbacks;

    public GameObject newPrefab;
    public GameObject handPrefab;
    public GameObject arrowPrefab;
    // Start is called before the first frame update
    void Start()
    {
        if (enviromentFeedback != null)
        {
            enviromentFeedback.PlayFeedbacks();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FeedbackHit()
    {
        hitFeedback.PlayFeedbacks();
    }

    public void ArrowFx()
    {
        arrowFxFeedback.PlayFeedbacks();
    }
    
    public void FlipEquip()
    {
        flipEquip.PlayFeedbacks();
    }

    public void CloseFx()
    {
        closeFxFeedback.PlayFeedbacks();
    }

    public void DestroyFx()
    {
        destroyFxFeedback.PlayFeedbacks();
    }

    public void SecButtonFx()
    {
        secondaryButtonFxFeedback.PlayFeedbacks();
    }

    public void FinishCraftFeedback()
    {
        onFinishCraftFeedback.PlayFeedbacks();
    }

    public void MovementFx()
    {
        movementFxFeedback.PlayFeedbacks();
    }
    public void NewFx()
    {
        newFxFeedback.PlayFeedbacks();
    }
    public void CraftFx()
    {
        craftFxFeedback.PlayFeedbacks();
    }

    public void PurchaseFxFeedback()
    {
        purchaseFxFeedback.PlayFeedbacks();
    }

    public void FeedbackCritical()
    {
        criticFeedback.PlayFeedbacks();
    }

    public void FeedbackIra()
    {
        iraFeedback.PlayFeedbacks();
    }

    public void BuildFeedback()
    {
        buildFxFeedback.PlayFeedbacks();
    }

    public void FeedbackSpellPoint(bool failure)
    {
        if (failure == true)
            spellFailureFeedback.PlayFeedbacks();

        if (failure == false)
            spellFeedback.PlayFeedbacks();
    }

    public void Notification(string _message)
    {
        StartCoroutine(newFeedbackCoroutine(_message)); 
    }

    public void ButtonFx()
    {
        buttonFxFeedback.PlayFeedbacks();
    }

    private IEnumerator newFeedbackCoroutine(string _message)
    {
        NewFx();
        GameObject newNew = Instantiate(newPrefab, EventSpace.transform);

        newNew.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        newNew.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        newNew.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        newNew.GetComponent<RectTransform>().pivot = new Vector2(0, 0);


        newNew.GetComponent<RectTransform>().sizeDelta = new Vector2(573.8f, 117);
        
        newPrefab.GetComponent<newHolder>().SetText(_message);

        yield return new WaitForSeconds(1f * TimeController.MyTimeInstance.multiplier);

        //Destroy(newNew.gameObject);


        yield break;
    }

    public GameObject InstantiateFeedbackHand()
    {
        GameObject newHand = Instantiate(handPrefab);
        return newHand;
    }

    public GameObject InstantiateFeedbackArrow()
    {
        GameObject newArrow = Instantiate(arrowPrefab);
        return newArrow;
    }
}
