using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class shopController : MonoBehaviour {

    private GameObject gc;

    [Header("Tiles")]
    public GameObject forestTile;
    public GameObject riverTile;
    public GameObject lavaTile;
    public GameObject choosedTile;

    [Header("Slots Panel")]
    public GameObject cards;
    [Header("Build Layout")]
    public GameObject build_layout;
    [Header("Other Panels To Disable")]
    public GameObject bars;
    public GameObject skills;
    public GameObject quests;
    public GameObject money;
    public GameObject lifeBar;
    public GameObject inventory;

    [Header("Slot List")]
    [SerializeField]
    public List<GameObject> slotList = null;

    private int selectedCard;
    public bool destroyMode = false;
    private int actualSlotId;

    // Use this for initialization
    void Start()
    {
        choosedTile = forestTile;
        selectedCard = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void changeToGreenCard()
    {
        selectedCard = 2;
        choosedTile = forestTile;
        choosedTile.name = forestTile.name;
    }

    public void changeToRedCard()
    {
        selectedCard = 1;
        choosedTile = lavaTile;
        choosedTile.name = lavaTile.name;
    }

    public void changeToBlueCard()
    {
        selectedCard = 0;
        choosedTile = riverTile;
        choosedTile.name = riverTile.name;
    }

    public void DestroyMode()
    {
        destroyMode = !destroyMode;
    }

    public void SetActualId(int skillId)
    {
        actualSlotId = skillId;
    }

    //public bool InventoryCheck(GameObject tileToCheck)
    //{
    //    //cambiar a numeral y no depender de naming
    //    for (int i = 0; i < slotList.Count; i++)
    //    {
    //        if (tileToCheck.name == slotList[i].GetComponent<shopSlot>().slotType)
    //        {
    //            if (slotList[i].GetComponent<shopSlot>().counter == 0)
    //            {
    //                return false;
    //            }
    //        }
    //    }
    //    return true;
    //}

    //public void CheckSlotDeactivation(int counter, int index)
    //{
    //    if(counter == 0)
    //    {
    //        slotList[index].GetComponent<shopSlot>().image.color = new Color32(73, 73, 73, 210);
    //        slotList[index].GetComponent<shopSlot>().button.image.color = new Color32(255, 255, 255, 0);
    //    }
    //}

    //public void CheckSlotActivation(int counter, int index)
    //{
    //    if (counter > 0)
    //    {
    //        slotList[index].GetComponent<shopSlot>().image.color = new Color32(255, 255, 255, 255);
    //        slotList[index].GetComponent<shopSlot>().button.image.color = new Color32(255, 255, 255, 255);
    //    }
    //}

}
