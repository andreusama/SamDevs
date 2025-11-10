using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Analytics;

[CreateAssetMenu]
public class Contract : Skill 
{
    private GameObject gc;
    [SerializeField]
    private float baseDamage;
    [SerializeField]
    private int numberTicks;
    [SerializeField]
    private int scaleFactor;
    [SerializeField]
    private float timeBtwTicks;

    private float lastDamage = 0;

    private float criticalBonus = 5f;

    //create getters for all the private variables
    public float GetBaseDamage()
    {
        return baseDamage;
    }

    public int GetNumberTicks()
    {
        return numberTicks;
    }

    public float GetTimeBtwTicks()
    {
        return timeBtwTicks;
    }

    public void SetBaseDamage(float baseDamage)
    {
        this.baseDamage = baseDamage;
    }

    public void AddBaseDamage(float baseDamage)
    {
        this.baseDamage += baseDamage;
    }
    public Contract(int id, float cooldown, string name, string type, Sprite sprite, float baseDamage, int numberTicks, float timeBtwTicks, bool buff)
    {
        this.id = id;
        this.cooldownTime = cooldown;
        this.name = name;
        this.type = type;
        this.sprite = sprite;
        this.sprite = sprite;
        this.baseDamage = baseDamage;
        this.numberTicks = numberTicks;
        this.timeBtwTicks = timeBtwTicks;
        this.isBuff = buff;
        this.broke = false;
        this.coldCd = 0.35f;
    }

    public override void Init()
    {
        gc = GameObject.FindWithTag("GameController");
        broke = false;
        actualStat = stat;
        coldCd = 1.0f;
        quantity = 1;
    }

    public override void SetBiomaDebuffs(bool isActive)
    {
        if (isActive == true)
        {
            coldCd = cooldownTime / 2;
        }
        else
        {
            coldCd = 1f;
        }
    }
    public override void Description(TextMeshProUGUI textObject)
    {
        textObject.text = "Damage: " + DamageCalc().ToString() + " /s (" + GetScaledBaseDmg().ToString() + "+" + "<color=yellow>" + ("penden to add") + "</color>" + ") \n" +
            "Cooldown: " + cooldownTime.ToString() + " s \n" +
            "Extensions: " + stat.ToString() + " steps \n" +
            "Validity: " + (numberTicks * timeBtwTicks).ToString() + "s \n";
    }

    public override void StaticDescription(TextMeshProUGUI textObject)
    {
        textObject.text = "Damage: " + DamageCalc().ToString() + " /s (" + GetScaledBaseDmg().ToString() + "+" + "<color=yellow>" + ("penden to add") + "</color>" + ") \n" +
            "Cooldown: " + cooldownTime.ToString() + " s \n" +
            "Extensions: " + stat.ToString() + " steps \n" +
            "Validity: " + (numberTicks * timeBtwTicks).ToString() + "s \n";
    }

    public override void GoldHit()
    {
        int newGold = 0;
        goldHit = StatController.MyInstance.avgGoldHit * cooldownTime;
        newGold = Mathf.RoundToInt((goldHit * (buildController.MyBuildInstance.GetLastMaxHp() / buildController.MyBuildInstance.GetMaxHp())));
        StatController.MyInstance.HitBag(newGold / 3);
    }
    public override void GoldHits(int hits)
    {
        int newGold = 0;
        newGold = Mathf.RoundToInt((goldHit /** (buildController.MyBuildInstance.GetLastXp() / buildController.MyBuildInstance.GetActualXp())*/));
        StatController.MyInstance.HitBag(newGold * hits);
        Debugger.MyTowerInstance.lastGoldHit.text = "Last GH" + newGold.ToString();
    }

    public override void SummonSpell(StatBar stat)
    {
        //GoldHit();

        lastDamage = 0;
        

        stat.Vidactual -= 1.0f;
        Debug.Log("Actual durability:" + stat.Vidactual);
        actualStat = stat.Vidactual;

        lastDamage = DamageCalc();

        
        gc.GetComponent<skillController>().ContractSummon(this);
        //Debug.Log("Contract!");

        if (stat.Vidactual <= 0)
        {
            Debug.Log("Broking!");
            broke = true;
            stat.Vidactual = stat.VidaM;
        }
    }

    public override void Refresh()
    {
        broke = false;
    }
    //CALC ALL DAMAGE
    private float DamageCalc()
    {
        if (buildController.MyBuildInstance == null)
        {
            return 0;

        }
        float baseDamage = Mathf.Round((Mathf.Sqrt(buildController.MyBuildInstance.GetMaxHp()) * (100 / scaleFactor)));
        return baseDamage /*(StatController.MyInstance.eloquence * 0.1f)*/;
    }

    //CALC BASE
    public override float GetScaledBaseDmg()
    {
        if (buildController.MyBuildInstance == null)
        {
            return 0;

        }
        float scaledDamage = Mathf.Round((Mathf.Sqrt(buildController.MyBuildInstance.GetMaxHp()) * (100 / scaleFactor)));
        return scaledDamage;
    }

    public override float GetScaledCriticalDmg()
    {
        if (buildController.MyBuildInstance == null)
        {
            return 0;

        }
        float scaledDamage = Mathf.Round((Mathf.Sqrt(buildController.MyBuildInstance.GetMaxHp()) * (100 / scaleFactor)));
        return scaledDamage + 5;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void Tick(StatBar stat)
    {
        
    }

    public override void NewGame()
    {
        baseDamage = 2.5f;
    }
}
