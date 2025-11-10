using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.M_UI;
using MoreMountains.Feedbacks;
using TMPro;
using System;

[Serializable]
public class UIStack
{
    public GameObject[] stack;
    public bool state = false;
    public void SetActive(bool active)
    {
        state = active;
        foreach (GameObject go in stack)
        {
            go.SetActive(active);
        }
    }

    public void Push(GameObject go)
    {
        //create a new array with the size of the old array + 1
        GameObject[] newStack = new GameObject[stack.Length + 1];
        //copy the old array to the new array
        Array.Copy(stack, newStack, stack.Length);
        //set the last element of the new array to the new object
        newStack[newStack.Length - 1] = go;
        //set the new array to the old array
        stack = newStack;

        SetState(go);
    }

    //set a new element to the same state of the stack
    public void SetState(GameObject go)
    {
        go.SetActive(state);
    }
}
public class UIController : MonoBehaviour
{
    public enum AnchorPresets
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    public AnchorPresets anchoredPos;
    
    [Header("Cover UI")]
    [SerializeField]
    private UIStack coverStack;

    [Header("Tutorial UI")]
    public GameObject tutorialScreen;

    public UIStack getCoverStack()
    {
        return coverStack;
    }
    
    [Header("Reward UI")]
    public GameObject finishPanel;
    public GameObject finishButton;
    [Header("Failed UI")]
    public GameObject failPanel;
    public GameObject failButton;
    public TextMeshProUGUI failedText;
    [Header("Skill Icons")]
    [SerializeField]
    public Sprite idleSprite;
    public Color32 idleColor;
    [Header("Buttons M_UI")]
    public Sprite longButtonGold;
    public Sprite longButtonGrey;
    public Sprite longButtonRed;
    public Sprite shortButtonGold;
    public Sprite shortButtonGrey;
    public Sprite shortButtonRed;
    public Sprite deepButtonGold;
    public Sprite deepButtonGrey;
    [Header("Quest UI")]
    public GameObject questPanel;
    [Header("Resources M_UI")]
    //public GameObject ResourcesSpace;
    [Header("ActionBar M_UI")]
    public GameObject ActionBarSpace;
    public GameObject AttackCover;
    [Header("Explore Space UI")]
    //-EXPLORE UI
    public Sprite unactiveButton;
    //-----------------------------
    [Header("Movement Space UI")]
    public GameObject NormalMovement;
    public GameObject FlatMovement;
    public GameObject MovementPanel;
    [Header("Exit M_UI")]
    public GameObject exitPanel;
    [Header("Bag Space M_UI")]
    public GameObject BagSpace;
    public GameObject equipDeco;
    public GameObject equipWheel;

    [Header("Skill M_UI")]
    public GameObject SpellSpace;
    public Image arcaneRuneDeco;
    public GameObject RageSpace;
    public GameObject ProgressLayout;
    public GameObject progressDotPrefab;
    public GameObject craftFeedbackCard;
    [Header("Deco M_UI")]
    public GameObject decoPanel;
    [Header("DayTime Deco")]
    public GameObject dayBackground;
    public GameObject nightBackground;
    public GameObject sun;
    public GameObject moon;
    private Animator sunAnimator;
    private Animator moonAnimator;
    [Header("Buff Space")]
    public GameObject buffSpace;

    [Header("Events VFX Space")]
    public GameObject freezeVfx;

    [Header("MiniMap Space")]
    public GameObject miniMapBillboard;

    [Header("Final Rewards Space")]
    public GameObject rewardPrefab;
    public GameObject separatorPrefab;
    public Transform rewardParent;

    [Header("Tutorial UI")]
    public GameObject towerAttackTutorial;

    [Header("Sign Space")]
    public TextMeshProUGUI signTalkingText;
    //first element is X hide second is X show
    public Vector2 hideShow = Vector2.zero;
    [SerializeField]
    private bool mapShown = false;

    public List<Image> signCrosses;
    
    public bool open;

    private static UIController uiInstance;

