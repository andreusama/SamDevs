using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LifeHolder : MonoBehaviour
{
    public SpriteRenderer lifeImage;
    private Material barMaterial;
    public TextMeshPro text;
    public TextMeshPro level;

    public Color idleColor;
    public Color freezedColor;

    public GameObject debuffGm;
    // Start is called before the first frame update

    public Material GetMaterial()
    {
        return barMaterial;
    }

    public void Debuff(bool freezed)
    {
        
        if (LevelTraveler.MyTravelInstance.Level.isTutorial == true && PlayerStats.MyInstance.Debuff.Vidactual >= PlayerStats.MyInstance.Debuff.VidaM * 0.75)
            return;
        
        PlayerStats.MyInstance.Debuff.Vidactual += PlayerStats.MyInstance.Debuff.VidaM / 10;
        
        UpdateBar();

        if (skillController.MySkillInstance.GetSkillButtonScript().actualSkill != null)
        {
            skillController.MySkillInstance.GetSkillButtonScript().actualSkill.SetBiomaDebuffs(freezed);
        }
    }

    public void SetDebuffColor(Color newColor)
    {
        //Debug.Log("Entering the set debuff color function");
        if (debuffGm.GetComponent<DebuffHolder>() == null)
        {
            Debug.Log("Debuff Holder doesnt exist");
        }
        debuffGm.GetComponent<DebuffHolder>().SetColor(newColor);
    }
    private void Awake()
    {
        barMaterial = lifeImage.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateBar()
    {
        //Debug.Log("Updating Life Holder bar");
        if (PlayerStats.MyInstance.Debuff.Vidactual >= PlayerStats.MyInstance.Debuff.VidaM)
        {
            //died freezed
            buildController.MyBuildInstance.myState = buildController.AliveState.DEADBYFREEZE;
            buildController.MyBuildInstance.FailureEnd();
        }
        if (PlayerStats.MyInstance.Debuff.Vidactual > 0)
        {
            SetDebuffBarActive();
            skillButton.SkillButton.debuffImage.gameObject.SetActive(true);
            debuffGm.GetComponent<DebuffHolder>().UpdateDebuffBar();
        }
        else
        {
            if (lifeImage.transform.gameObject.activeSelf == false)
                SetIdleBarActive();

            //Debug.Log("Setting Life Holder bar");
            float actualHp = PlayerStats.MyInstance.Salud.Vidactual;
            float maxHp = PlayerStats.MyInstance.Salud.VidaM;
            barMaterial.SetFloat("_Fill", Map(actualHp, 0, maxHp, 0, 1));
            text.text = StatController.Aproximation(actualHp) + " / " + StatController.Aproximation(maxHp);
            level.text = buildController.MyBuildInstance.level.ToString();
        }
    }
    
    public void SetIdleBarActive()
    {
        skillButton.SkillButton.debuffImage.gameObject.SetActive(false);
        lifeImage.transform.gameObject.SetActive(true);
        text.transform.gameObject.SetActive(true);

        debuffGm.GetComponent<DebuffHolder>().lifeImage.transform.gameObject.SetActive(false);
        debuffGm.GetComponent<DebuffHolder>().freezeDeco.SetActive(false);
        debuffGm.GetComponent<DebuffHolder>().text.transform.gameObject.SetActive(false);

        if (skillController.MySkillInstance.GetSkillButtonScript().actualSkill != null)
        {
            skillController.MySkillInstance.GetSkillButtonScript().actualSkill.SetBiomaDebuffs(false);
        }
    }

    public void SetDebuffBarActive()
    {
        lifeImage.transform.gameObject.SetActive(false);
        text.transform.gameObject.SetActive(false);


        //Debug.Log("name:" + debuffGm.GetComponent<DebuffHolder>().transform.gameObject.name);
        debuffGm.GetComponent<DebuffHolder>().lifeImage.transform.gameObject.SetActive(true);
        debuffGm.GetComponent<DebuffHolder>().freezeDeco.SetActive(true);
        debuffGm.GetComponent<DebuffHolder>().text.transform.gameObject.SetActive(true);

        SetDebuffColor(freezedColor);
    }
    public void InitializeBar()
    {
        barMaterial.SetFloat("_Fill", 0);
        text.text = "0";
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        //vida escalable con vida actual
    }
}
