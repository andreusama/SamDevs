using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;

[System.Serializable]
public class FloorsList
{
    public Floors[] floor;
}
public class buildController : MonoBehaviour {

    #region variables
    [HideInInspector] public float forestResources = 1;
    [HideInInspector] public float volcanoResources = 1;
    [HideInInspector] public float lakeResources = 1;
    public GameObject megaIslandText;
    public GameObject towerParent;

    public GameObject forestTile;
    public GameObject volcanoTile;
    public GameObject riverTile;
    public GameObject selectionTile;
    public GameObject cleanTile;
    //TOWER PHASES
    [SerializeField]
    private List<GameObject> theTower;

    public GameObject firstQuartPrefab;
    public GameObject secondQuartPrefab;
    public GameObject thirdQuartPrefab;
    public GameObject fullPrefab;
    public GameObject normalPrefab;

    [HideInInspector]
    private GameObject firstStepBuilding;
    private GameObject secondStepBuilding;
    private GameObject thirdStepBuilding;
    private GameObject fourthStepBuilding;

    public List<GameObject> prefabsAir;

    public GameObject camera;
    public GameObject gc;

    public GameObject actualTower = null;
    public List<Vector3> towerPositions;

    public GameObject miniMapPrefab;
    public GameObject parentMiniMap;

    public float archipelagoCosts = 0f;
    private float actualIslandCost = 500;
    private float lastIslandCost = 0;

    public void SetIslandCost(float newCost)
    {
        actualIslandCost = newCost;
    }

    public float GetActualIslandCost()
    {
        return actualIslandCost;
    }

    private float actualXp = 0;

    public float progressScale = 100;

    //tracks the actual flat where you will build 
    private int actualBuildingPoint = 0;
    private Floors actualFloor;

    public List<string> townNames;

    public void SetActualFloor(Floors newFloor)
    {
        actualFloor = newFloor;
    }

    public Floors GetActualFloor()
    {
        return actualFloor;
    }

    //tile to be purchased
    private islandHolder purchasedIsland;

    public void SetPurchasedIsland(islandHolder newIsland)
    {
        purchasedIsland = newIsland;
    }
    public islandHolder GetPurchasedIsland()
    {
        return purchasedIsland;
    }
    public int GetBuildingPoint()
    {
        return actualBuildingPoint;
    }

    public void SetBuildingPoint(int value)
    {
        actualBuildingPoint = value;
    }

    public void SetActualXp(float value)
    {
        actualXp = value;
    }
    public float GetMaxHp()
    {
        return actualXp;
    }

    public void SetLastXp(float value)
    {
        lastXp = value;
    }
    public float GetLastMaxHp()
    {
        return lastXp;
    }
    private float lastXp = 24.5f;

    public bool build;
    bool built1 = false;
    bool built2 = false;
    bool built3 = false;
    bool built4 = false;
    bool builtR = false;

    GameObject islandSelected;

    private float textTransform;
    public Vector3 vec;
    private Quaternion quat;
    public int buildingLevelMargin;
    public int biomaMargin;

    public List<Vector3> hexList;
    public List<Vector3> selectionVec;
    public List<GameObject> selectionGO;
    public Dictionary<int, Floors> floorsList;
    public List<Zone> zonesList;

    public int level;
    private float repairValue;

    float lifeFraction = 0f;

    public MMFeedbacks DecreaseFeedback;
    public GameObject islandsParent;

    [Header("Floating Text Variables")]

    private int textXDir = 0;
    private int textYDir = 0;

    private static buildController buildInstance;

    public static buildController MyBuildInstance
    {
        get
        {
            if (buildInstance == null)
            {
                buildInstance = FindObjectOfType<buildController>();
            }
            return buildInstance;
        }


    }

    public float ActualIslandCost { get => actualIslandCost; set => actualIslandCost = value; }
    public float LastIslandCost { get => lastIslandCost; set => lastIslandCost = value; }

    #endregion
    private void Awake()
    {
        floorsList = new Dictionary<int, Floors>();
        gc = GameObject.FindWithTag("GameController");
        textXDir = 1;
    }
    // Use this for initialization
    void Start ()
    {
        level = 1;
        myState = AliveState.ALIVE;
        actualXp = PlayerStats.MyInstance.Salud.VidaM;

        SumLife(0);
        lastXp = 0;
        build = false;
        quat = new Quaternion(0, 0.7071068f, 0, 0.7071068f);
        progress = Progress.WORKING;
        if (LevelTraveler.MyTravelInstance != null)
            SetHpStartingPoint(LevelTraveler.MyTravelInstance.Level.Objectives.startingProgressPoint);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (InLevelSaveLoad.MySaveController.newGame == false)
        {
            if (InLevelSaveLoad.MySaveController.loading == true)
                return;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Tower Position 1+ " + towerPositions[0]);
            Debug.Log("Floor Level Position 1+ " + floorsList[1].checkPoint);

            Debug.Log("Tower Position 2+ " + towerPositions[1]);
            Debug.Log("Floor Level Position 2+ " + floorsList[2].checkPoint);

            Debug.Log("Tower Position 3+ " + towerPositions[2]);
            Debug.Log("Floor Level Position 3+ " + floorsList[3].checkPoint);

            Debug.Log("Camera Level: " + rotation_Camera.MyCameraInstance.cameraLevel);
        }

       

 
    }

    public enum Progress
    {
        WORKING,
        FINISHED
    }

    [SerializeField]
    private Progress progress;


    public Progress GetProgress()
    {
        return progress;
    }

    public enum AliveState
    {
        ALIVE,
        DEADBYSIGN,
        DEADBYFREEZE,
        DEADBYQUIT
    }

    public AliveState myState = AliveState.ALIVE;
    
    public void RefreshBuyableIslands()
    {
        foreach (var floor in floorsList.Values)
        {
            if (floor.claimed == false || floor.edificable == false)
                continue;

            foreach (Island island in floor.islands)
            {
                if (island.type == Island.Type.BUILDABLE)
                {
                    island.RefreshBuyable();
                }
            }
        }
    }