    public static UIController MyUiInstance
    {
        get
        {
            if (uiInstance == null)
            {
                uiInstance = FindObjectOfType<UIController>();
            }
            return uiInstance;
        }


    }

    public List<GameObject> list;
    // Start is called before the first frame update
    private enum UIState
    {
        STATE_IDLE,
        STATE_COVERED
    }

    private UIState uiState;

    void Start()
    {
        coverStack.SetActive(false);
        
        finishPanel.SetActive(false);
        finishButton.SetActive(false);

        ws = false;
        ip = false;
        open = false;
        //list.Add(ResourcesSpace);
        list.Add(ActionBarSpace);
        sunAnimator = sun.gameObject.GetComponent<Animator>();
        moonAnimator = moon.gameObject.GetComponent<Animator>();
        NormalMovement.SetActive(false);
    }

    private bool ws;
    private bool ip;
    
    public void SwitchUI()
    {
        switch (uiState)
        {
            case UIState.STATE_IDLE:
                //do something
                //set the next state
                uiState = UIState.STATE_COVERED;
                break;
            case UIState.STATE_COVERED:
                //do something
                //set the next state
                uiState = UIState.STATE_IDLE;
                break;
        }
    }
  
    public void WinterIsComing(bool isActive)
    {
        freezeVfx.SetActive(isActive);
    }
    public void CleanUp(GameObject objectToMantain)
    {
        foreach (var panel in list)
        {
            if (panel != objectToMantain)
            {
                panel.SetActive(false);
            }
        }
    }

    public void ShowAll()
    {
        foreach (var panel in list)
        {
            panel.SetActive(true);
        }
    }

    

    public void EquipUI()
    {
        equipDeco.SetActive(true);
        equipWheel.SetActive(true);
    }

    public void UnEquipUI()
    {
        equipDeco.SetActive(false);
        equipWheel.SetActive(false);
    }
    
