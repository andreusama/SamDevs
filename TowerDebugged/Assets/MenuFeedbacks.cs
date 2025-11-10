using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class MenuFeedbacks : MonoBehaviour
{
    private static MenuFeedbacks menuFeedbackInstance;

    public static MenuFeedbacks MyMenuFeedback
    {
        get
        {
            if (menuFeedbackInstance == null)
            {
                menuFeedbackInstance = FindObjectOfType<MenuFeedbacks>();
            }
            return menuFeedbackInstance;
        }


    }
    
    public MMFeedbacks flipEquip;
    public GameObject craftPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //create a function that plays the feedbacks of flipEquip
    public void PlayFlipEquip(craftHolder _holderToFlip)
    {
        _holderToFlip.FlipHolder();
        flipEquip.PlayFeedbacks();

        foreach (var item in CraftController.MyWorkshopInstance.GetHolders)
        {
            item.GetComponent<craftHolder>().DisableButton();
        }
    }

    public void AbleAllButtons()
    {
        foreach (var item in CraftController.MyWorkshopInstance.GetHolders)
        {
            item.GetComponent<craftHolder>().EnableButton();
        }
    }
}