    public void CreateArchipelago()
    {
        ProgressionController.MyProgressionController.HpQuestPrice = 0;
        build = false;
        CheckIfFloorAndChangeBuildingPoint();
        if (floorsList.ContainsKey(actualBuildingPoint))
        {
            Debug.Log("Building Point on" + actualBuildingPoint + "is included // 1!");
        }
        
        for (int i = 0; i < floorsList[1].islands.Count; i++)
        {
            if (floorsList[actualBuildingPoint].edificable == false)
            {
                continue;
            }

            Debug.Log("Building Point: " + actualBuildingPoint);
            float lastCostBuffer = actualIslandCost;

            //here we demand the building of the new Island and we pass the localPosition ID and the price in order to set the future one
            floorsList[actualBuildingPoint].islands[i].Build = true;
            floorsList[actualBuildingPoint].islands[i].Price = actualIslandCost;
            floorsList[actualBuildingPoint].islands[i].id = i;

            ProgressionController.MyProgressionController.HpQuestPrice += actualIslandCost;
            actualIslandCost = actualIslandCost + (actualIslandCost - lastIslandCost + 50);
            lastIslandCost = lastCostBuffer;
            floorsList[actualBuildingPoint].claimed = true;
            buildController.MyBuildInstance.RefreshBuyableIslands();
            UIController.MyUiInstance.HideBuildingUI();
        }
    }

    
    private GameObject selectRandIsland()
    {
        int rand = Random.Range(1, 4);
        
        switch(rand)
        {
            case 1:
                islandSelected = forestTile;
                break;
            case 2:
                islandSelected = volcanoTile;
                break;
            case 3:
                islandSelected = riverTile;
                break;
            default:
                break;
        }

        Debug.Log("Island Randomly Selected:" + islandSelected.name);
        return islandSelected;
    }

    public List<Island> createSelectionTiles(Vector3 originVec)
    {
            //VirtualArchipelagoPrice(6);
            //ProgressionController.MyProgressionController.VirtualArchipelagoCalc();
            List<Island> islandHolders = new List<Island>(6);
            //new vectors
            /*
            Vector3 vec1H = new Vector3(harcodedPositions[1].transform.position.x + originVec.x, originVec.y, harcodedPositions[1].transform.position.z + originVec.z);
            Vector3 vec2H = new Vector3(harcodedPositions[2].transform.position.x + originVec.x, originVec.y, harcodedPositions[2].transform.position.z + originVec.z);
            Vector3 vec3H = new Vector3(harcodedPositions[3].transform.position.x + originVec.x, originVec.y, harcodedPositions[3].transform.position.z + originVec.z);
            Vector3 vec4H = new Vector3(harcodedPositions[4].transform.position.x + originVec.x, originVec.y, harcodedPositions[4].transform.position.z + originVec.z);
            Vector3 vec5H = new Vector3(harcodedPositions[5].transform.position.x + originVec.x, originVec.y, harcodedPositions[5].transform.position.z + originVec.z);
            Vector3 vec6H = new Vector3(harcodedPositions[6].transform.position.x + originVec.x, originVec.y, harcodedPositions[6].transform.position.z + originVec.z);
            */

            Vector3 vec1 = new Vector3((-0.61f - 0.45f + 0.04f) + originVec.x, originVec.y, (2.08f) + originVec.z);//right front
            Vector3 vec2 = new Vector3((1.57f + 0.27f) + originVec.x, originVec.y, (2.08f) + originVec.z);//left front
            Vector3 vec3 = new Vector3((-1.85f - 0.14f) + originVec.x, originVec.y, (0 * 1.75f) + originVec.z);//far right one
            Vector3 vec4 = new Vector3((2.92f - 0.14f) + originVec.x, originVec.y, (0 * 1.75f) + originVec.z);//far left one
            Vector3 vec5 = new Vector3((-0.55f - 0.74f) + originVec.x, originVec.y, (-2.08f) + originVec.z);//right back
            Vector3 vec6 = new Vector3((1.48f + 0.64f) + originVec.x, originVec.y, (-2.08f) + originVec.z);//left back

            if (CanAllocate(vec1, hexList))
            {
                if(CanAllocate(vec1, selectionVec))
                {
                    GameObject hex1 = Instantiate(selectionTile, vec1, quat, islandsParent.transform);//up-left 1 -1,1
                    hex1.GetComponent<islandHolder>().islandClass = new Empty("empty", vec1);
                    islandHolders.Add(hex1.GetComponent<islandHolder>().islandClass);
                    selectionVec.Add(vec1);
                }
            }
            if (CanAllocate(vec2, hexList))
            {
                if (CanAllocate(vec2, selectionVec))
                {
                    GameObject hex2 = Instantiate(selectionTile, vec2, quat, islandsParent.transform);//up-right 1,1
                    hex2.GetComponent<islandHolder>().islandClass = new Empty("empty", vec2);
                    islandHolders.Add(hex2.GetComponent<islandHolder>().islandClass);
                    selectionVec.Add(vec2);
                }
            }
            if (CanAllocate(vec3, hexList))
            {
                if (CanAllocate(vec3, selectionVec))
                {
                    GameObject hex3 = Instantiate(selectionTile, vec3, quat, islandsParent.transform);//middle left -2,0
                    hex3.GetComponent<islandHolder>().islandClass = new Empty("empty", vec3);
                    islandHolders.Add(hex3.GetComponent<islandHolder>().islandClass);
                    selectionVec.Add(vec3);
                }
            }
            if (CanAllocate(vec4, hexList))
            {
                if (CanAllocate(vec4, selectionVec))
                {
                    GameObject hex4 = Instantiate(selectionTile, vec4, quat, islandsParent.transform);//middle right 2,0
                    hex4.GetComponent<islandHolder>().islandClass = new Empty("empty", vec4);
                    islandHolders.Add(hex4.GetComponent<islandHolder>().islandClass);
                    selectionVec.Add(vec4);
                }
            }
            if (CanAllocate(vec5, hexList))
            {
                if (CanAllocate(vec5, selectionVec))
                {
                    GameObject hex5 = Instantiate(selectionTile, vec5, quat, islandsParent.transform);//low-left -1,-1
                    hex5.GetComponent<islandHolder>().islandClass = new Empty("empty", vec5);
                    islandHolders.Add(hex5.GetComponent<islandHolder>().islandClass);
                    selectionVec.Add(vec5);
                }
            }
            if (CanAllocate(vec6, hexList))
            {
                if (CanAllocate(vec6, selectionVec))
                {
                    GameObject hex6 = Instantiate(selectionTile, vec6, quat, islandsParent.transform);//low-right 1,-1
                    hex6.GetComponent<islandHolder>().islandClass = new Empty("empty", vec6);
                    islandHolders.Add(hex6.GetComponent<islandHolder>().islandClass);
                    selectionVec.Add(vec6);
                }
            }


        return islandHolders;
    }

