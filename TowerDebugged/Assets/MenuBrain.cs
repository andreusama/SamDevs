using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBrain : MonoBehaviour
{
    public GameObject generalScroll;

    private static MenuBrain menuBrain;

    //make a list of LevelHolders
    public LevelHolder actualLevel;
    public SliderClamp levelSlider;

    public List<Level> levels;

    [SerializeField]
    private GameObject levelPrefab;
    [SerializeField]
    private Transform levelParent;


    public static MenuBrain MyMenuBrain
    {
        get
        {
            if (menuBrain == null)
            {
                menuBrain = FindObjectOfType<MenuBrain>();
            }
            return menuBrain;
        }


    }

    public void Awake()
    {
        if (MMSaveLoadManager.Load(typeof(SavedData), "SavedGame" + ".brick") == null)
            LoadLevelUnlocks(null);
        
        CreateLevels();
        ProgressionController.MyProgressionController.InitBalancedLevels(levels);

        InstantiateBalancedLevel(StatController.MyInstance.GetLevel());

        //Debug.Log("Done loading levels");
    }

    public void Start()
    {

    }
    
    public void GetLevelByIndex(int index)
    {
        LevelTraveler.MyTravelInstance.Level = levels[index];
    }
    public void SelectLevel()
    {
        //get the nearest billboard
        RectTransform nearestBillboard = levelSlider.nearestGetter;
        //get the levelholder from the nearest billboard
        actualLevel = nearestBillboard.transform.GetComponent<LevelHolder>();
        actualLevel.level.actuallyPlaying = true;
        LevelTraveler.MyTravelInstance.Level = actualLevel.level;

        
        //here we set up all the expected gold and xp to cap the necessities of the player (level ups, unlocks, progress...)
    }

    public Level GetLevelBySlider()
    {
        RectTransform nearestBillboard = levelSlider.nearestGetter;
        //get the levelholder from the nearest billboard
        actualLevel = nearestBillboard.transform.GetComponent<LevelHolder>();

        return actualLevel.level;
    }
    private void CreateLevels()
    {
        foreach (Level item in levels)
        {
            GameObject newLevel = Instantiate(levelPrefab, levelParent);
            newLevel.GetComponent<LevelHolder>().SetLevel(item);
            //check if the level is unlocked and if it is snap to it
            //set the nearest billboard of the slider to the last level unlocked of the list
        }
    }
    


    //depending on the players level we want to instantiate levels according to his progression curve
    //level 2, 3 and 4 are the first levels to be played and they get the player the WOOD HAMMER - level 27
    //level 5, 6 and 7 are the second levels to be played and they get the player the STONE HAMMER - level 54
    //level 8, 9 and 10 are the third levels to be played and they get the player the IRON HAMMER - level 81
    //we gotta create a function that depending on the players level a certain level will be instantiated
    private void InstantiateBalancedLevel(int actualLevel)
    {
        if (actualLevel < 26)
        {
            //Debug.Log("The current value is: " + StatController.Map(actualLevel, 1, 26, 0, 100));
        }
        
        //this functions instantiates a level depending on the players level - this avoids frustration if the player is not enough powerfull

    }

    public void Update()
    {
        if (levelSlider.actualState == SliderClamp.States.ONSNAP)
        {
            MenuUI.MyMenuUiInstance.PlayAvailability(GetLevelBySlider().Locked);
        }

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    levelSlider.SnapTo(levelSlider.GetBillboardRects[0]);
        //    //Debug.Log("Snapping to level billboard named: " + levelSlider.GetBillboardRects[0].name);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    levelSlider.SnapTo(levelSlider.GetBillboardRects[1]);
        //    //Debug.Log("Snapping to level billboard named: " + levelSlider.GetBillboardRects[1].name);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    levelSlider.SnapTo(levelSlider.GetBillboardRects[2]);
        //    //Debug.Log("Snapping to level billboard named: " + levelSlider.GetBillboardRects[2].name);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    levelSlider.SnapTo(levelSlider.GetBillboardRects[3]);
        //    //Debug.Log("Snapping to level billboard named: " + levelSlider.GetBillboardRects[3].name);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    levelSlider.SnapTo(levelSlider.GetBillboardRects[4]);
        //    //Debug.Log("Snapping to level billboard named: " + levelSlider.GetBillboardRects[4].name);
        //}
    }

    public void LoadLevelUnlocks(List<SerializableUnlock> _loadingUnlocks)
    {
        if (_loadingUnlocks == null)
        {
            //Debug.Log("No unlocks to load");
            foreach (Level lvl in levels)
            {
                if (lvl.id != 0)
                {
                    lvl.Locked = true;
                }
            }
            return;
        }
        foreach (Level lvl in levels)
        {
            //Debug.Log("Number of levels: " + levels.Count);
            foreach (SerializableUnlock toLoad in _loadingUnlocks)
            {
                //Debug.Log("Number of unlocks to load: " + _loadingUnlocks.Count);
                if (lvl.id == toLoad.id)
                {
                    //Debug.Log("Loading unlock of ID: + " + toLoad.id + "with value: " + toLoad.unlocked.ToString().ToUpper());
                    lvl.Locked = toLoad.unlocked;
                    //Debug.Log("Level locked variable setted to" + toLoad.unlocked.ToString().ToUpper());
                }
            }
        }
    }

    public Level NextLevel(int levelId)
    {
        if (levels[levelId + 1] == null)
        {
            return null;
        }

        Debug.Log("Returning " + levels[levelId + 1].name);
        return levels[levelId + 1];
    }
}
