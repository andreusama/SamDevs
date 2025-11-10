using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    private static InventoryController inventory;
    
    public static InventoryController MyInventory
    {
        get
        {
            if (inventory == null)
            {
                inventory = FindObjectOfType<InventoryController>();
            }
            return inventory;
        }


    }

    [Header("Skill Prefab")]
    [SerializeField]
    private GameObject skillPrefab;
    [SerializeField]
    private GameObject bagUi;
    

    [Header("SkillList")]
    [SerializeField]
    private List<GameObject> inventoryList;

    [Header("Inventory Variables")]
    public skillHolder nextHolder;

    public List<GameObject> GetSkillList()
    {
        return inventoryList;
    }

    public List<Skill> arsenal;

    private void Start()
    {
        inventoryList.Clear();
        if (LevelTraveler.MyTravelInstance != null)
            if (LevelTraveler.MyTravelInstance.Gear.equipedSkill != null)
            {
            EquipSkill(LevelTraveler.MyTravelInstance.Gear.equipedSkill);
            }

        //EquipSkill(inventoryList[0].GetComponent<skillHolder>());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CreateSkill(arsenal[1]);
            CreateSkill(arsenal[2]);
            CreateSkill(arsenal[3]);
        }
    }
    
    public void CreateSkill(Skill skill)
    {
        bool exists = false;
        foreach (GameObject skillHolderGm in inventoryList)
        {
            if (skillHolderGm.GetComponent<skillHolder>().internalSkill.id == skill.id)
            {
                //Debug.Log("Same type of hab!");
                skillHolderGm.GetComponent<skillHolder>().internalSkill.quantity++;
                skillHolderGm.GetComponent<skillHolder>().Refresh();
                exists = true;
                break;
            }
        }
        if (exists == false)
        {
            //UI instantiate
            GameObject typehab = Instantiate(skillPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            typehab.gameObject.transform.SetParent(bagUi.transform);
            typehab.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //
            //Text and Image changes
            //Skill Instantiate
            typehab.gameObject.GetComponentInChildren<skillHolder>().internalSkill = skill;
            typehab.gameObject.GetComponentInChildren<skillHolder>().Initialize();
            typehab.gameObject.GetComponentInChildren<skillHolder>().internalButton = null;
            inventoryList.Add(typehab);
        }
    }

    public void EquipSkill()
    {
        skillController.MySkillInstance.skillButtonScript.Init(nextHolder);

        FeedbackController.MyFeedbackInstance.SecButtonFx();

        UIController.MyUiInstance.BagSwitchUI();

    }

    public void EquipSkill(Skill _newSkill)
    {
        skillController.MySkillInstance.skillButtonScript.Equip(_newSkill);

        FeedbackController.MyFeedbackInstance.SecButtonFx();
    }

    public void UnEquipSkill(skillHolder holder)
    {
        //visual erase
        holder.internalImage.GetComponent<Image>().sprite = UIController.MyUiInstance.idleSprite;
        holder.internalImage.GetComponent<Image>().color = UIController.MyUiInstance.idleColor;
        Image shadow = holder.internalButton.gameObject.transform.Find("shadow").GetComponent<Image>();
        shadow.sprite = UIController.MyUiInstance.idleSprite;
        //technical button erase
        Debug.Log(holder.internalButton.GetComponent<skillButton>());
        holder.internalButton.GetComponent<skillButton>().actualCooldown = 0f;
        holder.internalButton.GetComponent<skillButton>().actualSkill = null;
        holder.internalButton.GetComponent<skillButton>().holder = null;


        //holder erase
        holder.equiped = false;
        holder.internalImage = null;
        holder.internalButton = null;
        //cooldown
        holder.internalBar.Vidactual = 0;
        holder.internalBar.VidaM = 0;
        holder.internalBar = null;
    }

    public void DestroySkill(GameObject skillHolder)
    {
        Debug.Log(skillHolder.GetComponent<skillHolder>());

        if (skillHolder.GetComponent<skillHolder>().internalButton != null)
            UnEquipSkill(skillHolder.GetComponent<skillHolder>());

        inventoryList.Remove(skillHolder);
        Destroy(skillHolder);
    }

    public void SetSkillToEquip(skillHolder _holder)
    {
        //check that _holder is not null
        if (_holder != null)
        {
            nextHolder = _holder;
        }
        else
        {
            Debug.Log("holder sended was null");
        }

    }
}
