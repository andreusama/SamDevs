using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestAim
{
    public int currentProgress;
    public int aimProgress;

    public enum QuestType 
    {
        BUILD = 0,
        GATHERING = 1,
        EXPLORE = 2
    }

    public QuestType type;

    public QuestAim(int currentProgress, int aimProgress, QuestType type)
    {
        this.currentProgress = currentProgress;
        this.aimProgress = aimProgress;
        this.type = type;
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
