using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class resourceGenerator : MonoBehaviour {

    [Header("Generator Settings")]
    //--------GENERATION TIME------
    public float generationTime = 1;
    //--------RESOURCES GENERATED---------
	public int resourcesGenerated = 1;
    //----------CAP CAPACITY------
    public const float maxCapacity = 10;
    //---------CURRENT STORAGE-----------
    private int currentStorage;
    //----------BOOL IS GENERATING-----------
    private bool isGenerating;
    bool full;
    [Header("Elements Dropped By This Tile")]
    public List<Element> droppeableElements;
    [Header("Elements Storaged")]
    public List<Element> storagedElements;
    [Header("Clock Settings")]
    public MeshRenderer fillRenderer;
    private Material clockWise;


    public Sprite resourcesSprite;

    private GameObject gc;

    public MMFeedbacks clockFeedback;
    public MMFeedbacks recolectFeedback;

    private float luck;

    // Use this for initialization
    void Start () 
    {
		currentStorage = 0;
		isGenerating = false;
        clockWise = fillRenderer.material;
        gc = GameObject.FindWithTag("GameController");
        clockWise.SetFloat("_Progress", 1);
    }
	
	// Update is called once per frame
	void Update () 
    {
        // Si encara queda espai a les reserves i no estem generant recursos, generem els recursos.
        if (this.transform.GetComponent<islandHolder>().islandClass.Storage < maxCapacity && !isGenerating)
        {
            StartCoroutine("Generate");
		}
	}

	IEnumerator Generate()
    {
		isGenerating = true;
        float actualCd = generationTime;
        while (actualCd > 0)
        {
            clockWise.SetFloat("_Progress", Map(actualCd, 0, generationTime, 1, 0));
            //clockWise.color = Color.Lerp(Color.green, Color.red, Map(actualCd, 0, generationTime, 0, 1));
            yield return new WaitForSeconds(0.01f);
            actualCd -= 0.01f;
        }
        //Drop(droppeableElements);
        //LuckyDrop(droppeableElements);
        this.transform.GetComponent<islandHolder>().islandClass.Storage += resourcesGenerated;

        clockFeedback.PlayFeedbacks();

        isGenerating = false;

        yield break;
    }
    //public async void Farm()
    

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        //vida escalable con vida actual
    }

    public int Recolect()
    {
        int recolect = 0;

        if (currentStorage > 0)
        {
            //recolectFeedback.PlayFeedbacks();
            //StartCoroutine(gc.GetComponent<UIController>().feedbackMomento(gameObject.GetComponent<islandHolder>().islandDropFeedback, resourcesSprite, currentStorage));
            //Debug.Log("Lake");
            recolect += currentStorage;
            currentStorage = 0;
        }

        if (QuestController.MyQuestInstance.myQuestListDictionary.ContainsKey(1))
        {
            foreach (Quest quest in QuestController.MyQuestInstance.myQuestListDictionary[1])
            {
                if (quest.Active == true)
                {
                    quest.currentProgress += recolect;
                    quest.Dirty = true;
                }
            }
        }
        //Gathering Quests


        return recolect;
    }
  //  void OnMouseOver()
  //  {
		//if (Input.GetMouseButtonDown(0))
  //      {
		//	if(gameObject.tag == "Lake")
  //          {
  //              //if (storagedElements.Count != 0)
  //              //{
  //              //    gc.GetComponent<InventoryController>().AddElement(storagedElements);
  //              //    foreach (var element in storagedElements)
  //              //    {
  //              //        StartCoroutine(gc.GetComponent<UIController>().feedbackMomento(gameObject.GetComponent<islandHolder>().islandDropFeedback, element.sprite, 1));
  //              //    }
  //              //    storagedElements.Clear();
  //              //}
  //              //currentStorage = gc.GetComponent<buildController>().AddLake(currentStorage);
  //              if(currentStorage > 0)
  //              {
  //                  StatController.MyInstance.AddResources(currentStorage);
  //                  recolectFeedback.PlayFeedbacks();
  //                  StartCoroutine(gc.GetComponent<UIController>().feedbackMomento(gameObject.GetComponent<islandHolder>().islandDropFeedback, resourcesSprite, currentStorage));
  //                  //Debug.Log("Lake");
  //              }
                
		//	}
		//	else if (gameObject.tag == "Forest")
  //          {
  //              //if (storagedElements.Count != 0)
  //              //{
  //              //    gc.GetComponent<InventoryController>().AddElement(storagedElements);
  //              //    foreach (var element in storagedElements)
  //              //    {
  //              //        StartCoroutine(gc.GetComponent<UIController>().feedbackMomento(gameObject.GetComponent<islandHolder>().islandDropFeedback, element.sprite, 1));
  //              //    }
  //              //    storagedElements.Clear();
  //              //}
  //              //currentStorage = gc.GetComponent<buildController>().AddForest(currentStorage);
  //              if (currentStorage > 0)
  //              {
  //                  StatController.MyInstance.AddResources(currentStorage);
  //                  recolectFeedback.PlayFeedbacks();
  //                  StartCoroutine(gc.GetComponent<UIController>().feedbackMomento(gameObject.GetComponent<islandHolder>().islandDropFeedback, resourcesSprite, currentStorage));
  //                  //Debug.Log("F");
  //              }
  //          }
		//	else if (gameObject.tag == "Volcano")
  //          {
  //              //if (storagedElements.Count != 0)
  //              //{
  //              //    gc.GetComponent<InventoryController>().AddElement(storagedElements);
  //              //    foreach (var element in storagedElements)
  //              //    {
  //              //        StartCoroutine(gc.GetComponent<UIController>().feedbackMomento(gameObject.GetComponent<islandHolder>().islandDropFeedback, element.sprite, 1));
  //              //    }
  //              //    storagedElements.Clear();
  //              //}
  //              if (currentStorage > 0)
  //              {
  //                  StatController.MyInstance.AddResources(currentStorage);
  //                  recolectFeedback.PlayFeedbacks();
  //                  StartCoroutine(gc.GetComponent<UIController>().feedbackMomento(gameObject.GetComponent<islandHolder>().islandDropFeedback, resourcesSprite, currentStorage));
  //                  //Debug.Log("V");
  //              }
  //          }

		//	currentStorage = 0;

  //      }
  //  }

    private Element Drop(List<Element> droppeableElements)
    {
        foreach (var item in droppeableElements)
        {
            Debug.Log("Random Number" + Random.value);
            Debug.Log("Drop Chance" + item.dropChance);
            if (Random.value < item.dropChance)
            {
                Debug.Log("item created" + item.name);
                storagedElements.Add(item);
            }
        }


        return null;
    }


    private Element LuckyDrop(List<Element> droppeableElements)
    {



        return null;
    }



}
