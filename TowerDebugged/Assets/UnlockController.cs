using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using MoreMountains.Tools;
using UnityEngine.Playables;

public class UnlockController : MonoBehaviour
{
    public List<Unlock> unlocks = new List<Unlock>();

    [SerializeField]
    private Transform unlockPanel;
    [SerializeField]
    private GameObject unlockPrefab;
    //create an instance that points to this class
    private static UnlockController instance;

    [Header("Unlock Feedback")]
    public RectTransform unlockParent;
    public GameObject unlockFeedbackPrefab;

    public static UnlockController MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UnlockController>();
            }
            return instance;
        }


    }

    public List<UnlockHolder> unlocksUI;

    //[System.Serializable]
    //public class UnlockList
    //{
    //    public Unlock[] unlocks;
    //}

    //create a method where a ID is sent, and the method returns the unlock with that ID
    public Unlock GetUnlock(int id)
    {
        //loop through the list of unlocks
        foreach (Unlock unlock in unlocks)
        {
            //if the unlock id is the same as the id sent
            if (unlock.GetID() == id)
            {
                //return the unlock
                return unlock;
            }
        }
        //if no unlock is found, return null
        return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (LevelTraveler.MyTravelInstance.gameData == null)
        {
            LoadUnlocks(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckUnlocks(int level, int levelId = 200)
    {
        //create a for loop that goes through all the unlocks
        foreach (Unlock item in unlocks)
        {
            //Debug.Log("Checking unlock: " + item.name + "Is unlocked?" + item.GetUnlocked().ToString().ToUpper());
            if (item.GetUnlocked() == true)
                continue;
            
            if (level >= item.GetLvlRequisite())
            {
                GameObject newUnlock = Instantiate(unlockPrefab, unlockPanel);
                //set the stretch of the newUnlock to be 0 on min and 1 in max in both x and y
                newUnlock.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                newUnlock.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);

                unlocksUI.Add(newUnlock.GetComponent<UnlockHolder>());
                //unlock the item
                newUnlock.SetActive(true);
                newUnlock.GetComponent<UnlockHolder>().Unlock(item);
                item.UnlockItem();
            }
        }
    }

    public void Feedback(string contentMessage, MenuUI.AnchorPresets anchor)
    {
        //instantiate the unlockFeedbackPrefab inside the unlockParent
        GameObject newUnlock = Instantiate(unlockFeedbackPrefab, unlockParent);
        newUnlock.GetComponent<UnlockFeedbackHolder>().content.text = "New Unlock!";

        //create a rectTransform variable and set it to the newUnlock's rectTransform
        RectTransform rectTransform = (RectTransform)newUnlock.transform;
        rectTransform = MenuUI.SetAnchoredPos(anchor, rectTransform);
        newUnlock.GetComponent<UnlockFeedbackHolder>().Flip(anchor);
    }
    public void Continue(UnlockHolder holderToDestroy)
    {
        //destroy the holder passed as argument from the list unlockUI
        unlocksUI.Remove(holderToDestroy);
        holderToDestroy.gameObject.SetActive(false);
        Destroy(holderToDestroy.gameObject);
        Feedback("New Hammer available", MenuUI.AnchorPresets.MiddleLeft);
    }

    public void LoadUnlocks(List<SerializableUnlock> _loadingUnlocks)
    {
        if (_loadingUnlocks == null)
        {
            //Debug.Log("No unlocks to load");
            foreach (Unlock item in unlocks)
            {
                item.LoadUnlockeds(false);
            }

            return;
        }
        foreach (Unlock item in unlocks)
        {
            foreach (SerializableUnlock toLoad in _loadingUnlocks)
            {
                if(item.GetID() == toLoad.id)
                {
                    //Debug.Log("Loading unlock of ID: + " + toLoad.id + "with value: " + toLoad.unlocked.ToString().ToUpper());
                    item.LoadUnlockeds(toLoad.unlocked);
                }
            }
        }
    }

}

