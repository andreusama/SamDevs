using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftController : MonoBehaviour
{
    [Header("Recipes List Parent")]
    public GameObject recipesGrid;

    [Header("Craft Prefab")]
    public GameObject craftPrefab;

    [Header("Recipes List")]
    [SerializeField]
    private List<GameObject> craftHolders;

    public List<GameObject> GetCraftHolders()
    {
        return craftHolders;
    }
    
    public List<GameObject> GetHolders
    {
        get
        {
            return craftHolders;
        }
    }

    private GameObject gc;

    public bool isCrafting;

    public Sprite noCraftingSprite;

    public int acutalId;

    public bool dirty;

    public RectTransform craftRect;
    public CraftHolder craftHolder;

    public SliderClamp layoutObject;


    private static CraftController workshopInstance;

    public static CraftController MyWorkshopInstance
    {
        get
        {
            if (workshopInstance == null)
            {
                workshopInstance = FindObjectOfType<CraftController>();
            }
            return workshopInstance;
        }


    }

    public void Awake()
    {
        craftHolder.Initialize();
    }

    // Use this for initialization
    void Start()
    {
        acutalId = -1;
        isCrafting = false;
        dirty = true;
        InitializeRecipes(MenuUI.MyMenuUiInstance.craftIndex);
    }

    public void SumWork()
    {
        MenuUI.MyMenuUiInstance.SumUpWorkshop();
    }

    //do the same with substract
    public void SubstractWork()
    {
        MenuUI.MyMenuUiInstance.SubstractWorkShop();
    }
    // Update is called once per frame
    void Update()
    {

        

    }

    public void SetActualId(int id)
    {
        acutalId = id;
    }

    public void InitializeRecipes(int i)
    {
        if (craftHolder.generalList.Count > 0)
        {
            foreach (Recipe recipe in craftHolder.generalList[i])
            {
                //UI instantiate
                GameObject typeObject = Instantiate(craftPrefab, recipesGrid.transform);
                typeObject.transform.localScale = new Vector3(1f, 1f, 1f);
                //
                //Skill Instantiate
                typeObject.gameObject.GetComponentInChildren<craftHolder>().actualRecipe = recipe;
                typeObject.gameObject.GetComponentInChildren<craftHolder>().Init();
                craftHolders.Add(typeObject);

                //Debug.Log("Created" + recipe.name);
            }
        }
        else
        {
            Debug.Log("Nothing happened");
        }

        //layoutObject.SnapTo(layoutObject.nearestGetter);

        Canvas.ForceUpdateCanvases();
    }

    public void RefreshRecipes()
    {
        foreach (var holder in craftHolders)
        {
            //Debug.Log(holder.name);
            holder.GetComponentInChildren<craftHolder>().Refresh();

        }
        Canvas.ForceUpdateCanvases();

    }

    public void CleanUpRecipes()
    {
        foreach (var holder in craftHolders)
        {
            holder.GetComponent<craftHolder>().CleanUp();
            //Debug.Log("holder to destroy" + holder.transform.gameObject.name);
            Destroy(holder.transform.gameObject);
        }
        Canvas.ForceUpdateCanvases();
        craftHolders.Clear();
        layoutObject.ClearBillboard();
    }
    

}

