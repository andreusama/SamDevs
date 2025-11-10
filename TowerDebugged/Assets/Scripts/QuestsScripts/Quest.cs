using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string title;
    public string objective;
    public float goldReward;
    public float xpReward;
    public int typeIndex;
    public int currentProgress;
    public int aimProgress;

    public QuestAim type;

    private bool dirty;
    private bool completed;
    private bool delivered;

        
    private bool unlocked;
    public bool Unlocked
    {
        get { return unlocked; }
        set
        {
            if (value == unlocked)
                return;

            unlocked = value;
            if (unlocked == true)
            {
                
            }
        }
    }

    private bool active;

    public bool Active
    {
        get { return active; }
        set
        {
            if (value == active)
                return;

            active = value;
            if (active == false)
            {
                Debug.Log("Finishing Quest");
                QuestController.MyQuestInstance.SubstactActive();
            }
        }
    }

    public bool Completed
    {
        get { return completed; }
        set
        {
            if (value == completed)
                return;

            completed = value;
            if (completed == true)
            {
                QuestController.MyQuestInstance.AddCompletedQuest(this);
            }
        }
    }
    public bool Dirty { get => dirty; set => dirty = value; }
    public bool Delivered { get => delivered; set => delivered = value; }
    


    public Quest(string title, string objective, int goldReward, int xpReward, int typeIndex, int currentProgress, int aimProgress)
    {
        this.title = title;
        this.objective = objective;
        this.goldReward = goldReward;
        this.xpReward = xpReward;

        this.typeIndex = typeIndex;
        this.currentProgress = currentProgress;
        this.aimProgress = aimProgress;

        dirty = false;
        completed = false;
        delivered = false;
        active = false;
        unlocked = false;

        switch (typeIndex)
        {
            case 0:
                type = new QuestAim(currentProgress, aimProgress, QuestAim.QuestType.BUILD);
                break;
            case 1:
                type = new QuestAim(currentProgress, aimProgress, QuestAim.QuestType.GATHERING);
                break;
            case 2:
                type = new QuestAim(currentProgress, aimProgress, QuestAim.QuestType.EXPLORE);
                break;
            default:
                break;
        }
        
    }

    public void Reward()
    {
        //Debug.Log("Rewarding with gold:" + reward);
        StatController.MyInstance.AddLevelGold(goldReward);
        StatController.MyInstance.AddLevelXP(xpReward);
        delivered = true;
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
