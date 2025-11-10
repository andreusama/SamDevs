using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class Step
{
    [Header("Text to display.")]
    public string phrase = "";
    [Header("Black Bool")]
    public bool black;

    [Header("Rect where the HAND feedback will face.")]
    public RectTransform highlightedRect;
    [Header("Rect where the ARROW feedback will face.")]
    public List<RectTransform> signalToRect = new List<RectTransform>();

    private GameObject hand = null;
    private List<GameObject> arrows = new List<GameObject>();

    [Header("Side Objects to Activate")]
    public List<GameObject> sideObjects = new List<GameObject>();
    public enum IntantiateZone
    {
        TOP,
        MIDDLE,
        DOWN
    }

    [SerializeField]
    public UIController.AnchorPresets anchorPos;

    [SerializeField]
    public IntantiateZone zone;

    public void Play(TutorialPhase thisPhase, TextMeshProUGUI messageText)
    {
        messageText.text = phrase;
        
        if (thisPhase.blackUI != null && black == true)
        {
            thisPhase.blackUI.SetActive(true);
        }

        //we are parenting the message transform not the message text
        UIController.SetAnchoredPos(anchorPos, (RectTransform)TutorialManager.Instance.messageTransform, (RectTransform)TutorialManager.Instance.tutorialPivotPoints[(int)zone]);

        if (hand == null && highlightedRect != null)
        {
            if ((RectTransform)TutorialManager.Instance.messageTransform.GetComponent<tutorialHolder>().button.transform != highlightedRect.gameObject.GetComponent<RectTransform>())
            {
                TutorialManager.Instance.messageTransform.GetComponent<tutorialHolder>().button.SetActive(false);
            }
            else
            {
                TutorialManager.Instance.messageTransform.GetComponent<tutorialHolder>().button.SetActive(true);
            }

            InstantiateHand();
        }

        if (arrows.Count < signalToRect.Count && signalToRect.Count > 0)
            InstantiateArrows();

        foreach (var item in sideObjects)
        {
            item.SetActive(true);
        }
    }

    private void InstantiateHand()
    {
        hand = FeedbackController.MyFeedbackInstance.InstantiateFeedbackHand();
        UIController.SetAnchoredPos(UIController.AnchorPresets.MiddleCenter, (RectTransform)hand.transform, highlightedRect);
        //set the position to 0,0 
    }

    private void InstantiateArrows()
    {
        foreach (var item in signalToRect)
        {
            arrows.Add(FeedbackController.MyFeedbackInstance.InstantiateFeedbackArrow());
            UIController.SetAnchoredPos(UIController.AnchorPresets.MiddleCenter, (RectTransform)arrows[arrows.Count - 1].transform, signalToRect[arrows.Count - 1]);
        }
    }

    public void CleanUp()
    {
        Debug.Log("Cleaning up the hand!");
        GameObject.Destroy(hand);
        foreach (var item in sideObjects)
        {
            item.SetActive(false);
        }
        foreach (var item in arrows)
        {
            GameObject.Destroy(item);
        }
    }
}

[Serializable]
public class TutorialPhase
{
    [Header("Phase Name")]
    public string phaseName = "";
    [Header("Number of Steps")]
    public List<Step> steps = new List<Step>();
    [Header("Black Background Image.")]
    GameObject blackImage = null;
    [Header("Action Phase.")]
    public TutorialManager.GAMEPLAY_TUTORIAL_PHASE phase;
    [Header("Phase Objects to Activate")]
    public List<GameObject> sideObjects;

    public int actualStep = 0;

    public GameObject blackUI;

    public void DisplayStep(TextMeshProUGUI textToSet)
    {
        foreach (var item in sideObjects)
        {
            item.SetActive(true);
        }
        Debug.Log("Actual Step: " + actualStep);
        if (steps[actualStep] != null)
        {
            steps[actualStep].Play(this, textToSet);
        }
    }