    public void activateSelectionTile(Floors floor)
    {
        for (int i = 0; i < floor.islands.Count; i++)
        {
            if (floor.islands[i].type == Island.Type.EMPTY)
            {
                floor.islands[i].GetMeshState = Island.MeshState.toTrue;
            }
        }
    }

    public void deactivateSelectionTiles(Floors floor)
    {

        for (int i = 0; i < floor.islands.Count; i++)
        {
            if (floor.islands[i].type == Island.Type.EMPTY)
            {
                floor.islands[i].GetMeshState = Island.MeshState.toFalse;
            }
        }
        selectionVec.Clear();
    }


    public bool CanAllocate(Vector3 vec, List<Vector3> list)
    {
        foreach (Vector3 vector in list)
        {
            if (vector == vec)
            {
                return false;
            }
        }
        return true;
    }

    public void BuildMode()
    {
        build = !build;

        if (build == true)
        {
            for (int i = 0; i < floorsList.Count; i++)
            {
                //activateSelectionTile(floorsList[i]);
            }
        }
        else
        {
            for (int i = 0; i < floorsList.Count; i++)
            {
                //deactivateSelectionTiles(floorsList[i]);
            }
        }
    }

    public void SucessEnd()
    {
        UIController.MyUiInstance.getCoverStack().SetActive(true);
        progress = Progress.FINISHED;

        if (LevelTraveler.MyTravelInstance != null)
            LevelTraveler.MyTravelInstance.Level.state = Level.LevelState.Completed;
        
        PlayerStats.MyInstance.Salud.Vidactual = PlayerStats.MyInstance.Salud.VidaM;
        UIController.MyUiInstance.EndScreen(true);
    }

    public void DeadByQuit()
    {
        buildController.MyBuildInstance.myState = buildController.AliveState.DEADBYQUIT;
    }
    public void FailureEnd()
    {
        UIController.MyUiInstance.getCoverStack().SetActive(true);
        progress = Progress.FINISHED;
        LevelTraveler.MyTravelInstance.Level.state = Level.LevelState.Incompleted;


        UIController.MyUiInstance.EndScreen(false);
    }

    private void CheckFinish()
    {
        if (LevelTraveler.MyTravelInstance != null)
        {
            if (level == LevelTraveler.MyTravelInstance.Level.Objectives.numberOfLevels)
            {
                progress = Progress.FINISHED;
                UIController.MyUiInstance.getCoverStack().SetActive(true);
                //here there will be some deco things
                if (LevelTraveler.MyTravelInstance.Level.isTutorial == true)
                {
                    TutorialManager.Instance.NextPhase(TutorialManager.GAMEPLAY_TUTORIAL_PHASE.FREEPLAY);
                    FeedbackController.MyFeedbackInstance.tutorialSignFeedbacks.PlayFeedbacks();
                }
                else
                {
                    skillController.MySkillInstance.castCancelled = true;
                    FeedbackController.MyFeedbackInstance.signSetUpFeedbacks.PlayFeedbacks();
                }
            }
        }
        else
        {
            Debug.Log("Level traveler was null");
        }

    }

    public void SummonSign()
    {
        if (LevelTraveler.MyTravelInstance != null)
            LevelTraveler.MyTravelInstance.Gear.signature.SummonSpell();
        else
            Debugger.MyTowerInstance.debugSpell.SummonSpell();
    }
    public void LevelUp()
    {
        //STATS RESET
        //this check will be made every time we enter this in order to not level up if we're on the last flat
        //if level is ended
        CheckFinish();
        
        if (progress == Progress.FINISHED)
        {
            return;
        }

        LevelProgression(1);
        LevelUpDebug();
        QuestProgression();
        CameraProgression();
        
        //BUILD'S RESET
        built1 = false;
        built2 = false;
        built3 = false;
        built4 = false;
        builtR = false;

        NewFloor();
        NewBioma();
    }

    private void LevelProgression(int overlaps)
    {
        PlayerStats.MyInstance.Salud.Vidactual = 0;
        PlayerStats.MyInstance.Salud.VidaM = Mathf.Round((actualXp + ((actualXp / 1.2f) - (lastXp / 1.2f) + 10)));
        lastXp = actualXp;
        if (actualXp > 1000)
        {
            actualXp = actualXp / 1000;
        }
        actualXp = PlayerStats.MyInstance.Salud.VidaM;
        level++;
        Debug.Log("Set level at" + level);
        Debug.Log("This level has a max HP of: " + (PlayerStats.MyInstance.Salud.VidaM));
    }

    private void SetHpStartingPoint(int difficultyLevel)
    {
        if (difficultyLevel == 0)
        {
            return;
        }
        
        PlayerStats.MyInstance.Salud.Vidactual = 0;
        PlayerStats.MyInstance.Salud.VidaM = Mathf.Round((actualXp + ((actualXp / 1.2f) - (lastXp / 1.2f) + 10)));
        Debug.Log("Vida M: " + PlayerStats.MyInstance.Salud.VidaM);
        lastXp = actualXp;
        if (actualXp > 1000)
        {
            actualXp = actualXp / 1000;
        }
        actualXp = PlayerStats.MyInstance.Salud.VidaM;

        if (difficultyLevel > 1)
        {
            SetHpStartingPoint(difficultyLevel - 1);
        }
    }

    //set the logaritmic progression of the weapon damage that depends on the tower Health
    
    public void QuestProgression()
    {
        if (QuestController.MyQuestInstance.myQuestListDictionary == null)
            return;
            
        if (QuestController.MyQuestInstance.myQuestListDictionary.ContainsKey(0))
        {
            foreach (Quest quest in QuestController.MyQuestInstance.myQuestListDictionary[0])
            {
                if (quest.Active == true)
                {
                    quest.currentProgress += 1;
                    quest.Dirty = true;
                }
            }
        }
    }

    private void CameraProgression()
    {
        if (camera == null)
        {
            Debug.Log("camera is null");
        }
        else
        {
            camera.GetComponent<rotation_Camera>().maxLevel++;
            camera.GetComponent<rotation_Camera>().Up();
        }
    }

