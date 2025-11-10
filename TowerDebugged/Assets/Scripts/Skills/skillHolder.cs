using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class skillHolder : MonoBehaviour {

	[HideInInspector]
	public int id;
	public Color32 color;
	public Image image;
	public float cooldown;
	public TextMeshProUGUI skillName;
	public TextMeshProUGUI skillTypeName;
	public TextMeshProUGUI statsText;
	public bool equiped;
	public GameObject internalButton;
	public Image internalImage;
	public StatBar internalBar;
	public GameObject infoPanel;
	private bool infoOpen;
	public TextMeshProUGUI quantityText;

	public Image rarenessImage;
	public Image rarenessImageDeco;

	public Skill internalSkill;

	private GameObject gc;

	public GameObject tutorialPanel;

	private enum type
	{
		weapon,
		magic,
		contract
	};
	public string skillType;
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void SwitchInfo()
    {
		infoOpen = !infoOpen;
		infoPanel.SetActive(infoOpen);
    }
    public void Initialize()
    {
		gc = GameObject.FindWithTag("GameController");
		//id and cooldown
		internalSkill.Init();
		id = internalSkill.id;
		cooldown = internalSkill.cooldownTime;
		//ui
		skillName.text = internalSkill.name;
		skillTypeName.text = internalSkill.type;
		skillType = internalSkill.type;
		quantityText.text = "x" + internalSkill.quantity.ToString();
		
		//sprite
		image.sprite = internalSkill.sprite;

		gc.GetComponent<KingController>().SetRareness(rarenessImage, rarenessImageDeco, (int)internalSkill.rareness);
	}

	public void Refresh()
    {
		quantityText.text = "x" + internalSkill.quantity.ToString();
	}

	public void UpdateUI()
    {
		internalSkill.Description(statsText);
	}
	
	public void Fx()
    {
		FeedbackController.MyFeedbackInstance.SecButtonFx();
    }

	public void DestroyFx()
	{
		FeedbackController.MyFeedbackInstance.DestroyFx();
	}
	public void RequestDestroy()
    {
		internalSkill.quantity--;
		if (internalSkill.quantity > 0)
        {
			internalSkill.Refresh();
			Refresh();
        }
        else
        {
			gc = GameObject.FindWithTag("GameController");
			gc.GetComponent<InventoryController>().DestroySkill(this.transform.gameObject);
		}
	}
	public void SetBar()
	{
		internalBar.VidaM = internalSkill.stat;
		internalBar.Vidactual = internalBar.VidaM;
		internalBar.Vidactual = internalSkill.actualStat;
	}
}
