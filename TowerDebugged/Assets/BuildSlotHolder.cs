using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BuildSlotHolder : MonoBehaviour
{
    public TextMeshProUGUI stats;
    public TextMeshProUGUI slotName;
    public Image sprite;
    public GameObject tileToBuild;

    public islandHolder tileToDelete;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTileToDelete()
    {
        tileToDelete = buildController.MyBuildInstance.GetPurchasedIsland();
    }
    public void Build()
    {
        if (buildController.MyBuildInstance.GetPurchasedIsland() == null)
            return;

        if (StatController.MyInstance.GetLevelGold() < tileToDelete.islandClass.Price)
        {
            return;
        }

        StatController.MyInstance.SubstractLevelGold((int)tileToDelete.islandClass.Price);

        buildController.MyBuildInstance.CreateTile(tileToDelete.islandClass.id, tileToDelete.gameObject, tileToDelete.transform.position, tileToBuild, tileToDelete.floorLevel, 0f);
        UIController.MyUiInstance.SlotListActivation(false);

        FeedbackController.MyFeedbackInstance.PurchaseFxFeedback();
        UIController.MyUiInstance.ActionSpaceOrg();
        buildController.MyBuildInstance.RefreshBuyableIslands();


    }
}