    private void NewFloor()
    {
        //NEW FLOOR IF NECESSARY
        if ((level) % buildingLevelMargin == 0)
        {
            //Debug.Log("Building a floor through build UPDATE");
            CreateFloor(level, new Vector3(0.09f, ((level - 1) * 3.4f) - 1.42f - 0.49f), new Vector3(0.09f, (level * 3.4f) - 1.42f - 0.49f, 0f), false, townNames[1], true, new Vector3(0.0f, 2.31f, 0.0f));
            //FeedbackController.MyFeedbackInstance.FeedbackNewFlatBuildable();
            //Debug.Log("Floor Position Setter");
        }
        else
        {
            //Debug.Log("Key:" + level);
            CreateFloor(level, new Vector3(0.09f, ((level - 1) * 3.4f) - 1.42f - 0.49f), new Vector3(0.09f, (level * 3.4f) - 1.42f - 0.49f, 0f), false, townNames[1], false, Vector3.zero);
            //Debug.Log("Floor Position Setter");
        }
    }

    private void NewBioma()
    {
        //zonesList.Add(new Zone(level, int.MaxValue, actualTower.GetComponent<TowerHolder>().thisUnit.position, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue), zonesList.Count, 0, true));
        //zonesList[zonesList.Count - 1].StartBioma(true);
       
        ////fl
        //if (floorsList.Count > 1 && (level) % (((floorsList.Count - 1) * buildingLevelMargin) + biomaMargin) == 0)
        //{
        //    zonesList[zonesList.Count - 1].FinalLevel = level;
        //    zonesList[zonesList.Count - 1].FinishingPoint = actualTower.GetComponent<TowerHolder>().thisUnit.position;
        //    zonesList[zonesList.Count - 1].EndBioma(false);

        //    if (QuestController.MyQuestInstance.myQuestListDictionary.ContainsKey(2))
        //    {
        //        foreach (Quest quest in QuestController.MyQuestInstance.myQuestListDictionary[2])
        //        {
        //            if (quest.Active == true)
        //            {
        //                quest.currentProgress += 1;
        //                quest.Dirty = true;
        //            }
        //        }
        //    }
        //}
    }

    public void EqualLife(float maxLife)
    {
        PlayerStats.MyInstance.Salud.Vidactual = maxLife;
        TowerCheck();
    }
    public void SumLife(float life)
    {
        PlayerStats.MyInstance.Salud.Vidactual += life;

        TowerCheck();
        //check if its > than max life and level up if true
        if (PlayerStats.MyInstance.Salud.Vidactual >= PlayerStats.MyInstance.Salud.VidaM)
        {
            Debug.Log("Leveling Up FROM" + (level));
            Debug.Log("This level has a max HP of: " + (PlayerStats.MyInstance.Salud.VidaM));
            float overlap = PlayerStats.MyInstance.Salud.Vidactual - PlayerStats.MyInstance.Salud.VidaM;
            LevelUp();

            if (progress == Progress.WORKING)
                SumLife(overlap);
        }
    }

    public void TowerCheck()
    {
        //in this function we create the floor al lvl 1 in the first quarter, this is incorrect, fix it later on
        switch (progress)
        {
            case Progress.WORKING:

                lifeFraction = (PlayerStats.MyInstance.Salud.Vidactual / PlayerStats.MyInstance.Salud.VidaM);
                
                if (lifeFraction <= 0.25f && lifeFraction >= 0f && built1 == false)
                {
                    //Debug.Log("Building first quarter");
                    BuildFirstQuarter();
                }
                else if (lifeFraction > 0.25f && lifeFraction <= 0.50f && built2 == false)
                {
                    BuildSecondQuarter();
                }
                else if (lifeFraction > 0.50f && lifeFraction <= 0.75f && built3 == false)
                {
                    BuildThirdQuarter();
                }
                else if (lifeFraction > 0.75f && lifeFraction <= 1.0f && built4 == false)
                {
                    BuildFourthQuarter();
                }
                //if (lifeFraction >= 1.0f)
                //{
                //    GameObject towerUnit = Instantiate(normalPrefab, new Vector3(1.73f, ((level) * 3.4f) - 1.42f - 0.49f, 0), Quaternion.identity);
                //    towerUnit.GetComponent<TowerHolder>().SetVisualApparition(LevelTraveler.MyTravelInstance.Level.GetTier());
                //    towerUnit.gameObject.transform.SetParent(towerParent.transform);
                //    //Create Unit Script
                //    towerUnit.GetComponent<TowerHolder>().thisUnit = new Unit(new Vector3(0.09f, ((level) * 3.4f) - 1.42f - 0.49f, 0), /* FRAGMENT */ 1);

                //    //towerHolders.Add(towerUnit.GetComponent<TowerHolder>());
                //    //Add it to the List
                //    theTower.Add(towerUnit);
                //}
                break;
            case Progress.FINISHED:
                
                break;
            default:
                break;
        }
    }

    private void SafeBuilding()
    {
        foreach (Transform fragment in towerParent.transform)
        {
            if (fragment != towerParent.transform.GetChild(towerParent.transform.childCount - 1) && fragment.GetComponent<TowerHolder>().thisUnit.GetState() != 1)
            {
                fragment.parent = null;
                Destroy(fragment.gameObject);
            }

            //corner case
            if(theTower.Count == 0 && level == 2)
            {
                ReplaceTower(1);
            }
        }
    }

    public void CheckIfFloorAndChangeBuildingPoint()
    {
        //Debug.Log("Checking Floor");
        if (build == true)
        {
            Debug.Log("Build is true! Danger!");
            return;
        }
        int count = 0;

        foreach (Floors item in floorsList.Values)
        {
            if (item.edificable == false)
            {
                continue;
            }

            if(item.level == camera.GetComponent<rotation_Camera>().cameraLevel)
            {
                if(item.claimed == true)
                {
                    //Debug.Log("Buildable but claimed...");
                    //it gets full only with 1 island
                    UIController.MyUiInstance.HideBuildingUI();
                    actualBuildingPoint = item.floorLevel;
                    actualFloor = item;
                    return;
                }
                //Debug.Log("You can build here!");
                actualBuildingPoint = item.floorLevel;
                actualFloor = item;
                //Debug.Log("Actual Building Point: " + actualBuildingPoint);
                Debug.Log("Actual City: " + item.townName);
                count++;
                UIController.MyUiInstance.ShowBuildingUI();
            }
        }

        if(count == 0)
        {
            UIController.MyUiInstance.HideBuildingUI();
        }
    }

