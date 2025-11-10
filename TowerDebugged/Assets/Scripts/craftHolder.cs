using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using MoreMountains.Feel;
using MoreMountains.Tools;
using System.Net.Sockets;

public class craftHolder : MonoBehaviour
{

    public Recipe actualRecipe;
    public Image result;
    public Image backResult;
    public TextMeshProUGUI holderNameText;
    //PRICES
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI stats;
    public Image rarenessColor;
    public Image rarenessDecoColor;
    public bool infoOpened = false;
    public GameObject lockedUI;

    public GameObject gc;
    public GameObject InfoPanel;

    public Button internalButton;
    public Sprite activeButton;
    public Sprite unactiveButton;
    public Sprite bannedButton;

    public MMFeedbacks equipFeedback;

    public TextMeshProUGUI levelUpPrice;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI unlockLevel;

    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindWithTag("GameController");
        infoOpened = false;
        
        //with the MMSaveLoadManager create a if that checks if we have a saved game
        if (MMSaveLoadManager.Load(typeof(SavedData), "SavedGame" + ".brick") == null)
        {
            actualRecipe.NewGame();
            
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        if (MMSaveLoadManager.Load(typeof(SavedData), "SavedGame" + ".brick") == null)
        {
            actualRecipe.NewGame();
        }
        
        gc = GameObject.FindWithTag("GameController");
        holderNameText.text = actualRecipe.name;
        if (actualRecipe.result != null)
        {
            result.sprite = actualRecipe.result.sprite;
            backResult.sprite = actualRecipe.result.sprite;
        }
        else if(actualRecipe.skillResult != null)
        {
            result.sprite = actualRecipe.skillResult.sprite;
            backResult.sprite = actualRecipe.skillResult.sprite;
        }

        priceText.text = actualRecipe.GetPrice().ToString();
        if (actualRecipe.skillResult != null)
        SetRareness((int)actualRecipe.skillResult.rareness);

        if (actualRecipe.result != null)
        SetRareness((int)actualRecipe.result.rareness);

        if (actualRecipe.skillResult != null)
            UpdateUI();

        if (actualRecipe.skillResult != null)
            SetLevel();

        Lock(actualRecipe.locked);

        if(StatController.MyInstance.GetGold() >= actualRecipe.skillResult.GetLevelUpPrice())
        {
            internalButton.image.sprite = activeButton;
            internalButton.interactable = true;
        }
        else
        {
            internalButton.image.sprite = unactiveButton;
            internalButton.interactable = false;
        }


        //DEACTIVATE THIS ONLY PURPOSES OF DEBUG!!!
        //internalButton.image.sprite = activeButton;
        //internalButton.interactable = true;
    }

    private void SetLevel()
    {
        levelText.text = "Lvl " + actualRecipe.skillResult.visualLevel.ToString();
        //later on get it from the game data when loading;
        levelUpPrice.text = StatController.Aproximation((float)actualRecipe.skillResult.GetLevelUpPrice());
    }
    public void LevelUp()
    {
        //check that the Gamegold is enough
        if (StatController.MyInstance.GetGold() >= actualRecipe.skillResult.GetLevelUpPrice())
        {
            actualRecipe.skillResult.LevelUp();
            levelText.text = "Lvl " + actualRecipe.skillResult.visualLevel.ToString();
            levelUpPrice.text = StatController.Aproximation((float)actualRecipe.skillResult.GetLevelUpPrice());
            StatController.MyInstance.SubstractGold(actualRecipe.skillResult.GetLevelUpPrice());
        }
        CraftController.MyWorkshopInstance.RefreshRecipes();
    }

    public void Refresh()
    {
        Lock(actualRecipe.locked);

        if (StatController.MyInstance.GetGold() >= actualRecipe.skillResult.GetLevelUpPrice())
        {
            internalButton.image.sprite = activeButton;
            internalButton.interactable = true;
        }
        else
        {
            internalButton.image.sprite = unactiveButton;
            internalButton.interactable = false;
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        actualRecipe.skillResult.StaticDescription(stats);
        levelText.text = "Lvl " + actualRecipe.skillResult.visualLevel.ToString();
        levelUpPrice.text = StatController.Aproximation((float)actualRecipe.skillResult.GetLevelUpPrice());
    }
    public void CleanUp()
    {
        
    }

    public void DisableButton()
    {
        internalButton.image.sprite = unactiveButton;
        internalButton.interactable = false;
    }

    public void EnableButton()
    {
        if (StatController.MyInstance.GetGold() >= actualRecipe.skillResult.GetLevelUpPrice())
        {
            internalButton.image.sprite = activeButton;
            internalButton.interactable = true;
        }
        else
        {
            internalButton.image.sprite = unactiveButton;
            internalButton.interactable = false;
        }
    }

    public void FlipEquip()
    {
        MenuFeedbacks.MyMenuFeedback.PlayFlipEquip(this);
    }

    public void FlipHolder()
    {
        equipFeedback.PlayFeedbacks();
    }
    
    
    public void Fx()
    {
        FeedbackController.MyFeedbackInstance.PurchaseFxFeedback();
    }
    public void OpenInfo()
    {
        infoOpened = !infoOpened;
        InfoPanel.SetActive(infoOpened);
    }
    public void Craft()
    {
        //if (MenuFeedbacks.MyMenuFeedback.flipEquip.IsPlaying == true)
        //    return;
        
        FlipEquip();
        StartCoroutine(VisualEquiping());
        
        //StatController.MyInstance.SubstractResources(actualRecipe.price);
        //gc.GetComponent<CraftController>().Craft(this);
        //gc.GetComponent<CraftController>().RefreshRecipes();
    }

    private IEnumerator VisualEquiping()
    {
        yield return new WaitForSeconds(0.3f);
            
        MenuUI.MyMenuUiInstance.InitializeCraftPanel(result.sprite, holderNameText.text);
        LevelTraveler.MyTravelInstance.Gear.equipedSkill = actualRecipe.skillResult;

        yield break;
    }


    public void Lock(bool isLocked)
    {
        lockedUI.SetActive(isLocked);

    }

    private void SetRareness(int rareness)
    {
        switch (rareness)
        {
            case 0:
                rarenessColor.color = KingController.MyKingInstance.rarenessColors[0];
                rarenessDecoColor.color = KingController.MyKingInstance.rarenessColorsDeco[0];
                break;
            case 1:
                rarenessColor.color = KingController.MyKingInstance.rarenessColors[1];
                rarenessDecoColor.color = KingController.MyKingInstance.rarenessColorsDeco[1];
                break;
            case 2:
                rarenessColor.color = KingController.MyKingInstance.rarenessColors[2];
                rarenessDecoColor.color = KingController.MyKingInstance.rarenessColorsDeco[2];
                break;
            case 3:
                rarenessColor.color = KingController.MyKingInstance.rarenessColors[3];
                rarenessDecoColor.color = KingController.MyKingInstance.rarenessColorsDeco[3];
                break;
            case 4:
                rarenessColor.color = KingController.MyKingInstance.rarenessColors[4];
                rarenessDecoColor.color = KingController.MyKingInstance.rarenessColorsDeco[4];
                break;
            default:
                break;
        }
    }

}
