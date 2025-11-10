using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stone Unlock", menuName = "Unlock/Stone Unlock")]
public class WeaponUnlock : Unlock
{
    //create a constructor and a destructor
    WeaponUnlock() { }
    ~WeaponUnlock() { }
    public override void UnlockItem()
    {
        unlockedRecipe.locked = false;
        //Debug.Log("From Weapon Unlock Item Function returning " + unlocked.ToString().ToUpper());
        unlocked = true;
        //add weapon to player inventory
        //set the offset damage in order to follow the progression of the game
        unlockedRecipe.SetProgressionLevel(levelRequisite);
    }

    public override int GetLvlRequisite()
    {
        return levelRequisite;
    }

    public override bool GetUnlocked()
    {
        //Debug.Log("From Weapon unlock returning " + unlocked.ToString().ToUpper());
        return unlocked;
    }
}