    public bool IsFloor()
    {
        foreach (Floors item in floorsList.Values)
        {
            if (item.edificable == false)
            {
                continue;
            }
            if (item.level == camera.GetComponent<rotation_Camera>().cameraLevel)
            {
                //Debug.Log("Is Floor");
                return true;
            }
        }
        return false;
    }

    public bool IsTown()
    {
        if (IsFloor() == true)
        {
            if (actualFloor.isTown == true)
            {
                Debug.Log("Is Town");
                return true;
            }
        }
        return false;
    }

    public bool IsFloor(int cameraLevel)
    {
        int count = 0;

        foreach (Floors item in floorsList.Values)
        {
            if (item.edificable == false)
            {
                continue;
            }
            if (item.level == cameraLevel)
            {
                count++;
                return true;
            }
        }

        if (count == 0)
        {
            return false;
        }

        return false;
    }

    private void BuildFirstQuarter()
    {
        if (theTower.Count > 0)
        {
            ReplaceTower(1);
            SafeBuilding();
        }

        if (level == 1)
        {

            firstStepBuilding = BuildTower(firstQuartPrefab, new Vector3(1.73f, (level * 3.4f) - 1.42f - 0.49f, 0));
            firstStepBuilding.GetComponent<TowerHolder>().thisUnit = new Unit(new Vector3(0.09f, (level * 3.4f) - 1.42f - 0.49f, 0), /* FRAGMENT */0);
            towerPositions.Add(firstStepBuilding.GetComponent<TowerHolder>().thisUnit.position);
        }
        else
        {
            firstStepBuilding = BuildTower(prefabsAir[0], new Vector3(1.73f, (level * 3.4f) - 1.42f - 0.49f, 0));
            firstStepBuilding.GetComponent<TowerHolder>().thisUnit = new Unit(new Vector3(0.09f, (level * 3.4f) - 1.42f - 0.49f, 0), /* FRAGMENT */0);
            towerPositions.Add(firstStepBuilding.GetComponent<TowerHolder>().thisUnit.position);
        }
        firstStepBuilding.gameObject.transform.SetParent(towerParent.transform);
        firstStepBuilding.gameObject.name += level.ToString();
        built1 = true;
        actualTower = firstStepBuilding;
        gc.GetComponent<StatController>().AddLevelGold(0);
        FeedbackController.MyFeedbackInstance.FeedbackHit();

        //Debug.Log("Actual Level : " + level);


        //Debug.Log("Actual Level : " + level);
        if (level == 1)
        {
            Debug.Log("Creating first floor on TOWER FIRST");
            CreateFloor(level, new Vector3(0.09f, 0, 0), new Vector3(0.09f, (level * 3.4f) - 1.42f - 0.49f), false, townNames[0], true, Vector3.zero);
            floorsList[level].UpdateFloorData(actualTower.GetComponent<TowerHolder>().thisUnit, 1);
            actualFloor = floorsList[level];
        }

        SafeBuilding();
    }

    private void BuildSecondQuarter()
    {
        
        if (theTower.Count > 0 && built1 == false)
        {
            ReplaceTower(2);
            Debug.Log("Entering Safe Mode From Replacement 2 Quarter");
            SafeBuilding();
        }

        if (firstStepBuilding != null)
        {
            Destroy(firstStepBuilding);
        }
        if (level == 1)
        {
            secondStepBuilding = BuildTower(secondQuartPrefab, new Vector3(1.73f, (level * 3.4f) - 1.42f - 0.49f, 0)); ;
            secondStepBuilding.GetComponent<TowerHolder>().thisUnit = new Unit(new Vector3(0.09f, (level * 3.4f) - 1.42f - 0.49f, 0), /* FRAGMENT */ 0);
        }
        else
        {
            secondStepBuilding = BuildTower(prefabsAir[1], new Vector3(1.73f, (level * 3.4f) - 1.42f - 0.49f, 0)); ;
            secondStepBuilding.GetComponent<TowerHolder>().thisUnit = new Unit(new Vector3(0.09f, (level * 3.4f) - 1.42f - 0.49f, 0), /* FRAGMENT */ 0);
        }
        secondStepBuilding.gameObject.transform.SetParent(towerParent.transform);
        secondStepBuilding.gameObject.name += level.ToString();
        built2 = true;
        actualTower = secondStepBuilding;
        gc.GetComponent<StatController>().AddLevelGold(0);
        FeedbackController.MyFeedbackInstance.FeedbackHit();

        floorsList[level].UpdateFloorData(actualTower.GetComponent<TowerHolder>().thisUnit, 2);

        SafeBuilding();
        
    }

    private void BuildThirdQuarter()
    {
        
        if (theTower.Count > 0 && built1 == false && built2 == false)
        {
            ReplaceTower(3);
            Debug.Log("Entering Safe Mode From Replacement 3 Quarter");
            SafeBuilding();
        }

        if (firstStepBuilding != null)
        {
            Destroy(firstStepBuilding);
        }
        if (secondStepBuilding != null)
        {
            Destroy(secondStepBuilding);
        }

        if (level == 1)
        {
            thirdStepBuilding = BuildTower(thirdQuartPrefab, new Vector3(1.73f, (level * 3.4f) - 1.42f - 0.49f, 0));
            thirdStepBuilding.GetComponent<TowerHolder>().thisUnit = new Unit(new Vector3(0.09f, (level * 3.4f) - 1.42f - 0.49f, 0), /* FRAGMENT */ 0);
        }
        else
        {
            thirdStepBuilding = BuildTower(prefabsAir[2], new Vector3(1.73f, (level * 3.4f) - 1.42f - 0.49f, 0));
            thirdStepBuilding.GetComponent<TowerHolder>().thisUnit = new Unit(new Vector3(0.09f, (level * 3.4f) - 1.42f - 0.49f, 0), /* FRAGMENT */ 0);
        }
        thirdStepBuilding.gameObject.transform.SetParent(towerParent.transform);
        thirdStepBuilding.gameObject.name += level.ToString();
        built3 = true;
        actualTower = thirdStepBuilding;
        gc.GetComponent<StatController>().AddLevelGold(0);
        FeedbackController.MyFeedbackInstance.FeedbackHit();
        floorsList[level].UpdateFloorData(actualTower.GetComponent<TowerHolder>().thisUnit, 3);
        Debug.Log("Entering Safe Mode From Third Quarter");
        SafeBuilding();
        
    }

