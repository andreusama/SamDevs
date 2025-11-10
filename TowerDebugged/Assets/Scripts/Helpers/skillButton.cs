using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Feedbacks;

public class skillButton : MonoBehaviour {

    //objective to only ose this
    public Skill actualSkill;

	public enum MODE
	{
		EQUIPING,
		SUMMONING
	}

	[SerializeField]
	private MODE mode;

    public MODE ActualMode
    {
        get
        {
            return mode;
        }

        set
        {
            mode = value;
        }
    }

    [SerializeField]
    public int id;
    [HideInInspector]
    public float actualCooldown;
    [HideInInspector]
    public bool cooling;

    [SerializeField]
    private Image cdFill;

	
    public skillHolder holder;

	public MMProgressBar internalBar;
    [SerializeField]
    private Image icon;
    
	public Image rareness;
	public Image rarenessDeco;

	public Image barFront;
	public Image backBar;

	public Color backR;
	public Color backB;
	public Color backY;

    public Image debuffImage;
		

	public GameObject gc;
	// Use this for initialization

	private static skillButton skillSlot;

	public MMFeedbacks weaponFeedback;

	public static skillButton SkillButton
	{
		get
		{
			if (skillSlot == null)
			{
				skillSlot = FindObjectOfType<skillButton>();
			}
			return skillSlot;
		}


	}
	void Start ()
	{
        debuffImage.gameObject.SetActive(false);
        gc = GameObject.FindWithTag("GameController");
		cooling = false; 
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (actualSkill != null && holder != null)
        {
			if (actualSkill.broke == true)
            {
				holder.RequestDestroy();
			}
		}
	}

    public void Interact()
    {
        switch (mode)
        {
            case MODE.EQUIPING:
				InventoryController.MyInventory.EquipSkill();
                UIController.MyUiInstance.UnEquipUI();
                mode = MODE.SUMMONING;
                break;
            case MODE.SUMMONING:
				Summon(true);
                break;
        }
    }

    public void Summon(bool isButton)
    {
        if (actualSkill == null)
            return;

        if (cooling == false)
        {
            if (TutorialManager.Instance.isTutorial == true)
            {
                if (!isButton)
                {
                    if (TutorialManager.Instance.isTutorial == true)
                    {
                        TutorialManager.Instance.NextPhase(TutorialManager.GAMEPLAY_TUTORIAL_PHASE.TOWERATTACK);
                    }
                }
                else
                {
                    TutorialManager.Instance.NextPhase(TutorialManager.GAMEPLAY_TUTORIAL_PHASE.ATTACK);

                }
            }
            StartCoroutine(AsyncFeedback());
            actualSkill.SummonSpell(skillController.MySkillInstance.selectedStat);
            StartCoroutine(Cooldown(actualSkill.cooldownTime));
        }
    }

    public IEnumerator AsyncFeedback()
    {
        weaponFeedback.PlayFeedbacks();
        yield break;
    }

    public void Init(skillHolder equiping)
    {
        holder = equiping;
        holder.internalBar = skillController.MySkillInstance.selectedStat;
        holder.SetBar();
        actualSkill = equiping.internalSkill;
        icon.sprite = actualSkill.sprite;
        SetButtonRareness();
        SetDurabilityStatColor();

        weaponFeedback.GetComponent<MMFeedbackSound>().Sfx = actualSkill.GetFx();

        equiping.internalSkill.SetBiomaDebuffs(TimeController.MyTimeInstance.freezeEventOn);

        //buildController.MyBuildInstance.SetFeedbackIntensity(equiping.internalSkill.GetScaledBaseDmg(), equiping.internalSkill.GetScaledCriticalDmg());
    }

    public void Equip(Skill _newSkill)
    {
        //we need the internalBar
        _newSkill.internalBar = skillController.MySkillInstance.selectedStat;
        _newSkill.SetBar();
        _newSkill.Init();
        actualSkill = _newSkill;
        icon.sprite = actualSkill.sprite;
        SetButtonRareness();
        SetDurabilityStatColor();
        weaponFeedback.GetComponent<MMFeedbackSound>().Sfx = actualSkill.GetFx();
        actualSkill.SetBiomaDebuffs(TimeController.MyTimeInstance.freezeEventOn);
        //buildController.MyBuildInstance.SetFeedbackIntensity(actualSkill.GetScaledBaseDmg(), actualSkill.GetScaledCriticalDmg());
    }
    
    public IEnumerator Cooldown(float _cd)
    {
        float maxCd = _cd;
        float actualCd = maxCd;
        float restedTime = 0.01f;
        cdFill.fillAmount = 1;
        cooling = true;
        while (cdFill.fillAmount > 0)
        {
            cdFill.fillAmount = skillController.MySkillInstance.Map(actualCd, 0, maxCd, 0, 1);
            yield return new WaitForSeconds(restedTime);
            actualCd -= restedTime;
        }
        cooling = false;
        yield break;
    }

    public void SetButtonRareness()
    {
		gc.GetComponent<KingController>().SetRareness(rareness, rarenessDeco, (int)actualSkill.rareness);
	}

	public void SetDurabilityStatColor()
    {
        if (actualSkill.type == "weapon")
        {
			barFront.color = Color.red;
			
			
			backBar.color = backR;
		}
        else if (actualSkill.type == "spell")
        {
			barFront.color = Color.blue;
			

			backBar.color = backB;
		}
        else if (actualSkill.type == "contract")
        {
			barFront.color = Color.yellow;
			

			backBar.color = backY;
		}
    }
}