    public void UpdateMovementUI()
    {
        
        NormalMovement.SetActive(true);
        //Debug.Log("Camera Level: " + rotation_Camera.MyCameraInstance.cameraLevel);
        if (rotation_Camera.MyCameraInstance.cameraLevel == 1)
        {
            //FlatMovement.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            //FlatMovement.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (buildController.MyBuildInstance.IsFloor(rotation_Camera.MyCameraInstance.cameraLevel) && (buildController.MyBuildInstance.floorsList.Count - 1) == buildController.MyBuildInstance.GetBuildingPoint())
        {
            FlatMovement.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            FlatMovement.transform.GetChild(0).gameObject.SetActive(true);
        }

        if(rotation_Camera.MyCameraInstance.cameraLevel == rotation_Camera.MyCameraInstance.maxLevel)
        {
            NormalMovement.transform.GetChild(0).gameObject.SetActive(false);
            //NormalMovement.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            NormalMovement.transform.GetChild(0).gameObject.SetActive(true);
            //NormalMovement.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (rotation_Camera.MyCameraInstance.cameraLevel == 1)
        {
            //NormalMovement.transform.GetChild(2).gameObject.SetActive(false);
            NormalMovement.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            //NormalMovement.transform.GetChild(2).gameObject.SetActive(true);
            NormalMovement.transform.GetChild(1).gameObject.SetActive(true);
        }

        ActionSpaceOrg();
    }
    public void ShowBuildingUI()
    {
    }

    public void ActionSpaceOrg()
    {
        if (LevelTraveler.MyTravelInstance.Level.isTutorial == true && TutorialManager.Instance.GetPhase() == TutorialManager.GAMEPLAY_TUTORIAL_PHASE.TOWERATTACK)
        {
            return;
        }
        if (rotation_Camera.MyCameraInstance.cameraLevel == rotation_Camera.MyCameraInstance.maxLevel)
        {
            AttackCover.SetActive(false);
        }
        else
        {
            AttackCover.SetActive(true);
        }

        if (rotation_Camera.MyCameraInstance.cameraLevel == rotation_Camera.MyCameraInstance.maxLevel && buildController.MyBuildInstance.IsFloor() == true)
        {
            
            AttackCover.SetActive(false);
        }

        if(buildController.MyBuildInstance.IsFloor() == false)
        {
            //Debug.Log("It is not a floor!");
            
        }

        if (buildController.MyBuildInstance.IsFloor() == true && buildController.MyBuildInstance.IsTown() == false)
        {
            
        }

        if (rotation_Camera.MyCameraInstance.cameraLevel == rotation_Camera.MyCameraInstance.maxLevel && buildController.MyBuildInstance.IsFloor() == true && buildController.MyBuildInstance.IsTown() == true)
        {
            Debug.Log("Attack, Floor and Town!");
            
            AttackCover.SetActive(false);
            
        }

        if (rotation_Camera.MyCameraInstance.cameraLevel != rotation_Camera.MyCameraInstance.maxLevel && buildController.MyBuildInstance.IsFloor() == true && buildController.MyBuildInstance.IsTown() == true)
        {
            Debug.Log("Attack, Floor and Town!");
            
            AttackCover.SetActive(true);
            
        }
    }

    public void ShowMiniMap()
    {
        mapShown = !mapShown;
        if (LevelTraveler.MyTravelInstance.Level.isTutorial == true)
        {
            TutorialManager.Instance.NextPhase(TutorialManager.GAMEPLAY_TUTORIAL_PHASE.MAP);
        }
        if (mapShown == false)
        {
            Debug.Log("hiding");
            miniMapBillboard.transform.localPosition = new Vector3(hideShow.x - 41.8f, 0f, 0f);
        }
        else if (mapShown == true)
        {
            Debug.Log("showing");
            miniMapBillboard.transform.localPosition = new Vector3(hideShow.y - 41.8f, 0f, 0f);
        }
    }
    public void HideBuildingUI()
    {
        //NormalMovement.SetActive(true);
        //FlatMovement.SetActive(false);
        //SHOW NORMAL MOVEMENT
        //HIDE MOVEMENT BETWEEN FLATS
    }

    public void EndScreen(bool _success)
    {
        if (_success == true)
        {
            finishPanel.SetActive(true);
            finishButton.SetActive(true);


            GameObject newObjective = new GameObject();
            newObjective = Instantiate(rewardPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, rewardParent.transform);

            newObjective.GetComponent<RewardHolder>().SetState(RewardHolder.rewardState.OBJECTIVE);
            if (LevelTraveler.MyTravelInstance != null)
            {
                newObjective.GetComponent<RewardHolder>().SetQuestsCompleted(LevelTraveler.MyTravelInstance.Level.Objectives.questBuffer);

            }
            newObjective.GetComponent<RewardHolder>().RevealFeedback();


            foreach (Rewards item in LevelTraveler.MyTravelInstance.Level.Rewards)
            {
                GameObject separator = new GameObject();
                separator = Instantiate(separatorPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, rewardParent.transform);

                GameObject newReward = new GameObject();
                newReward = Instantiate(rewardPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, rewardParent.transform);

                newReward.GetComponent<RewardHolder>().SetState(RewardHolder.rewardState.REWARD);
                newReward.GetComponent<RewardHolder>().SetReward(item);

                newReward.GetComponent<RewardHolder>().RevealFeedback();
            }
        }
        else
        {
            switch (buildController.MyBuildInstance.myState)
            {
                case buildController.AliveState.DEADBYSIGN:
                    failPanel.SetActive(true);
                    failButton.SetActive(true);
                    failedText.text = "Never stop dragging! You offended the king.";
                    break;
                case buildController.AliveState.DEADBYFREEZE:
                    failPanel.SetActive(true);
                    failButton.SetActive(true);
                    failedText.text = "It was too cold out there...";
                    break;
                case buildController.AliveState.DEADBYQUIT:
                    failPanel.SetActive(true);
                    failButton.SetActive(true);
                    failedText.text = "Bye(?)...";
                    break;
                default:
                    break;
            }
        }
        

        

        //newObjective.GetComponent<RewardHolder>().Feedbacks();

    }
    public GameObject SetFeedback(Sprite icon, int quantity)
    {
        craftFeedbackCard.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = icon;
        Debug.Log("Resources Quant:" + quantity);
        craftFeedbackCard.GetComponentInChildren<TextMeshProUGUI>().text = "+" + quantity.ToString();

        return craftFeedbackCard;
    }

    public IEnumerator feedbackMomento(GameObject parent, Sprite icon, int quantity)
    {
        GameObject newFeedback = Instantiate(SetFeedback(icon ,quantity), Vector3.zero, Quaternion.identity);
        newFeedback.gameObject.transform.SetParent(parent.transform);
        newFeedback.gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        newFeedback.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
        yield return new WaitForSeconds(1.3f);

        Destroy(newFeedback);
        
        yield break;
    }

    public void InitializeSpell(int numberOfSteps)
    {
        SpellSpace.SetActive(true);
        //number of steps is decreased by 1 --
        for (int i = 0; i < numberOfSteps; i++)
        {
            Instantiate(progressDotPrefab, ProgressLayout.transform);
        }

    }

    public void SpellProgressCheck(int tick)
    {
        if (ProgressLayout.transform.GetChild(tick) != null)
            ProgressLayout.transform.GetChild(tick).GetComponent<DotLayoutController>().Reached();
    }

    public void CleanUpSpell()
    {
        StopCoroutine("AlphaPingPongModifier");
        arcaneRuneDeco.color = new Color(arcaneRuneDeco.color.r, arcaneRuneDeco.color.g, arcaneRuneDeco.color.b, 98f);
        SpellSpace.SetActive(false);
        foreach (Transform child in ProgressLayout.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    public void FloatTextUIUpdate()
    {
        //this.GetComponent<buildController>().camera.GetComponent<rotation_Camera>().Up();
    }

    public void DaySwitch(bool isDay)
    {
        dayBackground.SetActive(isDay);
        nightBackground.SetActive(!isDay);
    }

    public void DayAnimation(bool isDay)
    {
        sunAnimator.SetBool("isDay", isDay);
    }

    public void NightAnimation(bool isDay)
    {
        moonAnimator.SetBool("isDay", isDay);
    }

    public void DayAdd(int day)
    {
        
    }
    public void BagSwitchUI()
    {
        open = !open;
        if (open == true)
        {
            InitializeSkillMenu(open);
        }
        else if (open == false)
        {
            CleanUpSkillMenu(open);
            skillController.MySkillInstance.skillButtonScript.ActualMode = skillButton.MODE.SUMMONING;
            UnEquipUI();
        }
    }

    private void InitializeSkillMenu(bool open)
    {
        foreach (var item in this.transform.GetComponent<InventoryController>().GetSkillList())
        {
            item.GetComponent<skillHolder>().UpdateUI();
        }
    }

    private void CleanUpSkillMenu(bool open)
    {

    }

    public void FaintRuneLight(Color spellColor)
    {
        var r = spellColor.r;
        var g = spellColor.g;
        var b = spellColor.b;
        var originalAlpha = 22f;

        arcaneRuneDeco.color = new Color(r, g, b, originalAlpha);

    }

    public void NormalRuneLight(Color spellColor)
    {
        var r = spellColor.r;
        var g = spellColor.g;
        var b = spellColor.b;
        var originalAlpha = spellColor.a;

        arcaneRuneDeco.color = new Color(r,g,b, originalAlpha);
    }

    public void ReadyRuneLight()
    {
        var tempColor = arcaneRuneDeco.color;
        tempColor.a = 120f;
        arcaneRuneDeco.color = tempColor;
    }

    public void ShineRuneLight(Color spellColor)
    {
        //Debug.Log("SHINING");
        var r = spellColor.r;
        var g = spellColor.g;
        var b = spellColor.b;
        var originalAlpha = spellColor.a;
        StartCoroutine(AlphaPingPongModifier(arcaneRuneDeco, new Color(r,g,b, originalAlpha), new Color(r, g, b, 255f), 0.25f));
    }

    public IEnumerator AlphaPingPongModifier(Image colorToModify, Color originalColor, Color newColor, float seconds)
    {
        float speed = Time.deltaTime;
        float time = 0;
        int iteration = 0;

        float temporalColor = colorToModify.color.a;
 
        while (time <= seconds)
        {
            colorToModify.color = Color.Lerp(originalColor, newColor, Map(temporalColor, originalColor.a, newColor.a, 0, 1));
            yield return new WaitForSeconds(speed);
            temporalColor += Time.deltaTime;
            time += speed;
            iteration++;
        }

        time = 0;
        
        while (time <= seconds)
        {
            colorToModify.color = Color.Lerp(originalColor, newColor, Map(Mathf.Abs(temporalColor), originalColor.a, newColor.a, 0, 1));
            yield return new WaitForSeconds(speed);
            temporalColor -= Time.deltaTime;
            time += speed;
            iteration++;
        }


        yield break;
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        //vida escalable con vida actual
    }

    public void SlotListActivation(bool isActive)
    {
        ActionBarSpace.SetActive(!isActive);
        //WorkshopSpace.SetActive(!isActive);
    }

    public static RectTransform SetAnchoredPos(AnchorPresets newAnchor, RectTransform rectToSet, RectTransform parent = null)
    {
        if (parent != null)
        {
            Debug.Log("Parenting!" + rectToSet.name + "with" + parent.name);
            rectToSet.transform.SetParent(parent.transform);
        }
        else
        {
            Debug.LogError("Parent Was null");
        }
        //create a switch that takes the newAnchor and modifies the Anchors min and max values aswell as the pivot in order to math the logical anchor of the enum
        switch (newAnchor)
        {
            case AnchorPresets.TopLeft:
                rectToSet.anchorMin = new Vector2(0, 1);
                rectToSet.anchorMax = new Vector2(0, 1);
                rectToSet.pivot = new Vector2(0, 1);
                break;
            case AnchorPresets.TopCenter:
                rectToSet.anchorMin = new Vector2(0.5f, 1);
                rectToSet.anchorMax = new Vector2(0.5f, 1);
                rectToSet.pivot = new Vector2(0.5f, 1);
                break;
            case AnchorPresets.TopRight:
                rectToSet.anchorMin = new Vector2(1, 1);
                rectToSet.anchorMax = new Vector2(1, 1);
                rectToSet.pivot = new Vector2(1, 1);
                break;
            case AnchorPresets.MiddleLeft:
                rectToSet.anchorMin = new Vector2(0, 0.5f);
                rectToSet.anchorMax = new Vector2(0, 0.5f);
                rectToSet.pivot = new Vector2(0, 0.5f);
                break;
            case AnchorPresets.MiddleCenter:
                rectToSet.anchorMin = new Vector2(0.5f, 0.5f);
                rectToSet.anchorMax = new Vector2(0.5f, 0.5f);
                rectToSet.pivot = new Vector2(0.5f, 0.5f);
                break;
            case AnchorPresets.MiddleRight:
                rectToSet.anchorMin = new Vector2(1, 0.5f);
                rectToSet.anchorMax = new Vector2(1, 0.5f);
                rectToSet.pivot = new Vector2(1, 0.5f);
                break;
            case AnchorPresets.BottomLeft:
                rectToSet.anchorMin = new Vector2(0, 0);
                rectToSet.anchorMax = new Vector2(0, 0);
                rectToSet.pivot = new Vector2(0, 0);
                break;
            case AnchorPresets.BottomCenter:
                rectToSet.anchorMin = new Vector2(0.5f, 0);
                rectToSet.anchorMax = new Vector2(0.5f, 0);
                rectToSet.pivot = new Vector2(0.5f, 0);
                break;
        }
        //then set the position to 0 in X and Y
        rectToSet.anchoredPosition = Vector2.zero;
        
        return rectToSet;
    }
}
