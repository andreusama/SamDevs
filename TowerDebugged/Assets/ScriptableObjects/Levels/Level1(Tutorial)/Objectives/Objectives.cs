using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Objectives", menuName = "Level/Objectives")]
public class Objectives : ScriptableObject
{
    public int numberOfLevels = 10;
    public int finishedQuests = 0;
    public Quest[] questBuffer;

    public int startingProgressPoint = 0;

    public int averageProgressionReps = 0;
    // Start is called before the first frame update
    void Start()
    {
        numberOfLevels = 10;
        finishedQuests = 0;
    }

    public void PushQuest(Quest quest)
    {
        //create a new array with the size of the old array + 1
        Quest[] newStack = new Quest[questBuffer.Length + 1];
        //copy the old array to the new array
        Array.Copy(questBuffer, newStack, questBuffer.Length);
        //set the last element of the new array to the new object
        newStack[newStack.Length - 1] = quest;
        //set the new array to the old array
        questBuffer = newStack;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObjectiveReset()
    {
        finishedQuests = 0;
        for (int i = 0; i < questBuffer.Length; i++)
        {
            questBuffer[i] = null;

            Quest[] newStack = new Quest[0];
            questBuffer = newStack;
        }
    }
}
