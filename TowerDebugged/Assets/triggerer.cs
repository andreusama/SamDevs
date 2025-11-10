using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class triggerer : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (this.transform.GetComponentInParent<islandHolder>().islandClass.Price > StatController.MyInstance.GetLevelGold())
                return;

            UIController.MyUiInstance.SlotListActivation(true);
            this.transform.GetComponentInParent<islandHolder>().SetAsPurchasedTile();
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
}
