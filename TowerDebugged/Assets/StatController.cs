using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using TMPro;

public class StatController : MonoBehaviour
{

    private static StatController instance;

    public static StatController MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StatController>();
            }
            return instance;
        }


    }
    [SerializeField]
    private TextMeshProUGUI goldText;
    [SerializeField]
    private TextMeshProUGUI resourcesText;
    [SerializeField]
    private TextMeshProUGUI levelText;

    //internal gold is the gold we get on-level
    private float internalGold = 0;

    //internal xp is the xp we get on-level
    private float internalXp = 0;

    private int level = 1;

    private float actualXp = 0f;

    public float avgGoldHit = 0;
    public float hitBag = 0;

    //game gold is the gold we own out of the level
    public float gameGold = 0f;

    public float tickPercentage;

    public MMProgressBar xpBar;

    //create a dictionary that stores the priceToLevelUp of all the weapons taking the skill id as the key
    public Dictionary<int, float> priceToLevelUp = new Dictionary<int, float>();

    //with the list of craftHolders fill the priceToLevelUp dictionary with an initial value of zero and the ID of the skill as the key


    public float GetXp()
    {
        return actualXp;
    }

    public int GetLevel()
    {
        return level;
    }
    //create a setter for the xp
    public void LoadXp(float xp, int level)
    {
        //Debug.Log("Setting xp to" + xp);
        this.actualXp = xp;
        this.level = level;
        //check that actualXp is not greater than vidamax

        PlayerStats.MyInstance.XpBar.VidaM = (Mathf.Pow(level, 3) + 10);
        PlayerStats.MyInstance.XpBar.Vidactual = xp;
        levelText.text = level.ToString();
    }

    public float GetGold()
    {
        return gameGold;
    }

    public void SetGold(float amount)
    {
        gameGold = amount;
        SetGoldText();
    }

    public void AddGold(float amount)
    {
        
        gameGold += amount;
        Debug.Log("Adding Gold: " + amount + " For a total of:" + gameGold);
        SetGoldText();
    }

    public void SubstractGold(float amount)
    {
        gameGold -= amount;
        SetGoldText();
    }

    
    // Start is called before the first frame update
    void Awake()
    {
        if (levelText != null)
            levelText.text = level.ToString();
        
        internalGold = 0;
        hitBag = 0f;
    }

    //create a update method with two get key down inputs in the y and u keys that adds and substracts Xp
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (LevelTraveler.MyTravelInstance.Level != null)
                GetDTG(LevelTraveler.MyTravelInstance.Level.Objectives.startingProgressPoint, LevelTraveler.MyTravelInstance.Level.Objectives.numberOfLevels);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            SubstractXp(100);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Unlocking...");
            UnlockController.MyInstance.CheckUnlocks(28);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SubstractGold(100);
        }
    }

    public void SetGoldText()
    {
        goldText.text = Aproximation(gameGold);
    }

    #region XP
    public void SubstractXp(int value)
    {
        actualXp -= value;
        PlayerStats.MyInstance.XpBar.Vidactual = actualXp;
        //resourcesText.text = actualResources.ToString();
    }
    
    public void AddXp(float amount)
    {
        actualXp += amount;
        
        //check that actualXp is not greater than XpBar max xp
        if (actualXp >= PlayerStats.MyInstance.XpBar.VidaM)
        {
            LevelUp(actualXp);
            return;
        }

        PlayerStats.MyInstance.XpBar.Vidactual = actualXp;
        Debug.Log("Adding XP: " + amount + " For a total of:" + actualXp);
    }

    public void LevelUp(float xp)
    {
        //create a function that gets the rest of the xp and adds it to the next level
        float restXp = xp - PlayerStats.MyInstance.XpBar.VidaM;
       
        level++;
        Debug.Log("Vida max up to: " + (Mathf.Pow(level, 3) + 10));
        PlayerStats.MyInstance.XpBar.VidaM = (Mathf.Pow(level, 3)+10);
        levelText.text = level.ToString();

        //recursive call
        if (restXp >= PlayerStats.MyInstance.XpBar.VidaM)
        {
            LevelUp(restXp);
        }
        else
        {
            PlayerStats.MyInstance.XpBar.Vidactual = restXp;
            actualXp = restXp;
        }
    }

    #endregion

    #region Gold IN-LEVEL

        public float GetLevelGold()
        {
            return internalGold;
        }
        public void HitBag(float amount)
        {
            hitBag += amount;
        }

        public void AddGoldFromHitBag()
        {
            AddLevelGold(hitBag);
            buildController.MyBuildInstance.RefreshBuyableIslands();
            //Debug.Log("Accumulated Gold Hit: " + hitBag + "G");
            buildController.MyBuildInstance.actualTower.GetComponent<TowerHolder>().GoldFeedback.GoldFeedbacks((int)hitBag);
            Debug.Log("Gold gained from hit : " + hitBag + "G");
            
            hitBag = 0f;
        }

        public void AddLevelGold(float amount)
        {
            //i add the amount to the data
            internalGold += amount;
            Debugger.MyTowerInstance.totalGoldDebug += amount;
            Debug.Log("Adding amount of gold: " + amount);

            if (LevelTraveler.MyTravelInstance != null)
                LevelTraveler.MyTravelInstance.Level.GetGoldReward().GetAmount += amount;
        }

        public void AddLevelXP(float amount)
        {
            internalXp += amount;
        
            LevelTraveler.MyTravelInstance.Level.GetXpReward().GetAmount += amount;
        }   

        public void SubstractLevelGold(float amount)
        {
            internalGold -= amount;
        }
        #endregion

    #region Gold OUT-LEVEL and XP OUT-LEVEL
    public void AddRewards(Rewards[] reward)
    {
        //load the scene with the data stored
        
        foreach (Rewards item in reward)
        {
            item.ApplyReward();
        }
    }
    #endregion

    
    public void RecolectCity()
    {
        int newResources = 0;
        foreach (Island item in buildController.MyBuildInstance.GetActualFloor().islands)
        {
            if (item == null)
                return;

            if (item.type == Island.Type.BUILDABLE || item.type == Island.Type.EMPTY)
            {
                continue;
            }

            Debug.Log("Id of the island: " + item.id);

            //carefull with that, in balancing it can create some error.
            newResources += (int)item.Recolect();
            item.Storage = 0f;
        }
        AddXp(newResources);
    }

    public void PrintRecolectCity()
    {
        foreach (Island item in buildController.MyBuildInstance.GetActualFloor().islands)
        {
            Debug.Log("Name: " + buildController.MyBuildInstance.GetActualFloor().townName);
            Debug.Log("Name of the island: " + item.name);
            if (item == null)
            {
                Debug.Log("That island doesn't exist!");
            }
        }
    }



    //i want a static function that given a int or a float returns a string with the aproximation of the number
    public static string Aproximation(float number)
    {
        if (number == 0)
        {
            return 0.ToString();
        }
        string sufix = "";
        string aproximation = "";
        if (number >= 1000)
        {
            number = number / 1000;
            sufix = "k";
            aproximation = "F1";


            if (number >= 1000)
            {
                number = number / 1000;
                sufix = "M";
                aproximation = "F1";

            }
            //round the number to only two decimals and avoid putting two zeros at the end
            //aproximation = number.ToString("F2");
            //if (aproximation[aproximation.Length - 1] == '0' && aproximation[aproximation.Length - 2] == '0')
            //{
            //    aproximation = number.ToString("F0");
            //}
        }
        else
        {
            aproximation = "F0";
        }
        return number.ToString(aproximation) + sufix;
    }

    public static float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        //vida escalable con vida actual
    }

    
    public float GetDTG(int startingLevel, int finishLevel)
    {
        float DTG = 0;
        //in order to get the dgt we must use a float named contract %, this'll be the % of health it applies every tick
        //ex - if we have a 0.01% that means each tick advances a 0.01 of the health bar, so if we have 1000 ticks we'll have a 10% of the health bar
        //the contract damage is calculated getting the health of the starting point. After that taking the health of each level to the end and add it into a SUM
        //this sum divided by the number of levels and multiplied by the contract % will give us the contract damage we must add to our contract. 
        float debugValue = 0f;

        //so here we'll get the sum
        float hpSum = 0f;
        for (int i = startingLevel; i < startingLevel + finishLevel; i++)
        {
            Debug.Log("Sending the level: " + i);
            hpSum += GetRelativeHp(i, false);
            debugValue += GetRelativeHp(i, true);
            Debug.Log("Adding value to sum" + debugValue);
            Debug.Log("For a total value of: " + hpSum);
            debugValue = 0f;
        }

        DTG = hpSum / finishLevel;
        DTG *= tickPercentage;

        Debug.Log("Sending a Damage to Get of " + DTG);
        return DTG;
    }

    public float GetRelativeHp(int level, bool debug)
    {
        float startingPoint = 50f;
        float lastXp = 0f;
        float bufferMaxHp = 0f;

        if (level == 1)
        {
            bufferMaxHp = startingPoint;
            return bufferMaxHp;
        }

        while (level > 1)
        {
            bufferMaxHp = Mathf.Round((startingPoint + ((startingPoint / 1.2f) - (lastXp / 1.2f) + 10)));
            lastXp = startingPoint;
            startingPoint = bufferMaxHp;

            level--;
        }
        
        return bufferMaxHp;
    }
}
