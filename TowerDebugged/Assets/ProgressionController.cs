using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionController : MonoBehaviour
{
    private static ProgressionController progressionController;

    public static ProgressionController MyProgressionController
    {
        get
        {
            if (progressionController == null)
            {
                progressionController = FindObjectOfType<ProgressionController>();
            }
            return progressionController;
        }


    }

    
    public int numberOfQuestHotPoint = 3;
    
    private float hpQuestPrice = 250f;
    public float HpQuestPrice { get => hpQuestPrice; set => hpQuestPrice = value; }

    private float hpQuestXp = 250f;

    public float HpQuestXp { get => hpQuestXp; set => hpQuestXp = value; }

    //% of gold from quests and hit
    public float questP = 0.7f;
    public float hitP = 0.3f;
    
    void Awake()
    {
        //here we are going to set how many gold we're gonna win from this level
        //this is made basically by knowing how many level ups we want to player to purchase after the level
        //the first step is 25 so in the progression we're going to do 25 steps
    }
    
    public void SetQuestGold(Quest quest, float questPool)
    {
        //HpQuestPrice = 66625;
        //Gold we must win this level
        if (LevelTraveler.MyTravelInstance != null)
            HpQuestPrice = LevelTraveler.MyTravelInstance.Level.ProjectedGold;
        
        //average gold per quests
        float avgGold = hpQuestPrice / questPool;
        Debug.Log("Average Gold : " + avgGold);
        //reward of this quest
        quest.goldReward = avgGold;
    }

    public void SetQuestXp(Quest quest, float questPool)
    {
        //HpQuestXp = 105875;
        if (LevelTraveler.MyTravelInstance != null)
            HpQuestXp = LevelTraveler.MyTravelInstance.Level.ProjectedXp;
        float avgXp = hpQuestXp / questPool;
        Debug.Log("Average XP : " + avgXp);
        quest.xpReward = avgXp;
    }

    public void InitBalancedLevels(List<Level> _levels)
    {
        foreach (Level lvlObject in _levels)
        {
            lvlObject.ProjectedGold = QuestProgressionGold(lvlObject);
            lvlObject.ProjectedXp = QuestProgressionXp(lvlObject);
        }
    }
    
    //in this function we set the gold needed to cover the levels that we have to level up the weapon after the level
    public float QuestProgressionGold(Level lvlObject)
    {
        //set it to the initial values of the progression
        float virtualActualCost = 5;
        float virtualLastCost = 0;
        
        float lastCostBuffer = 0;

        float totalSum = 0f;


        int firstLevel = 0;

        //progression part that has to be avoided
        for (int i = 0; i < lvlObject.Objectives.startingProgressPoint - 1; i++)
        {
            lastCostBuffer = virtualActualCost;
            virtualActualCost = virtualActualCost + (virtualActualCost - virtualLastCost + 25);
            virtualLastCost = lastCostBuffer;
            //Debug.Log("Leaving the simulation part NO INCLUDED in the" + lvlObject.name.ToUpper() + "in iteration " + i + " ...With value: " + virtualActualCost);
        }

        //progression part that has to be considered
        totalSum += virtualActualCost;
        //Debug.Log("Adding the SPECIAL VALUE part in the" + lvlObject.name.ToUpper() + " ...With value: " + virtualActualCost);

        for (int i = lvlObject.Objectives.startingProgressPoint; i < ((lvlObject.Objectives.startingProgressPoint-1) + lvlObject.Objectives.numberOfLevels); i++)
        {
            lastCostBuffer = virtualActualCost;
            virtualActualCost = virtualActualCost + (virtualActualCost - virtualLastCost + 25);
            virtualLastCost = lastCostBuffer;

            totalSum += virtualActualCost;
            //Debug.Log("Leaving the simulation part INCLUDED in the" + lvlObject.name.ToUpper() + "in iteration " + i + " ...With value: " + virtualActualCost);
        }

        //Debug.Log("Leaving the real part in the gold progression: LEVEL " + lvlObject.name.ToUpper() + "... With value: " + totalSum);
        return totalSum;
    }

    //in this function we set the xp needed to cover the levels that we have to level up the weapon after the level

    public float QuestProgressionXp(Level lvlObject)
    {
        float xpBuffer = 0f;

        for (int i = lvlObject.Objectives.startingProgressPoint; i <= ((lvlObject.Objectives.startingProgressPoint - 1) + lvlObject.Objectives.numberOfLevels); i++)
        {
            xpBuffer += (Mathf.Pow(i, 3) + 10);

            if (i == lvlObject.Objectives.startingProgressPoint)
            {
                //Debug.Log("First Value to added " + (lvlObject.name).ToUpper() + ": " + (Mathf.Pow(i, 3) + 10));
            }
            else if(i == ((lvlObject.Objectives.startingProgressPoint - 1) + lvlObject.Objectives.numberOfLevels))
            {
                //Debug.Log("Last Value to add " + (lvlObject.name).ToUpper() + ": " + (Mathf.Pow(i, 3) + 10) + "in value of i" + i + "ITERATION");
            }
        }

        //Debug.Log("The XP we must win in this run is:" + xpBuffer);
        return xpBuffer;
    }

    public float GetRelativeHp(int level)
    {
        float startingPoint = 50f;
        float lastXp = 0f;
        float bufferMaxHp = 0f;

        if (level == 1)
        {
            bufferMaxHp = startingPoint;
            return bufferMaxHp;
        }

        Debug.Log("Last Xp in iteration 1" + level + " : " + lastXp);
        Debug.Log("Actual Xp in interation 1" + level + " : " + startingPoint);

        while (level > 0)
        {
            bufferMaxHp = Mathf.Round((startingPoint + ((startingPoint / 1.2f) - (lastXp / 1.2f) + 10)));
            //Debug.Log("Max hp reached:" + bufferMaxHp);
            lastXp = startingPoint;
            startingPoint = bufferMaxHp;

            level--;
        }
        return bufferMaxHp;
    }

    public float HitProgressionGold(int lvls)
    {
        return 0;
    }
}
