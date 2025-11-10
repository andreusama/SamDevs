using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Debugger : MonoBehaviour
{
    //KNOW CAMERA LEVEL
    // Start is called before the first frame update
    public int target = 60;
    public bool godMode = false;
    public GameObject debugPanel;

    public Text timeText;
    public Text questCompleted;
    public bool active;
    private static Debugger towerInstance;

    public float totalGoldDebug = 0;
    public Text totalGoldWinned;
    public Text lastGoldHit;
    public float lastGold = 0;
    public float totalResDebug = 0;
    public Text totalResWinned;
    //movement debug
    public Text cameraLvl;
    public Text towerLvl;
    public Text buildingPoint;
    public Text floorLevel;
    public Text actualFloor;

    public List<Image> patternDebug;
    
    private float time;
    public static Debugger MyTowerInstance
    {
        get
        {
            if (towerInstance == null)
            {
                towerInstance = FindObjectOfType<Debugger>();
            }
            return towerInstance;
        }


    }

    void Start()
    {
        active = false;
        Physics.reuseCollisionCallbacks = true;
        debugPanel.SetActive(false);
    }
    void Awake()
    {
        Application.targetFrameRate = target;
    }

    void Update()
    {
        if (Application.targetFrameRate != target)
            Application.targetFrameRate = target;

        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("God!");
            godMode = !godMode;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            debugPanel.SetActive(true);
            questCompleted.text = "Quest Pool" + QuestController.MyQuestInstance.questPool.ToString();

            questCompleted.text = "Quest Pool" + QuestController.MyQuestInstance.questPool.ToString();
            totalGoldWinned.text = "Tot G:" + totalGoldDebug.ToString();
            totalResWinned.text = "Tot R" + totalResDebug.ToString();
            timeText.text = "Time" + time.ToString();
            cameraLvl.text = "Camera Lvl: " + rotation_Camera.MyCameraInstance.cameraLevel.ToString();
            towerLvl.text = "Tower Lvl: " + buildController.MyBuildInstance.level;
            buildingPoint.text = "Building Point Lvl: " + buildController.MyBuildInstance.GetBuildingPoint().ToString();

            if(buildController.MyBuildInstance.floorsList.ContainsKey(rotation_Camera.MyCameraInstance.cameraLevel))
            {
                floorLevel.text = "Floor Level Point Lvl: " + buildController.MyBuildInstance.floorsList[rotation_Camera.MyCameraInstance.cameraLevel].level.ToString();
            }

            if (buildController.MyBuildInstance.floorsList != null)
            {
                actualFloor.text = "Number of Floors: " + buildController.MyBuildInstance.floorsList.Count.ToString();
            }
        }
        else if(Input.GetKeyUp(KeyCode.F2))
        {
            debugPanel.SetActive(false);
        }
        else if (Input.GetKeyUp(KeyCode.U))
        {
            skillController.MySkillInstance.castCancelled = true;
            FeedbackController.MyFeedbackInstance.signSetUpFeedbacks.PlayFeedbacks();
        }
        timeText.text = "Time" + time.ToString();
        time += Time.deltaTime;

    }

    public Skill debugSpell;

    public void SetPatternDebug(int position, bool activated)
    {
        //Debug.Log("Receiving position" + position);
        if (activated)
        {
            //Debug.Log("Activating in position: " + position);
            patternDebug[position].color = Color.blue;
        }
        else
        {
            patternDebug[position].color = Color.white;
        }

        if (position == 0)
        {
            patternDebug[patternDebug.Count - 1].transform.localScale = new Vector3(1, 1, 1);
            patternDebug[position].transform.localScale = new Vector3(1, 2, 1);
        }
        else
        {
            patternDebug[position].transform.localScale = new Vector3(1, 2, 1);
            patternDebug[position - 1].transform.localScale = new Vector3(1, 1, 1);
        }
        
    }
    // Update is called once per frame
    
}
