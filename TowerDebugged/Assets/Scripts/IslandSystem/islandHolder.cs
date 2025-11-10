using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class islandHolder : MonoBehaviour
{
    public Island islandClass;
    public GameObject mesh;
    public int floorLevel;
    public GameObject islandDropFeedback;

    public float visualPrice = 0f;


    private string sufix = "";


    public TextMeshPro priceText;

    public TextMeshPro amount;

    //all the data must be here
    public bool refresh = false;
    public bool build = false;
    public bool Refresh
    {
        get { return refresh; }
        set
        {
            refresh = value;

            if (value == refresh)
                return;

            if (refresh == true)
            {
                
            }
        }
    }

    public bool TimeToBuild
    {
        get { return build; }
        set
        {
            if (value == build)
                return;

            build = value;

            if (build == true)
            {
                this.transform.GetComponent<selectionHex>().InstantiateIsland(islandClass.id, buildController.MyBuildInstance.cleanTile, islandClass.Price);
                islandClass.Build = false;
                build = false;
            }
        }
    }

    public void SetVisualPrice(float myPrice)
    {
        visualPrice = myPrice;
        if (myPrice > 1000)
        {
            visualPrice = myPrice / 1000;
            sufix = "K";
        }

        if (priceText == null)
        {
            //Debug.Log("Price Text Error");
        }
        else
        {
            priceText.text = visualPrice.ToString() + sufix;
        }
    }
    // Start is called before the first frame update
    private void Awake()
    {
        islandClass.Price = buildController.MyBuildInstance.ActualIslandCost;
        mesh = this.gameObject.gameObject;
    }

    void Start()
    {
        SetUpText();
        SetVisualPrice(islandClass.Price);
    }

    // Update is called once per frame
    void Update()
    {
        if (islandClass.resourcesCollected == true)
        {
            Recolect();
        }

        if (islandClass.newStorage == true)
        {
            SetUpText();
        }

        if (islandClass.refresh == true)
        {
            this.transform.GetComponent<selectionHex>().RefreshBuyable();
            islandClass.refresh = false;
            refresh = false;
        }

        if (islandClass.Build == true)
        {
            TimeToBuild = true;
        }

        if (islandClass.GetMeshState == Island.MeshState.toTrue)
        {
            mesh.SetActive(true);
            islandClass.GetMeshState = Island.MeshState.idle;
            Debug.Log("Setting mesh to true!");
        }
        if (islandClass.GetMeshState == Island.MeshState.toFalse)
        {
            mesh.SetActive(false);
            islandClass.GetMeshState = Island.MeshState.idle;
            Debug.Log("Setting mesh to false!");
        }
    }

    public void CalcPrice()
    {

    }
    public void SetAsPurchasedTile()
    {
        buildController.MyBuildInstance.SetPurchasedIsland(this);
    }

    public void Recolect()
    {
        islandClass.Storage = 0f;
        islandClass.SetResources = 0f;
        islandClass.resourcesCollected = false;
    }

    public void SetUpText()
    {
        if (amount != null)
            amount.text = islandClass.Storage.ToString();
    }
}
