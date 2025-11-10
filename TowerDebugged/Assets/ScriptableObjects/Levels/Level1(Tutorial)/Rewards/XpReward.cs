using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Xp Reward", menuName = "Level/Rewards/XP")]
public class XpReward : Rewards
{
    //create a function that applies the reward
    public override void ApplyReward()
    {
        Debug.Log("Rewarding XP + actual Amount: " + GetAmount);
        //add the amount of gold to the player's gold
        StatController.MyInstance.AddXp(GetAmount);
        GetAmount = 0;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}

