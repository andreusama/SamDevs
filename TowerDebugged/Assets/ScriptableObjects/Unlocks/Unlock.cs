using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlock : ScriptableObject
{
    public Recipe unlockedRecipe;
    [SerializeField]
    protected bool unlocked = true;
    [SerializeField]
    protected int levelRequisite = 0;

    [SerializeField]
    protected int id = 0;
    public virtual void UnlockItem() {}

    public enum UnlockType
    {
        Hammer,
        Worker
    }

    [SerializeField]
    protected UnlockType unlockType;

    //there has to be a difference of SetUnlock and LoadUnlock because setting the unlock level every time we Load it will cause the weapons to be stucked in the same level

    public virtual void LoadUnlockeds(bool newUnlocked)
    {
        unlocked = newUnlocked;
        unlockedRecipe.Unlock(!newUnlocked);
    }

    public virtual UnlockType GetUnlockType()
    {
        return unlockType;
    }
    public virtual bool GetUnlocked()
    {
        return false;
    }

    public virtual int GetLvlRequisite()
    {
        return 0;
    }
    
    public virtual int GetID()
    {
        return id;
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
