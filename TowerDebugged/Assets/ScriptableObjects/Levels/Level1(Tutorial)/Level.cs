using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level/Level")]
public class Level : ScriptableObject
{
    public string flavorName = "";
    
    [SerializeField]
    private PlayerInfo playerInfo;

    [SerializeField]
    private Objectives objectives;

    //make a getter of the objectives
    public Objectives Objectives
    {
        get { return objectives; }
    }

    [SerializeField]
    private Rewards[] rewards;

    public Rewards[] Rewards
    {
        get { return rewards; }
        set { rewards = value; }
    }

    public bool updateParent = false;

    [SerializeField]
    private bool locked;

    public bool Locked
    {
        get 
        { 
            return locked; 
        }
        set 
        {
            updateParent = true;
            locked = value; 
        }
    }

    [SerializeField]
    //create a public type.enum with name Bioma
    private Bioma bioma;
    //create the bioma type enum

    [SerializeField]
    private Weather weather;

    public enum LevelState
    {
        Completed, 
        Incompleted
    }

    public LevelState state = LevelState.Incompleted;

    //this is used as a index in some sorting functions but it's dangerous, change it.
    public int id;
    private enum Weather
    {
        SUNNY,
        WINDY
    }

    [SerializeField]
    private Tier tier;
    public enum Tier
    {
        WOOD,
        STONE,
        DEMON
    }

    public int GetTier()
    {
        return (int)tier;
    }

    public float signCountdown = 0f;
    public int signTicks = 0;
    
    public bool actuallyPlaying = false;

    public bool isTutorial = false;
    
    [SerializeField]
    private float projectedGold = 0f;

    public float ProjectedGold
    {
        get { return projectedGold; }
        set { projectedGold = value; }
    }

    [SerializeField]
    private float projectedXp = 0f;

    public float ProjectedXp
    {
        get { return projectedXp; }
        set { projectedXp = value; }
    }
    // Start is called before the first frame update

    //set all the values of the level to 0
    [SerializeField]
    private string description;

    public string GetDescription
    {
        get { return description; }
    }
    public void SetBioma()
    {
        bioma.SetBioma(true);
    }

    public Bioma GetBioma()
    {
        return bioma;
    }

    public void NewGame()
    {
        locked = true;
    }
    public bool CheckNulls()
    {   
        if (objectives == null)
        {
            return true;
        }
        else if (rewards == null)
        {
            return true;
        }
        else if (bioma == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ResetLevel()
    {
        objectives.ObjectiveReset();
    }

    public XpReward GetXpReward()
    {
        //loop through all the rewards
        foreach (Rewards reward in rewards)
        {
            //if the reward is of type XpReward
            if (reward is XpReward)
            {
                //return the reward
                return (XpReward)reward;
            }
        }
        //if there is no reward of type XpReward return null
        return null;
    }

    //do the same with goldReward
    public GoldReward GetGoldReward()
    {
        //loop through all the rewards
        foreach (Rewards reward in rewards)
        {
            //if the reward is of type XpReward
            if (reward is GoldReward)
            {
                //return the reward
                return (GoldReward)reward;
            }
        }
        //if there is no reward of type XpReward return null
        return null;
    }

    //create a method named resetRewards that set the GetAmount of both to 0
    public void ResetRewards()
    {
        GetXpReward().GetAmount = 0;
        GetGoldReward().GetAmount = 0;
    }

    public void SetSign()
    {
        LevelTraveler.MyTravelInstance.Gear.signature.SetNumberTicks(signTicks);
        LevelTraveler.MyTravelInstance.Gear.signature.SetTimeBetweenTicks(signCountdown);
    }
}
