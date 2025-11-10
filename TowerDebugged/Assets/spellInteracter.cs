using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class spellInteracter : MonoBehaviour
{
    private GameObject gc;
    private Spell actualSpell;
    [SerializeField]
    public Image circle;

    public Color activateColor;

    [Header("Spell Feedbacks")]
    public MMFeedbacks insideFeedback;
    public MMFeedbacks outsideFeedback;
    public MMFeedbacks failFeedbacks;

    private bool wasPressed = false;
    private bool deactivated = false;
    public bool debugPressed = false;
    public int id;
    public bool GetPressed()
    {
        return wasPressed;
    }
    // Start is called before the first frame update
    void Start()
    {
        wasPressed = false;
        insideFeedback.Initialization();
        outsideFeedback.Initialization();
        gc = GameObject.FindWithTag("GameController");
        
    }

    public void SetActivateColor(Color newColor)
    {
        activateColor = newColor;
    }
    public void Inside(bool fromThird = false, bool pointerData = false)
    {
        if (pointerData == true)
        {
            if (!wasPressed)
            {
                Debug.Log("Going for the next DOT!");
                insideFeedback.PlayFeedbacks();
                wasPressed = true;
                gc.GetComponent<skillController>().SetPressed(this);
            }
            //if (DrawController.MyDrawInstance.CurrentLine != null)
            //DrawController.MyDrawInstance.CurrentLine.SetPosition(this.transform.localPosition);
        }
    }

    public void Outside(bool pointerData = false)
    {
        if (pointerData == true)
        {
            //Debug.Log("Went Outside! with DEAGGING SIGN: " + skillController.MySkillInstance.draggingSign);
            if (wasPressed && deactivated == false)
            {
                outsideFeedback.PlayFeedbacks();
                deactivated = true;
            }
        }
    }

    public void OutOfTime(PointerEventData pointerData = null)
    {
        Debug.Log("Exit from interacter!");
        failFeedbacks.PlayFeedbacks();
        skillController.MySkillInstance.SetExit(true);
        skillController.MySkillInstance.FailedSummon();
        if (skillController.MySkillInstance.casting == true)
        {
            skillController.MySkillInstance.casting = false;
        }
    }

    public Spell GetSpell()
    {
        return actualSpell;
    }

    public void SetSpell(Spell spell)
    {
        actualSpell = spell;
    }
    
    public void Countdown()
    {
        if (actualSpell == null)
        {
            StartCoroutine(CounterSimulation(2f));
        }
        else
        {
            StartCoroutine(CounterSimulation(actualSpell.GetTimeBtwTicks()));

        }
    }
    private IEnumerator CounterSimulation(float time = 0f)
    {
        RectTransform rectCircle = (RectTransform)circle.gameObject.transform;
        Vector2 buffer = new Vector2();
        buffer = rectCircle.sizeDelta;
        float timeCounter = 0f;

        while(timeCounter <= time)
        {
            //Debug.Log("Time is s: " + timeCounter);
            yield return new WaitForSeconds(Time.deltaTime);
            timeCounter += Time.deltaTime;
            if (rectCircle != null)
            {
                rectCircle.sizeDelta = Vector2.Lerp(buffer, Vector2.zero, timeCounter / time);
            }
        }

        if (wasPressed == false && LevelTraveler.MyTravelInstance.Level.isTutorial == false)
            OutOfTime();


        if (!debugPressed)
        {
            Debug.Log("Was pressed was" + wasPressed);
            Debug.Log("Debug was pressed was" + debugPressed);
        }
    }

}
