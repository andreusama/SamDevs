using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gold Reward", menuName = "Level/Rewards/Gold")]
public class GoldReward : Rewards
{
    //create a function that applies the reward
    public override void ApplyReward()
    {
        //add the amount of gold to the player's gold
        
        StatController.MyInstance.AddGold(GetAmount);
        GetAmount = 0;
    }

    public void Store(float gold)
    {
        StatController.MyInstance.AddGold(gold);
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
