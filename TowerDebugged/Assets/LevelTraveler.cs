using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;


public class LevelTraveler : MonoBehaviour
{
    private static LevelTraveler travelInstance;

    public static LevelTraveler MyTravelInstance
    {
        get
        {
            if (travelInstance == null)
            {
                travelInstance = FindObjectOfType<LevelTraveler>();
            }
            return travelInstance;
        }


    }

    public enum GameState
    {
        MENU,
        LEVEL
    }

    [SerializeField]
    private GameState gameState;

    public GameState GetGameState()
    {
        return gameState;
    }

    public SavedData gameData = null;

    //reference the level we are playing
    [SerializeField]
    private Level level;

    //setter for level
    public Level Level
    {
        get { return level; }
        set
        {
            level = value;
        }
    }

    [SerializeField]
    private GearObject gear;

    public GearObject Gear
    {
        get { return gear; }
        set
        {
            gear = value;
        }
    }
    // Start is called before the first frame update
    void OnLevelWasLoaded()
    {
        switch (gameState)
        {
            case GameState.MENU:
                //entering LEVEL from MENU
                if (Level != null && Level.CheckNulls() == false)
                {
                    Level.ResetLevel();
                    Level.ResetRewards();
                    Level.SetSign();
                }
                gameState = GameState.LEVEL;
                break;
            case GameState.LEVEL:
                //entering MENU from LEVEL
                Debug.Log("Loading from OnLevelLoaded");
                LoadData();
                switch (level.state)
                {
                    case Level.LevelState.Completed:
                        StatController.MyInstance.AddRewards(level.Rewards);
                        
                        if (MenuBrain.MyMenuBrain.levels[level.id + 1] != null)
                        {
                            MenuBrain.MyMenuBrain.levels[level.id + 1].Locked = false;
                        }

                        //KingController.MyKingInstance.
                        break;
                    case Level.LevelState.Incompleted:
                        Debug.Log("Did nothing on the rewards section");
                        break;
                    default:
                        break;
                }
                KingController.MyKingInstance.DTGtoCards(StatController.MyInstance.GetDTG(MenuBrain.MyMenuBrain.NextLevel(Level.id).Objectives.startingProgressPoint, MenuBrain.MyMenuBrain.NextLevel(Level.id).Objectives.numberOfLevels), 4);
                level.actuallyPlaying = false;
                UnlockController.MyInstance.CheckUnlocks(StatController.MyInstance.GetLevel(), level.id);
                gameState = GameState.MENU;
                break;
            default:
                break;
        }
    }
    
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        gameState = (GameState)SceneManager.GetActiveScene().buildIndex;

        if (travelInstance == null)
        {
            travelInstance = FindObjectOfType<LevelTraveler>();
        }
        else
        {
            Destroy(this.gameObject);
        }
        

        //Debug.Log("Loading from awake");

    }

    private void Start()
    {
        LoadData();
    }
    private void LoadData()
    {
        gameData = (SavedData)MMSaveLoadManager.Load(typeof(SavedData), "SavedGame" + ".brick");
        //Debug.Log("The value of game data is: " + gameData);
        //fill all the data of the game with the data loaded from gameData
        if (gameData != null)
        {
            //Debug.Log("Gold loaded  from save: " + gameData.Gold);
            StatController.MyInstance.gameGold = gameData.Gold;
            StatController.MyInstance.SetGoldText();
            //Debug.Log("Xp loaded from save: " + gameData.xp);
            StatController.MyInstance.LoadXp(gameData.xp, gameData.level);

            if (gameData.weaponUnlock != null)
            {
                UnlockController.MyInstance.LoadUnlocks(gameData.weaponUnlock);
            }
            else
            {
                //Debug.Log("No unlocks found in save");
            }
            
            if (gameData.levelUnlock != null)
            {
                MenuBrain.MyMenuBrain.LoadLevelUnlocks(gameData.levelUnlock);
            }
            else
            {
                //Debug.Log("No unlocks found in save");
            }
        }
        else
        {
            gear.contract.NewGame();
        }
        
    }

    public void SerializeAndSave()
    {
        gameData = new SavedData(StatController.MyInstance.gameGold, StatController.MyInstance.GetXp(), StatController.MyInstance.GetLevel(), new List<SerializableUnlock>(), new List<SerializableUnlock>());
        //foreach unlock in unlock controller push the id and the unlocked status into gameData
        foreach (Unlock unlock in UnlockController.MyInstance.unlocks)
        {
            gameData.weaponUnlock.Add(new SerializableUnlock(unlock.GetID(), unlock.GetUnlocked()));
        }
        foreach (Level level in MenuBrain.MyMenuBrain.levels)
        {
            gameData.levelUnlock.Add(new SerializableUnlock(level.id, level.Locked));
        }

        MMSaveLoadManager.Save(gameData, "SavedGame" + ".brick");
    }

    public void OnApplicationQuit()
    {
        //clear the reward of both rewards of the level variable

        if (gameState == GameState.MENU)
        {
            //Debug.Log("Saving! XP" + StatController.MyInstance.GetXp());
            SerializeAndSave();
        }
    }

    
}