    private void BuildFourthQuarter()
    {
        if (theTower.Count > 0 && built1 == false && built2 == false && built3 == false)
        {
            ReplaceTower(4);
            Debug.Log("Entering Safe Mode From Replacement 4 Quarter");
            SafeBuilding();
        }

        //Debug.Log("BUILDING FOURTH QUARTER");
        if (firstStepBuilding != null)
        {
            Destroy(firstStepBuilding);
        }
        if (secondStepBuilding != null)
        {
            Destroy(secondStepBuilding);
        }
        if (thirdStepBuilding != null)
        {
            Destroy(thirdStepBuilding);
        }

        if (level == 1)
        {
            fourthStepBuilding = BuildTower(fullPrefab, new Vector3(1.73f, (level * 3.4f) - 1.42f - 0.49f, 0));
            fourthStepBuilding.GetComponent<TowerHolder>().thisUnit = new Unit(new Vector3(0.09f, (level * 3.4f) - 1.42f - 0.49f), /* FRAGMENT */ 0);
        }
        else
        {
            fourthStepBuilding = BuildTower(prefabsAir[3], new Vector3(1.73f, (level * 3.4f) - 1.42f - 0.49f, 0));
            fourthStepBuilding.GetComponent<TowerHolder>().thisUnit = new Unit(new Vector3(0.09f, (level * 3.4f) - 1.42f - 0.49f, 0), /* FRAGMENT */ 0);
        }
        fourthStepBuilding.gameObject.transform.SetParent(towerParent.transform);
        fourthStepBuilding.gameObject.name += level.ToString();
        //Debug.Log("Level added to the list, number:" + theTower.Count);
        //theTower.Add(fourthStepBuilding);
        built4 = true;
        actualTower = fourthStepBuilding;
        gc.GetComponent<StatController>().AddLevelGold(0);
        FeedbackController.MyFeedbackInstance.FeedbackHit();

        floorsList[level].UpdateFloorData(actualTower.GetComponent<TowerHolder>().thisUnit, 4);

        //Debug.Log("Entering Safe Mode From Fourth Quarter");
        SafeBuilding();
    }

    public void ReplaceTower(int localLevel)
    {
        //CHANGE LATER
        switch (localLevel)
        {
            case 1:
                builtR = true;
                break;
            case 2:
                built1 = true;
                break;
            case 3:
                built1 = true;
                built2 = true;
                break;
            case 4:
                built1 = true;
                built2 = true;
                built3 = true;
                break;
            default:
                break;
        }

        //TOTALLY DESTROYING THE OBJECT
        //theTower[theTower.Count - 1].transform.parent = null;
        //Destroy(theTower[theTower.Count - 1]);
        //theTower.RemoveAt(theTower.Count - 1);
        //Replace it with Unit
        //Debug.Log("Replacing in level" + (level - 1));
        GameObject towerUnit = Instantiate(normalPrefab, new Vector3(1.73f, ((level - 1) * 3.4f) - 1.42f - 0.49f, 0), Quaternion.identity);
        towerUnit.GetComponent<TowerHolder>().SetVisualApparition(LevelTraveler.MyTravelInstance.Level.GetTier());
        towerUnit.gameObject.transform.SetParent(towerParent.transform);
        //Create Unit Script
        towerUnit.GetComponent<TowerHolder>().thisUnit = new Unit(new Vector3(0.09f, ((level - 1) * 3.4f) - 1.42f - 0.49f, 0), /* FRAGMENT */ 1);
        floorsList[level - 1].UpdateFloorData(towerUnit.GetComponent<TowerHolder>().thisUnit, -1);

        //towerHolders.Add(towerUnit.GetComponent<TowerHolder>());
        //Add it to the List
        theTower.Add(towerUnit);
    }

    private GameObject BuildTower(GameObject towerBuffer, Vector3 position)
    {
        GameObject towerUnit = Instantiate(towerBuffer, position, Quaternion.identity);
        towerUnit.GetComponent<TowerHolder>().SetVisualApparition(LevelTraveler.MyTravelInstance.Level.GetTier());
        return towerUnit;
    }

    public void setText(string title, Vector3 position, Color color)
    {
        Debug.Log(position);
        megaIslandText.SetActive(true);
        megaIslandText.GetComponent<TextMeshPro>().text = title;
        megaIslandText.transform.localPosition = new Vector3(position.x - 0.0145f, (position.y / 50.0f) + 0.03f, position.z + 0.025f);
        megaIslandText.GetComponent<TextMeshPro>().color = color;

        StartCoroutine(Show3DText(textTransform, megaIslandText));
    }

    public void DamageFeedback(int typeDamage, float damage)
    {
        Mathf.Round(damage);

        actualTower.GetComponent<TowerHolder>().TakeDamage(typeDamage, damage);
        //StartCoroutine(ShowFeedbackCoroutine(typeDamage, damage));
    }
    //IEnumerator ShowFeedbackCoroutine(int typeDamage, float damage)
    //{
        //returning 0 will make it wait 1 frame because is the time the tower needs to initialize 
        //yield return 0;
        //Mathf.Round(damage);

        //actualTower.GetComponent<TowerHolder>().TakeDamage(typeDamage, damage);

        //yield return null;
        //code goes here
    //}

