using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    private static MenuUI menuUiInstance;

    public static MenuUI MyMenuUiInstance
    {
        get
        {
            if (menuUiInstance == null)
            {
                menuUiInstance = FindObjectOfType<MenuUI>();
            }
            return menuUiInstance;
        }


    }

    public enum AnchorPresets
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    public AnchorPresets anchoredPos;
        
    public Image craftedElementIcon;
    public TextMeshProUGUI craftedText;

    public Image activeSprite;
    public TextMeshProUGUI activeNameText;
    public GameObject activePanel;
    //Contract UI
    public TextMeshProUGUI contractDps;

    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Image buttonImage;

    //0 for activated
    //1 for deactivated
    public List<Sprite> buttonStates;

    public GameObject tutorialPanel;
    
    public void Awake()
    {
        UpdateContractInfo();
    }

    public void UpdateContractInfo()
    {
        contractDps.text = LevelTraveler.MyTravelInstance.Gear.contract.GetBaseDamage().ToString();
    }
    public int craftIndex = 0;
    public void OpenWorkShop()
    {
        CraftController.MyWorkshopInstance.CleanUpRecipes();
        CraftController.MyWorkshopInstance.InitializeRecipes(craftIndex);
    }

    public void SumUpWorkshop()
    {
        craftIndex++;

        if (craftIndex > 2)
        {
            craftIndex = 0;
            //workshopController.MyWorkshopInstance.dirty = true;
        }

        CraftController.MyWorkshopInstance.CleanUpRecipes();
        CraftController.MyWorkshopInstance.InitializeRecipes(craftIndex);
    }

    public void SubstractWorkShop()
    {
        craftIndex--;

        if (craftIndex < 0)
        {
            craftIndex = 2;
            //workshopController.MyWorkshopInstance.dirty = true;
        }

        CraftController.MyWorkshopInstance.CleanUpRecipes();
        CraftController.MyWorkshopInstance.InitializeRecipes(craftIndex);
    }

    public void InitializeCraftPanel(Sprite craftedElementSprite, string craftedElementName)
    {
        craftedElementIcon.sprite = craftedElementSprite;
        craftedText.text = craftedElementName;
    }

    public void InitializeActivePanel(Sprite activeSprite, string name)
    {
        this.activeSprite.sprite = activeSprite;
        activeNameText.text = name;
    }

    public void OpenMenuUI()
    {
        if (activePanel.activeSelf == false)
        {
            activePanel.SetActive(true);
        }
        else
        {
            activePanel.SetActive(false);
        }
    }

    public static RectTransform SetAnchoredPos(AnchorPresets newAnchor, RectTransform rectToSet)
    {
        //create a switch that takes the newAnchor and modifies the Anchors min and max values aswell as the pivot in order to math the logical anchor of the enum
        switch (newAnchor)
        {
            case AnchorPresets.TopLeft:
                rectToSet.anchorMin = new Vector2(0, 1);
                rectToSet.anchorMax = new Vector2(0, 1);
                rectToSet.pivot = new Vector2(0, 1);
                break;
            case AnchorPresets.TopCenter:
                rectToSet.anchorMin = new Vector2(0.5f, 1);
                rectToSet.anchorMax = new Vector2(0.5f, 1);
                rectToSet.pivot = new Vector2(0.5f, 1);
                break;
            case AnchorPresets.TopRight:
                rectToSet.anchorMin = new Vector2(1, 1);
                rectToSet.anchorMax = new Vector2(1, 1);
                rectToSet.pivot = new Vector2(1, 1);
                break;
            case AnchorPresets.MiddleLeft:
                rectToSet.anchorMin = new Vector2(0, 0.5f);
                rectToSet.anchorMax = new Vector2(0, 0.5f);
                rectToSet.pivot = new Vector2(0, 0.5f);
                break;
            case AnchorPresets.MiddleCenter:
                rectToSet.anchorMin = new Vector2(0.5f, 0.5f);
                rectToSet.anchorMax = new Vector2(0.5f, 0.5f);
                rectToSet.pivot = new Vector2(0.5f, 0.5f);
                break;
            case AnchorPresets.MiddleRight:
                rectToSet.anchorMin = new Vector2(1, 0.5f);
                rectToSet.anchorMax = new Vector2(1, 0.5f);
                rectToSet.pivot = new Vector2(1, 0.5f);
                break;
            case AnchorPresets.BottomLeft:
                rectToSet.anchorMin = new Vector2(0, 0);
                rectToSet.anchorMax = new Vector2(0, 0);
                rectToSet.pivot = new Vector2(0, 0);
                break;
            case AnchorPresets.BottomCenter:
                rectToSet.anchorMin = new Vector2(0.5f, 0);
                rectToSet.anchorMax = new Vector2(0.5f, 0);
                break;
        }
        return rectToSet;
    }

    public void PlayAvailability(bool _locked)
    {
        if (_locked) 
        {
            playButton.interactable = false;
            buttonImage.sprite = buttonStates[1];
        }
        else
        {
            playButton.interactable = true;
            buttonImage.sprite = buttonStates[0];
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {

            //LevelTraveler.MyTravelInstance.LoadData();
        }
    }
}

