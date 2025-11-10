using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestController : MonoBehaviour
{
    private static QuestController questInstance;

    public static QuestController MyQuestInstance
    {
        get
        {
            if (questInstance == null)
            {
                questInstance = FindObjectOfType<QuestController>();
            }
            return questInstance;
        }


    }

    public GameObject prefabParent;
    public GameObject questPrefab;
    public TextAsset textJSON;

    public int maximumQuests = 3;

    [System.Serializable]
    public class QuestList
    {
        public Quest[] quest;
    }

    private bool dirty;
    public bool Dirty
    {
        get { return dirty; }
        set
        {
            if (value == dirty)
                return;

            dirty = value;
            if (dirty == true)
            {
                RefreshPanel();
            }
        }
    }

    public int ActiveQuests { get => activeQuests; set => activeQuests = value; }

    public QuestList myTotalQuestList = new QuestList();
    
    public List<Quest> completedQuests = new List<Quest>();

    public Dictionary<int, List<Quest>> myQuestListDictionary;

    private List<Quest> myBuildQuests = new List<Quest>();
    private List<Quest> myExploreQuests = new List<Quest>();
    private List<Quest> myGatherQuests = new List<Quest>();

    [SerializeField]
    private int activeQuests = 0;
    public int questsCompleted = 0;
    public float questPool = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        if (TutorialManager.Instance.isTutorial == true)
        {
            maximumQuests = 1;
            questPool = 1;
        }
        else
        {
            maximumQuests = 3;
            questPool = 3;
        }
        
        myQuestListDictionary = new Dictionary<int, List<Quest>>();
        myTotalQuestList = JsonUtility.FromJson<QuestList>(textJSON.text);

        dirty = false;

        int counter = 0;

        foreach (Quest item in myTotalQuestList.quest)
        {
            if (questPool >= 1f && item.Delivered != true)
            {
                CreateQuest(item, maximumQuests);
            }
            //set all the quests in the key 0 list - Build
            if (item.typeIndex == 0)
            {
                myBuildQuests.Add(item);
            }
            //set all the quests in the key 1 list - Recolect
            if (item.typeIndex == 1)
            {
                myGatherQuests.Add(item);
            }
            //set all the quests in the key 2 list - Explora
            if (item.typeIndex == 2)
            {
                myExploreQuests.Add(item);
            }
            counter++;
            //RectTransform rect = (RectTransform)newQuest.transform;
            //rect.localScale.Set(0.85f, 0.85f, 0.85f);
        }
        myQuestListDictionary.Add(0, myBuildQuests);
        myQuestListDictionary.Add(1, myGatherQuests);
        myQuestListDictionary.Add(2, myExploreQuests);

        foreach (var list in myQuestListDictionary)
        {
            Debug.Log("Quest List with key: " + list.Key + "witch contains: ");

            foreach (Quest quest in list.Value)
            {
                Debug.Log("Quest with name: " + quest.title);
            }
        }
    }
    public void SubstactActive()
    {
        ActiveQuests -= 1;
    }

    public void AddCompletedQuest(Quest quest)
    {
        //Push the quest to completedQuests
        completedQuests.Add(quest);

        LevelTraveler.MyTravelInstance.Level.Objectives.PushQuest(quest);
    }
    private void RefreshPanel()
    {
        //Debug.Log("Refreshing Panel");
        foreach (Quest item in myTotalQuestList.quest)
        {
            if (item.Active == true || item.Delivered == true)
            {
                continue;
            }
            if (ActiveQuests < maximumQuests && questPool >= 1f)
            {
                CreateQuest(item, questPool);
            }
        }

        Dirty = false;
    }
    
    //KEY IN THE PROGRESSION SYSTEM
    public void CreateQuest(Quest item, float _questPool)
    {
        GameObject newQuest = new GameObject();
        newQuest = Instantiate(questPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, prefabParent.transform);
        ProgressionController.MyProgressionController.SetQuestGold(item, _questPool);
        ProgressionController.MyProgressionController.SetQuestXp(item, _questPool);
        switch (ActiveQuests)
        {
            case 0:
                item.aimProgress = 1;
                break;
            case 1:
                item.aimProgress = Mathf.RoundToInt(LevelTraveler.MyTravelInstance.Level.Objectives.numberOfLevels / 2);
                break;
            case 2:
                item.aimProgress = LevelTraveler.MyTravelInstance.Level.Objectives.numberOfLevels;
                break;
            default:
                break;
        }
        newQuest.GetComponent<QuestHolder>().SetQuest(item);
        item.Active = true;
        ActiveQuests += 1;
        questPool -= 1f;
    }
}