    public void SwitchFloatTextDir(float xDir = 0f, float yDir = 0f)
    {
        //textXdir is switching each time the function is called but the argument is static so we gotta use that ping pong
        textXDir *= -1;
        xDir *= textXDir;

        //Debug.Log("TextXDir is:" + textXDir);
        //Debug.Log("Feedback 8 changing" + actualTower.GetComponent<TowerHolder>().DamageFeedback.Feedbacks[8].GetComponent<MMFeedbackFloatingText>().Label);
        actualTower.GetComponent<TowerHolder>().DamageFeedback.Feedbacks[8].GetComponent<MMFeedbackFloatingText>().Direction = new Vector3(xDir, yDir, 0);
    }
    public void SetFeedbackIntensity(float minDamage, float criticalDamage)
    {
        //do it for both the feedbacks of the contract and the weapon feedback because they are different
        //Debug.Log("Normal Damage: " + minDamage);
        //Debug.Log("Critical Damage: " + criticalDamage);

        float maxNormalValue = 0;
        maxNormalValue = actualTower.GetComponent<TowerHolder>().DamageFeedback.Feedbacks[8].Timing.IntensityIntervalMax;
        maxNormalValue = minDamage;
        actualTower.GetComponent<TowerHolder>().DamageFeedback.Feedbacks[8].Timing.IntensityIntervalMax = maxNormalValue + 0.1f;
        
        
        //HIGH INTENSITY
        float minHighValue = 0;
        minHighValue = actualTower.GetComponent<TowerHolder>().DamageFeedback.Feedbacks[9].Timing.IntensityIntervalMin;
        minHighValue = minDamage;
        actualTower.GetComponent<TowerHolder>().DamageFeedback.Feedbacks[9].Timing.IntensityIntervalMin = minHighValue + 0.1f;

        float maxHighValue = 0;
        maxHighValue = actualTower.GetComponent<TowerHolder>().DamageFeedback.Feedbacks[9].Timing.IntensityIntervalMax;
        maxHighValue = criticalDamage;
        actualTower.GetComponent<TowerHolder>().DamageFeedback.Feedbacks[9].Timing.IntensityIntervalMax = maxHighValue + 0.1f;
        

        //MAX INTENSITY
        float minMaxValue = 0;
        minMaxValue = actualTower.GetComponent<TowerHolder>().DamageFeedback.Feedbacks[10].Timing.IntensityIntervalMin;
        minMaxValue = criticalDamage;
        actualTower.GetComponent<TowerHolder>().DamageFeedback.Feedbacks[10].Timing.IntensityIntervalMin = minMaxValue + 0.1f;
        

        //Debug.Log("Maximum Small Intensity:" + actualTower.GetComponent<TowerHolder>().DamageFeedback.Feedbacks[8].Timing.IntensityIntervalMax);
        //Debug.Log("Minimum High Intensity:" + actualTower.GetComponent<TowerHolder>().DamageFeedback.Feedbacks[9].Timing.IntensityIntervalMin);
        //Debug.Log("Maximum High Intensity:" + actualTower.GetComponent<TowerHolder>().DamageFeedback.Feedbacks[9].Timing.IntensityIntervalMax);
        //Debug.Log("Minimum High Intensity:" + actualTower.GetComponent<TowerHolder>().DamageFeedback.Feedbacks[10].Timing.IntensityIntervalMin);
    }

    IEnumerator Show3DText(float y, GameObject text)
    {
        float maxSize = 10.68f;
        float minSize = 0.01f;
        text.GetComponent<TextMeshPro>().fontSize = 0.01f;
        for (int i = 0; i < maxSize; i++)
        {
            yield return new WaitForSeconds(0.025f);
            text.GetComponent<TextMeshPro>().fontSize += 0.5f;
        }
        yield return new WaitForSeconds(1.0f);
        for (int z = (int)maxSize; z > minSize; z--)
        {
            yield return new WaitForSeconds(0.025f);
            text.GetComponent<TextMeshPro>().fontSize -= 0.5f;
        }
        text.SetActive(false);
        yield break;
    }

    public void CreateTile(int index, GameObject tileToRemove, Vector3 position, GameObject tile, int floorLevel, float futurePrice)
    {
        GameObject newTile = Instantiate(tile, position, quat);

        
        if (tile.name == "forestTile")
        {
            newTile.gameObject.GetComponent<islandHolder>().islandClass = new Forest("forest", position);
            newTile.gameObject.GetComponent<islandHolder>().islandClass.id = index;
            actualFloor.isTown = true;
        }
        if (tile.name == "riverTile")
        {
            Debug.Log("Creating sea");
            newTile.gameObject.GetComponent<islandHolder>().islandClass = new Sea("sea", position);
            newTile.gameObject.GetComponent<islandHolder>().islandClass.id = index;
            actualFloor.isTown = true;

        }
        if (tile.name == "lavaTile")
        {
            newTile.gameObject.GetComponent<islandHolder>().islandClass = new Volcano("volcano", position);
            newTile.gameObject.GetComponent<islandHolder>().islandClass.id = index;
            actualFloor.isTown = true;

        }
        if (tile.name == "dirtyBasicTile")
        {
            Debug.Log("Creating DirtyBasicTile");
            newTile.gameObject.GetComponent<islandHolder>().islandClass = new buildableTile("buildable", futurePrice, position);
            newTile.gameObject.GetComponent<islandHolder>().islandClass.id = index;

        }
        Debug.Log("Removing at:" + index);
        actualFloor.islands.RemoveAt(index);
        actualFloor.islands.Insert(index, newTile.gameObject.GetComponent<islandHolder>().islandClass);

        Destroy(tileToRemove);

        CheckMegaIsland(gc.GetComponent<buildController>().GetActualFloor());
    }

    public void CreateFloor(int level, Vector3 buildingPoint, Vector3 checkPoint, bool full, string name, bool edificable, Vector3 islandOffset)
    {
        if (edificable == true)
        {
            //MINIMAP INSTANTIATION
            GameObject newMapCheckPoint;
            newMapCheckPoint = Instantiate(miniMapPrefab, Vector3.zero, Quaternion.identity, parentMiniMap.transform);
            newMapCheckPoint.GetComponent<MiniMapHolder>().Level = level;
            //important to know that this code is executed BEFORE actualTower is re-setted so the building point is from the last tower, that is perfect for instantiating the islands visually
            //BUT IT'S ONLY VISUAL
            //floorsList.Add(level, new Floors(level, createSelectionTiles(buildingPoint + islandOffset), buildingPoint, checkPoint, full, name, edificable));
            floorsList.Add(level, new Floors(level, buildingPoint, checkPoint, name, edificable));
        }
        else
        {
            //important to know that this code is executed BEFORE actualTower is re-setted so the building point is from the last tower, that is perfect for instantiating the islands visually
            //BUT IT'S ONLY VISUAL
            Debug.Log("Adding non edificable floor");
            floorsList.Add(level, new Floors(level, buildingPoint, checkPoint, name, edificable));
        }
    }
    public void RemoveFromFloor(GameObject tileToRemove, Vector3 position, int floorLevel)
    {
        floorsList[actualBuildingPoint].islands.Remove(tileToRemove.gameObject.GetComponent<islandHolder>().islandClass);
    }

