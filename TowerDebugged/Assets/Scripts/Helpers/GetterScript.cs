using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetterScript : MonoBehaviour {

	//her parent
	private GameObject gc;

	// Use this for initialization
	void Start ()
	{
		gc = GameObject.FindWithTag("GameController");
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

    public TextMeshProUGUI GetSlotText()
    {

        return this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

	public void GetSkill()
    {
        gc.GetComponent<InventoryController>().SetSkillToEquip(this.GetComponentInParent<skillHolder>());
    }

	public void EquipMode()
    {
		skillController.MySkillInstance.skillButtonScript.ActualMode = skillButton.MODE.EQUIPING;
		UIController.MyUiInstance.EquipUI();
	}

	public void GetButtonId()
    {
		if (this.GetComponent<skillButton>().id != skillController.MySkillInstance.skillButtonScript.id)
		{
			Debug.Log("Setting the Script of the Skill Controller");
            skillController.MySkillInstance.SetActualButton(this.GetComponent<skillButton>());
        }
    }

	public void GetSkillToDestroy()
	{
		Debug.Log(this.transform.gameObject.GetComponentInParent<GameObject>().gameObject.name);
	}

	public void GetFilterType()
    {

    }

	public void GetPanelId()
    {
		gc.GetComponent<CraftController>().SetActualId(this.GetComponent<WorkshopButton>().id);
	}


}
