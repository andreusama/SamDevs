using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Header("Seconds/Day")]
    public float dayTime;
    [Header("Speed Modification")]
    public int multiplier;

    [Header("Color Modification")]
    public Color nightLightColor;
    public Color dayLightColor;

    public Color towerLightDay;
    public Color towerLightNight;

    public Light generalLight;

    private bool aboutToFreeze = false;

    private int day;
    private bool isDay;

    [Header("Freeze Event")]
    public float freezeTime;
    public bool stopFreezing;
    public bool freezeEventOn;

    [Header("Pattern Metrics")]
    [SerializeField]
    private int step = 0;
    [SerializeField]
    private int patternPeriod = 8;
    [SerializeField]
    private int patternLoop = 0;

    int distanceToDanger = 100;
    //this bools check if we passed all elements to get the distance to the next dangerous tick only once. 
    private bool allPassed = false;
    public int GetPatternPeriod { get => patternPeriod; set => patternPeriod = value; }
    public enum PatternState
    {
        DANGER,
        SAFENESS
    }

    [SerializeField]
    private PatternState patternState;

    public void SetPatternState(PatternState newState)
    {
        patternState = newState;
    }
    public PatternState GetPatternState()
    {
        return patternState;
    }

    // Start is called before the first frame update
    void Start()
    {
        patternState = PatternState.SAFENESS;
        day = 1;
        step = 1;
        this.gameObject.GetComponent<UIController>().DayAdd(day);
        this.gameObject.GetComponent<UIController>().DayAnimation(true);
        this.gameObject.GetComponent<UIController>().NightAnimation(true);
        freezeEventOn = false;
        StartCoroutine(Clock(day, dayTime, multiplier));

        if (LevelTraveler.MyTravelInstance != null && LevelTraveler.MyTravelInstance.Level.isTutorial == false)
            LevelTraveler.MyTravelInstance.Level.SetBioma();
    }

    private static TimeController timeInstance;

    public static TimeController MyTimeInstance
    {
        get
        {
            if (timeInstance == null)
            {
                timeInstance = FindObjectOfType<TimeController>();
            }
            return timeInstance;
        }


    }

    IEnumerator Clock(int day, float dayTime, int multiplier)
    {
        for (int i = 0; i <= dayTime / 2; i++)
        {
            //Debug.Log("first loop" + i);
            if (i == 0)
            {
                DayActions();
                isDay = true;
                generalLight.color = dayLightColor;
                generalLight.intensity = 0.6f;
                this.gameObject.GetComponent<skillController>().resting = false;
                this.gameObject.GetComponent<UIController>().DaySwitch(isDay);
                this.gameObject.GetComponent<UIController>().DayAnimation(isDay);
            }
            if (i == (dayTime/2 - 2))
            {
                //only visual
                this.gameObject.GetComponent<UIController>().DayAnimation(false);
            }
            
            yield return new WaitForSeconds(1f * multiplier);

                TickBioma();
            //Debug.Log("Bioma Actual Step: " + step);

            step = IncreaseStep();
        }

        isDay = false;
        this.gameObject.GetComponent<skillController>().resting = true;

        for (int i = 0; i <= dayTime / 2; i++)
        {
            //Debug.Log("second loop" + i);
            generalLight.color = nightLightColor;
            generalLight.intensity = 0.4f;
            if (i == 0)
            {
                this.gameObject.GetComponent<UIController>().DaySwitch(isDay);
                this.gameObject.GetComponent<UIController>().NightAnimation(true);
            }
            if (i == (dayTime / 2 - 1))
            {
                //only visual
                this.gameObject.GetComponent<UIController>().NightAnimation(isDay);
            }

            yield return new WaitForSeconds(1f * multiplier);
            
            
            TickBioma();
            //Debug.Log("Actual Step: " + step);

            step = IncreaseStep();

        }

        day++;
        this.gameObject.GetComponent<UIController>().DayAdd(day);
        StartCoroutine(Clock(day, dayTime, multiplier));
        yield break;
    }

    public void DayActions()
    {
        if (QuestController.MyQuestInstance.ActiveQuests < 3 && QuestController.MyQuestInstance.questPool >= 1f )
        {
            QuestController.MyQuestInstance.Dirty = true;
        }
    }

    private void TickBioma()
    {
        if (LevelTraveler.MyTravelInstance.Level.isTutorial == true)
            return;
        
        if (buildController.MyBuildInstance.GetProgress() == buildController.Progress.WORKING)
        {
            //Debug.Log("Ticking bioma");
            if (LevelTraveler.MyTravelInstance != null)
                LevelTraveler.MyTravelInstance.Level.GetBioma().Tick(true, step);
        }
        else
        {
            if (LevelTraveler.MyTravelInstance != null)
                LevelTraveler.MyTravelInstance.Level.GetBioma().Tick(false, step);
        }
        
    }

    private int IncreaseStep()
    {
        step++;
        if (step > patternPeriod)
        {
            patternLoop++;
            step = 1;
            distanceToDanger = 100;
        }
        return step;
    }
    
    public int GetStepsToNextDanger()
    {
        List<int> dangerousSteps = LevelTraveler.MyTravelInstance.Level.GetBioma().GetPattern;

        if (dangerousSteps.Count <= 0)
        {
            return 1;
        }
        
        //it has to be a high value in order to update the first element of the array.
        for (int i = 0; i < dangerousSteps.Count; i++)
        {
            if (dangerousSteps[i] - step == 0)
            {
                distanceToDanger = 100;
            }

            if (dangerousSteps[i] - step > 0)
            {
                
                distanceToDanger = Mathf.Min(distanceToDanger, (dangerousSteps[i] - step));
                Debug.Log("Distance to danger: " + distanceToDanger + "A" + Time.deltaTime);

                return distanceToDanger;
            }
        }

        if (dangerousSteps[dangerousSteps.Count - 1] - step < 0)
        {
            distanceToDanger = patternPeriod - step + dangerousSteps[0];
            Debug.Log("Time to danger: " + distanceToDanger * (1f * multiplier) + "   " + +Time.deltaTime);
            return distanceToDanger;
        }

        return 0;
    }

    public float TimeFromSteps(int step)
    {


        return (step * (1f * multiplier));
    }
}