    public void CheckMegaIsland(Floors floor)
    {
        int forestCount = 0;
        int seaCount = 0;
        int volcanoCount = 0;
        int generalCount = 0;
            foreach (var island in floor.islands)
            {
                if (island.type == Island.Type.FOREST)
                {
                    Debug.Log("forest Count" + forestCount);
                    forestCount++;
                    if (forestCount == 6)
                    {
                        
                        return;
                    }
                }
                if (island.type == Island.Type.SEA)
                {
                    Debug.Log("sea Count" + seaCount);
                    seaCount++;
                    if (seaCount == 6)
                    {
                        
                        return;
                    }
                }
                if (island.type == Island.Type.VOLCANO)
                {
                    Debug.Log("volcano Count" + volcanoCount);
                    volcanoCount++;
                    if (volcanoCount == 6)
                    {
                        
                        return;
                    }
                }
                if (island.type != Island.Type.BUILDABLE)
                {
                    generalCount++;
                }
                
                if(generalCount == 6)
                {
                    Debug.Log("Town is full!");
                    actualFloor.full = true;
                }
            }
            forestCount = 0;
            seaCount = 0;
            volcanoCount = 0;
    }

    public void LevelUpDebug()
    {
        DecreaseFeedback.StopFeedbacks();
    }

    //save when leaving
    public FloorsList Save(string path)
    {

        FloorsList floorSerializable = new FloorsList();
        floorSerializable.floor = new Floors[floorsList.Values.Count];
        //foreach (var item in floorsList)
        //{
        //    Debug.Log("Element with key" + item.Key);
        //}

        for (int i = 0; i < floorsList.Values.Count; i++)
        {
            if (floorsList.ContainsKey(i + 1) == true)
            {
                floorSerializable.floor[i] = floorsList[i + 1];
            }
            else
            {
                Debug.Log("Error at Saving, floor at level" + (i + 1) + "doesn't exist");
            }
        }

        return floorSerializable;
    }

    //Only load when coming from out or menu
    public void Load(FloorsList floorSerializable)
    {
        floorsList.Clear();
        int key = 1;
        foreach (Floors item in floorSerializable.floor)
        {
            floorsList.Add(key, item);

            if (key == floorSerializable.floor.Length)
            {
                level = key;
                actualFloor = item;

                rotation_Camera.MyCameraInstance.maxLevel = key;
                rotation_Camera.MyCameraInstance.cameraLevel = 1;

                Debug.Log("Loading CameraInformation" + rotation_Camera.MyCameraInstance.maxLevel);

                rotation_Camera.MyCameraInstance.Maximum();
            }

            Debug.Log("New Element on the Dictionary!" + "Element" + item.level + "Fragment:" + item.towerUnitData.GetState());
            ReplicateWorld(item);

            key++;
        }

        //set the dictionary

        //Debug.Log("Unit Level Loaded:" + newFloor.level);
    }

    public void ReplicateWorld(Floors floor)
    {
        //Tower
        switch (floor.towerUnitData.GetState())
        {
            case 1:
                GameObject fullUnit = Instantiate(normalPrefab, new Vector3(1.73f, ((floor.level) * 3.4f) - 1.42f - 0.49f, 0), Quaternion.identity);
                fullUnit.gameObject.transform.SetParent(towerParent.transform);
                fullUnit.GetComponent<TowerHolder>().thisUnit = new Unit(new Vector3(0.09f, ((floor.level) * 3.4f) - 1.42f - 0.49f, 0), /* FRAGMENT */ floor.towerUnitData.GetState());

                theTower.Add(fullUnit);
                break;
            case 0:
                //switch (floor.buildData.buildStage)
                //{
                //    case 1:
                //        built1 = true;
                //        break;
                //    case 2:
                //        built1 = true;
                //        built2 = true;
                //        break;

                //    case 3:
                //        built1 = true;
                //        built2 = true;
                //        built3 = true;
                //        break;

                //    case 4:
                //        built1 = true;
                //        built2 = true;
                //        built3 = true;
                //        built4 = true;
                //        break;
                //    default:
                //        break;
                //}
                TowerCheck();
                break;
            default:
                break;
        }

        if (floor.edificable == true)
        {
            actualBuildingPoint = floor.floorLevel;

            GameObject newMapCheckPoint;
            newMapCheckPoint = Instantiate(miniMapPrefab, Vector3.zero, Quaternion.identity, parentMiniMap.transform);
            newMapCheckPoint.GetComponent<MiniMapHolder>().Level = floor.level;

            foreach (Island item in floor.islands)
            {
                switch (item.type)
                {
                    case Island.Type.FOREST:
                        GameObject forestGm = Instantiate(forestTile, item.buildPosition, quat, islandsParent.transform);
                        forestGm.gameObject.GetComponent<islandHolder>().islandClass = item;
                        break;
                    case Island.Type.SEA:
                        GameObject seaGm = Instantiate(riverTile, item.buildPosition, quat, islandsParent.transform);
                        seaGm.gameObject.GetComponent<islandHolder>().islandClass = item;
                        break;
                    case Island.Type.VOLCANO:
                        GameObject volcanoGm = Instantiate(volcanoTile, item.buildPosition, quat, islandsParent.transform);
                        volcanoGm.gameObject.GetComponent<islandHolder>().islandClass = item;
                        break;
                    case Island.Type.BUILDABLE:
                        GameObject buildGm = Instantiate(cleanTile, item.buildPosition, quat, islandsParent.transform);
                        buildGm.gameObject.GetComponent<islandHolder>().islandClass = item;
                        break;
                    case Island.Type.EMPTY:
                        GameObject emptyGm = Instantiate(selectionTile, item.buildPosition, quat, islandsParent.transform);
                        emptyGm.gameObject.GetComponent<islandHolder>().islandClass = item;
                        break;
                    default:
                        break;
                }
            }
        }
        
        
        //Create Unit Script

        //towerHolders.Add(towerUnit.GetComponent<TowerHolder>());
        //Add it to the List
    }
}
