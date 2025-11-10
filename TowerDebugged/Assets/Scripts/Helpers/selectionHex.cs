using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class selectionHex : MonoBehaviour {

    private int floorLevel;

    Transform position;
    private GameObject gc;
    private GameObject slot;

    public GameObject forestTile;
    public GameObject riverTile;
    public GameObject lavaTile;
    public GameObject cleanTile;

    public GameObject redSelec;
    public GameObject blueSelec;
    public GameObject greenSelec;

    public bool red;
    public bool blue;
    public bool green;

    public GameObject tile;
    private Quaternion quat;

    public SpriteRenderer buttonSprite;

    void Start()
    {
        gc = GameObject.FindWithTag("GameController");
        slot = GameObject.FindWithTag("slot");
        quat = new Quaternion(0, 0.7071068f, 0, 0.7071068f);
        position = this.GetComponent<Transform>();
        floorLevel = this.gameObject.GetComponent<islandHolder>().islandClass.islandLevel;
        //Debug.Log(floorLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshBuyable()
    {
        Debug.Log("Refreshing price!");
        if (this.gameObject.GetComponent<islandHolder>().islandClass.Price <= StatController.MyInstance.GetLevelGold())
        {
            buttonSprite.sprite = UIController.MyUiInstance.deepButtonGold;
        }
        else
        {
            buttonSprite.sprite = UIController.MyUiInstance.deepButtonGrey;
        }
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (this.gameObject.GetComponent<islandHolder>().islandClass.Price > StatController.MyInstance.GetLevelGold())
            {
                Debug.Log("Can't purchase!");
                return;
            }

            UIController.MyUiInstance.SlotListActivation(true);
            this.gameObject.GetComponent<islandHolder>().SetAsPurchasedTile();
            FeedbackController.MyFeedbackInstance.ButtonFx();
            FeedbackController.MyFeedbackInstance.BuildFeedback();
            //if (gc.GetComponent<shopController>().InventoryCheck(gc.GetComponent<shopController>().choosedTile) == false)
            //{
            //    Debug.Log("YOU DONT HAVE " + gc.GetComponent<shopController>().choosedTile.name + " !! ");
            //    return;
            //}
            //if (gc.GetComponent<shopController>().choosedTile != null)
            //{
            //    //CREATE THE TILE
            //    InstantiateIsland(gc.GetComponent<shopController>().choosedTile, position.position, floorLevel);
            //    gc.GetComponent<buildController>().RemoveFromFloor(this.gameObject, position.position, floorLevel);
            //    SlotRefresh();

            //    gc.GetComponent<buildController>().hexList.Add(position.position);
            //    gc.GetComponent<buildController>().selectionVec.Clear();
            //    //Debug.Log("Destroying Selection Tile");
            //    Destroy(this.gameObject);
            //    gc.GetComponent<buildController>().CheckMegaIsland(gc.GetComponent<buildController>().floorsList[floorLevel]);
            //    Debug.Log(floorLevel);
            //}
            //else
            //{
            //    Debug.Log("nothing selected");
            //}
        }
    }

    public void InstantiateIsland(int index, GameObject tile, float futurePrice)
    {
        Debug.Log("Tile being instantiated:" + index);
        if (tile.name == "forestTile")
        {
            gc.GetComponent<buildController>().CreateTile(index, this.gameObject, position.position, tile, floorLevel, futurePrice);

        }
        if (tile.name == "riverTile")
        {
            gc.GetComponent<buildController>().CreateTile(index, this.gameObject, position.position, tile, floorLevel, futurePrice);
        }
        if (tile.name == "lavaTile")
        {
            gc.GetComponent<buildController>().CreateTile(index, this.gameObject, position.position, tile, floorLevel, futurePrice);
        }
        if (tile.name == "dirtyBasicTile")
        {
            gc.GetComponent<buildController>().CreateTile(index, this.gameObject, position.position, tile, floorLevel, futurePrice);
        }
    }

    
    //public void SlotRefresh()
    //{
    //    for (int i = 0; i < gc.GetComponent<shopController>().slotList.Count; i++)
    //    {
    //        if (gc.GetComponent<shopController>().choosedTile.name == gc.GetComponent<shopController>().slotList[i].GetComponent<shopSlot>().slotType)
    //        {
    //            //Debug.Log(gc.GetComponent<shopController>().choosedTile.name);
    //            gc.GetComponent<shopController>().slotList[i].GetComponent<shopSlot>().counter--;
    //            gc.GetComponent<shopController>().CheckSlotDeactivation(gc.GetComponent<shopController>().slotList[i].GetComponent<shopSlot>().counter, i);
    //        }
    //    }
    //}


}