    public void CleanUp()
    {
        if (blackUI != null)
        {
            blackUI.SetActive(false);
        }
        foreach (var item in steps)
        {
            item.CleanUp();
        }
        foreach (var item in sideObjects)
        {
            item.SetActive(false);
        }
    }

    
}
public class TutorialManager : MonoBehaviour
{
    private static TutorialManager instance;
    public static TutorialManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TutorialManager>();
            }
            return instance;
        }
    }
    //first the tower
    //second the bag
    //third the attack
    //then the quests
    //finally the map
    public List<Transform> tutorialPivotPoints;

    private GameObject questsPanel;
    private GameObject movementPanel;
    private GameObject skillButtonCover;
    private GameObject bagPanel;
    private GameObject tutorialPanel;
    private GameObject exitPanel;

    public Transform messageTransform;

    public bool isTutorial;

    public List<TutorialPhase> tutorialPhases = new List<TutorialPhase>();

    private bool isDirty = false;

    #region GamePlayTutorial
    [Serializable]
    public enum GAMEPLAY_TUTORIAL_PHASE
    {
        INTRODUCTION,
        ATTACK,
        TOWERATTACK,
        ACTIVE,
        CONTRACT,
        QUEST,
        MAP,
        FREEPLAY,
        SIGNING,
        REWARDS,
        END
    }

    [SerializeField]
    private GAMEPLAY_TUTORIAL_PHASE currentPhase;

    public GAMEPLAY_TUTORIAL_PHASE GetPhase()
    {
        return currentPhase;
    }
    #endregion
    #region menuTutorial
    [Serializable]
    public enum MENU_TUTORIAL_PHASE
    {
        WELCOME,
        TOPIC,
        GOLD_MANA,
        WORKSHOP_EQUIPPED,
        WORKSHOP_GEAR,
        WORKSHOP_LEVELUP,
        RECRUITMENT_INTRO,
        RECRUITMENT_DAMAGE,
        RECRUITMENT_PASSIVES,
        PLAY,
        END
    }

    [SerializeField]
    private MENU_TUTORIAL_PHASE menuPhase;

    public MENU_TUTORIAL_PHASE GetMenuPhase()
    {
        return menuPhase;
    }
    #endregion
    private void Awake()
    {
        if (LevelTraveler.MyTravelInstance != null)
        {
            //Debug.Log("Tutorial Manager Awakes!");
            switch (LevelTraveler.MyTravelInstance.GetGameState())
            {
                case LevelTraveler.GameState.MENU:
                    //isDirty = true;
                    //menuPhase = MENU_TUTORIAL_PHASE.WELCOME;
                    break;
                case LevelTraveler.GameState.LEVEL:
                    Debug.Log("Case level!");
                    if (LevelTraveler.MyTravelInstance.Level.isTutorial == true)
                    {
                        Debug.Log("Is Tutorial");
                        isTutorial = true;
                    }
                    else
                    {
                        Debug.Log("Is NOT Tutorial");
                        isTutorial = false;
                    }
                    break;
                default:
                    break;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (LevelTraveler.MyTravelInstance != null)
        {
            //Debug.Log("Tutorial Manager Starts!");
            if (LevelTraveler.MyTravelInstance.GetGameState() == LevelTraveler.GameState.LEVEL)
            {
                questsPanel = UIController.MyUiInstance.questPanel;
                movementPanel = UIController.MyUiInstance.MovementPanel;
                skillButtonCover = UIController.MyUiInstance.AttackCover;
                bagPanel = UIController.MyUiInstance.BagSpace;
                exitPanel = UIController.MyUiInstance.exitPanel;
                //Debug.Log("Tutorial bool set to" + isTutorial);
                if (LevelTraveler.MyTravelInstance.Level.isTutorial == true)
                {
                    isTutorial = true;
                    currentPhase = GAMEPLAY_TUTORIAL_PHASE.INTRODUCTION;
                    isDirty = true;
                    StartCoroutine(Tutorial());
                }
                else
                {
                    isTutorial = false;
                    skillButtonCover.SetActive(false);
                }
            }
            if (LevelTraveler.MyTravelInstance.GetGameState() == LevelTraveler.GameState.MENU)
            {
                //if (isTutorial == true)
                //{
                //    StartCoroutine(MenuTutorial());
                //}
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            movementPanel.SetActive(true);
        }
    }

    public IEnumerator Tutorial()
    {
        UIController.MyUiInstance.tutorialScreen.SetActive(true);
        //what about going through steps instead of going through the enum?
        //this way we can create a list of steps and go through them or a scriptable object containing them
        //if I create a scriptable object named tutorial with all the info needed i can tutorialize things virtually
        //but then how de we do the menu?
        //it will be good to create a function tutorialize that takes a tutorial object and goes through it, step to step, it doesnt need to be a IEnumerator but it can be
        while (currentPhase != GAMEPLAY_TUTORIAL_PHASE.END)
        {
            switch (currentPhase)
            {
                case GAMEPLAY_TUTORIAL_PHASE.INTRODUCTION:
                    if (isDirty == true)
                    {
                        tutorialPhases[(int)currentPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);
                        questsPanel.SetActive(false);
                        movementPanel.SetActive(false);
                        skillButtonCover.SetActive(true);
                        bagPanel.SetActive(false);
                        exitPanel.SetActive(false);

                        isDirty = false;
                    } 
                    break;
                case GAMEPLAY_TUTORIAL_PHASE.ATTACK:
                    if (isDirty == true)
                    {
                        tutorialPhases[(int)currentPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);
                        skillButtonCover.SetActive(false);

                        isDirty = false;
                    }
                    break;
                case GAMEPLAY_TUTORIAL_PHASE.TOWERATTACK:
                    if (isDirty == true)
                    {
                        tutorialPhases[(int)currentPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);
                        isDirty = false;
                    }
                    break;
                case GAMEPLAY_TUTORIAL_PHASE.ACTIVE:
                    if (isDirty == true)
                    {
                        LevelTraveler.MyTravelInstance.Level.GetBioma().SetBioma(true);
                        LevelTraveler.MyTravelInstance.Level.GetBioma().StartBioma(true);
                        tutorialPhases[(int)currentPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);
                        isDirty = false;
                    }
                    break;
                case GAMEPLAY_TUTORIAL_PHASE.CONTRACT:
                    if (isDirty == true)
                    {
                        LevelTraveler.MyTravelInstance.Level.GetBioma().StopBioma(false);
                        LevelTraveler.MyTravelInstance.Level.GetBioma().EndBioma(false);
                        tutorialPhases[(int)currentPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);
                        isDirty = false;
                    }
                    break;
                case GAMEPLAY_TUTORIAL_PHASE.QUEST:
                    if (isDirty == true)
                    {
                        tutorialPhases[(int)currentPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);
                        questsPanel.SetActive(true);
                        movementPanel.SetActive(false);
                        bagPanel.SetActive(true);
                        isDirty = false;
                    }
                    
                    break;
                case GAMEPLAY_TUTORIAL_PHASE.MAP:
                    if (isDirty == true)
                    {
                        tutorialPhases[(int)currentPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);
                        movementPanel.SetActive(true);
                        skillButtonCover.SetActive(true);
                        isDirty = false;
                    }
                    
                    break;
                case GAMEPLAY_TUTORIAL_PHASE.FREEPLAY:
                    if (isDirty == true)
                    {
                        tutorialPhases[(int)currentPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);
                        isDirty = false;
                    }
                    break;
                case GAMEPLAY_TUTORIAL_PHASE.SIGNING:
                    if (isDirty == true)
                    {
                        tutorialPhases[(int)currentPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);
                        isDirty = false;
                    }

                    break;
                case GAMEPLAY_TUTORIAL_PHASE.REWARDS:
                    if (isDirty == true)
                    {
                        tutorialPhases[(int)currentPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);
                        buildController.MyBuildInstance.SucessEnd();
                        isDirty = false;
                    }
                    break;
                default:
                    break;
            }
            yield return null;
        }
        UIController.MyUiInstance.tutorialScreen.SetActive(false);
        isTutorial = false;
        yield break; 
    }
    public IEnumerator MenuTutorial()
    {
        MenuUI.MyMenuUiInstance.tutorialPanel.SetActive(true);
        
        while (menuPhase != MENU_TUTORIAL_PHASE.END)
        {
            switch (menuPhase)
            {
                case MENU_TUTORIAL_PHASE.WELCOME:
                    if (isDirty == true)
                    {
                        tutorialPhases[(int)menuPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);

                        isDirty = false;
                    }
                    break;
                case MENU_TUTORIAL_PHASE.TOPIC:
                    if (isDirty == true)
                    {
                        tutorialPhases[(int)menuPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);

                        isDirty = false;
                    }
                    break;
                case MENU_TUTORIAL_PHASE.GOLD_MANA:
                    if (isDirty == true)
                    {
                        tutorialPhases[(int)menuPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);
                        isDirty = false;
                    }
                    break;
                case MENU_TUTORIAL_PHASE.WORKSHOP_EQUIPPED:
                    if (isDirty == true)
                    {
                        tutorialPhases[(int)menuPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);
                        isDirty = false;
                    }
                    break;
                case MENU_TUTORIAL_PHASE.WORKSHOP_GEAR:
                    if (isDirty == true)
                    {
                        tutorialPhases[(int)menuPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);
                        isDirty = false;
                    }
                    break;
                case MENU_TUTORIAL_PHASE.WORKSHOP_LEVELUP:
                    if (isDirty == true)
                    {
                        tutorialPhases[(int)menuPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);

                        isDirty = false;
                    }

                    break;
                case MENU_TUTORIAL_PHASE.RECRUITMENT_INTRO:
                    if (isDirty == true)
                    {
                        tutorialPhases[(int)menuPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);
                        movementPanel.SetActive(true);
                        skillButtonCover.SetActive(true);
                        isDirty = false;
                    }

                    break;
                case MENU_TUTORIAL_PHASE.RECRUITMENT_DAMAGE:
                    if (isDirty == true)
                    {
                        tutorialPhases[(int)menuPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);
                        isDirty = false;
                    }
                    break;
                case MENU_TUTORIAL_PHASE.RECRUITMENT_PASSIVES:
                    if (isDirty == true)
                    {
                        tutorialPhases[(int)menuPhase].DisplayStep(messageTransform.GetComponent<tutorialHolder>().text);
                        isDirty = false;
                    }
                    break;
                default:

                    break;
            }
            yield return null;
        }
        MenuUI.MyMenuUiInstance.tutorialPanel.SetActive(false);
        yield break;
    }
    public void NextPhase(GAMEPLAY_TUTORIAL_PHASE nextPhase)
    {
        Debug.Log("Going to phase: " + nextPhase.ToString().ToUpper());
        if (nextPhase != currentPhase)
        {
            return;
        }
        else
        {
            Debug.Log("Changing step of " + tutorialPhases[(int)currentPhase].ToString() + 
                "with a actual value of: " + tutorialPhases[(int)currentPhase].actualStep + "and an max steps of: " + tutorialPhases[(int)currentPhase].steps.Count); 
            //we only get the same phase or the nextOne
            if (tutorialPhases[(int)currentPhase].actualStep < tutorialPhases[(int)currentPhase].steps.Count - 1)
            {
                //if there is steps to do in the phase advance
                Debug.Log("Next step" + tutorialPhases[(int)currentPhase].actualStep);
                tutorialPhases[(int)currentPhase].steps[tutorialPhases[(int)currentPhase].actualStep].CleanUp();
                tutorialPhases[(int)currentPhase].blackUI.SetActive(false);
                tutorialPhases[(int)currentPhase].actualStep++;
                isDirty = true;

            }
            else
            {
                //if there is no steps to do advance to the next one
                tutorialPhases[(int)currentPhase].CleanUp();
                currentPhase = currentPhase + 1;
                Debug.Log("Changing to phase: " + currentPhase.ToString().ToUpper());
                isDirty = true;

            }
        }
    }

    public void NextPhase(MENU_TUTORIAL_PHASE nextPhase)
    {
        Debug.Log("Going to phase: " + nextPhase.ToString().ToUpper());
        if (nextPhase != menuPhase)
        {
            return;
        }
        else
        {
            Debug.Log("Changing step of " + tutorialPhases[(int)menuPhase].ToString() +
                "with a actual value of: " + tutorialPhases[(int)menuPhase].actualStep + "and an max steps of: " + tutorialPhases[(int)menuPhase].steps.Count);
            //we only get the same phase or the nextOne
            if (tutorialPhases[(int)menuPhase].actualStep < tutorialPhases[(int)menuPhase].steps.Count - 1)
            {
                //if there is steps to do in the phase advance
                Debug.Log("Next step" + tutorialPhases[(int)menuPhase].actualStep);
                tutorialPhases[(int)menuPhase].steps[tutorialPhases[(int)menuPhase].actualStep].CleanUp();
                tutorialPhases[(int)menuPhase].actualStep++;
                isDirty = true;

            }
            else
            {
                //if there is no steps to do advance to the next one
                tutorialPhases[(int)menuPhase].CleanUp();
                menuPhase = menuPhase + 1;
                Debug.Log("Changing to phase: " + menuPhase.ToString().ToUpper());
                isDirty = true;

            }
        }
    }
    public void NextStepButton()
    {
        switch (LevelTraveler.MyTravelInstance.GetGameState())
        {
            case LevelTraveler.GameState.MENU:
                NextPhase(menuPhase);

                break;
            case LevelTraveler.GameState.LEVEL:
                NextPhase(currentPhase);

                break;
            default:
                break;
        }
    }

}
