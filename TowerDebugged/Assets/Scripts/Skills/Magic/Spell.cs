using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[CreateAssetMenu]
public class Spell : Skill
{
    private GameObject gc;

    [SerializeField]
    private float baseDamage;
    [SerializeField]
    private int numberTicks;
    private int completedTicks;
    [SerializeField]
    private float timeBtwTicks;
    [SerializeField]
    private float manaCost;
    [SerializeField]
    private float scaleFactor;

    [SerializeField]
    private Color spellColor;

    [SerializeField]
    private float durability;
    [SerializeField]
    private float maxDurability;

    private float lastDamage = 0;

    [SerializeField]
    private float criticalChance = 0f;

    public float LastDamage { get => lastDamage; set => lastDamage = value; }


    private float criticalBonus = 5;

    [SerializeField]
    private int defuffScale;


    public Spell(int id, float cooldown, string name, string type, Sprite sprite, float baseDamage, int numberTicks, float timeBtwTicks, bool buff)
    {
        this.id = id;
        this.cooldownTime = cooldown;
        this.name = name;
        this.type = type;
        this.sprite = sprite;
        this.baseDamage = baseDamage;
        this.numberTicks = numberTicks;
        this.completedTicks = 0;
        this.timeBtwTicks = timeBtwTicks;
        this.isBuff = buff;
        this.broke = false;
        this.coldCd = 0.35f;
    }

    public int GetCompletedTicks()
    {
        return completedTicks;
    }

    public int GetNumberTicks()
    {
        return numberTicks;
    }

    public void SetNumberTicks(int _newNumber)
    {
        numberTicks = _newNumber;
    }
    public float GetTimeBtwTicks()
    {
        return timeBtwTicks;
    }

    public void SetTimeBetweenTicks(float _newTime)
    {
        timeBtwTicks = _newTime;
    }

    public float GetDamageCalc()
    {
        return DamageCalc();
    }
    public Color GetColor()
    {
        return spellColor;
    }

    public void AddTick(int ticks)
    {
        completedTicks += ticks;
    }
    public override void Init()
    {
        gc = GameObject.FindWithTag("GameController");
        actualStat = stat;
        durability = maxDurability;
        quantity = 1;
        coldCd = 1.0f;
        broke = false;
    }
    public override void Description(TextMeshProUGUI textObject)
    {
        textObject.text = "Damage: " + DamageCalc().ToString() + " (" + GetScaledBaseDmg().ToString() + "+" + "<color=blue>" + (DamageCalc() - baseDamage) + "</color>" + ") \n" +
            "Cooldown: " + cooldownTime.ToString() + " s \n" +
            "Mana Cost: " + manaCost.ToString() + " mana \n" +
            "Durability: " + durability.ToString() + "/" + maxDurability.ToString();
    }

    public override void StaticDescription(TextMeshProUGUI textObject)
    {
        textObject.text = "Damage: " + DamageCalc().ToString() + " (" + GetScaledBaseDmg().ToString() + "+" + "<color=blue>" + (DamageCalc() - baseDamage) + "</color>" + ") \n" +
            "Cooldown: " + cooldownTime.ToString() + " s \n" +
            "Mana Cost: " + manaCost.ToString() + " mana \n" +
            "Durability: " + maxDurability.ToString() + "/" + maxDurability.ToString();
    }
    public override void SummonSpell(StatBar stat = null)
    {
        skillController.MySkillInstance.SpellSummon(this);
    }

    public override void Refresh()
    {
        broke = false;
        durability = maxDurability;
    }
    private float DamageCalc()
    {
        if (buildController.MyBuildInstance == null)
        {
            return 0;

        }
        baseDamage = Mathf.Round((Mathf.Sqrt(buildController.MyBuildInstance.GetMaxHp()) * (100 / scaleFactor)));
        return baseDamage /*(StatController.MyInstance.intellect * 0.1f)*/;
    }

    public override float GetScaledBaseDmg()
    {
        if (buildController.MyBuildInstance == null)
        {
            return 0;

        }
        float scaledDamage = Mathf.Round(((Mathf.Sqrt(buildController.MyBuildInstance.GetMaxHp()) * (100 / scaleFactor))));
        return scaledDamage;
    }

    public override float GetScaledCriticalDmg()
    {
        if (buildController.MyBuildInstance == null)
        {
            return 0;

        }
        float scaledDamage = Mathf.Round(((Mathf.Sqrt(buildController.MyBuildInstance.GetMaxHp()) * (100 / scaleFactor))));
        return (scaledDamage + 10);
    }

    public float CriticalHit()
    {
        int chance = Random.Range(0, 100);
        if (chance < criticalChance)
        {
            FeedbackController.MyFeedbackInstance.FeedbackCritical();
            //Debug.Log("Critical!");
            return 10;
        }
        else
        {
            return 0;
        }
    }
    public void Use()
    {

    }

    public override void GoldHit()
    {
        int newGold = 0;
        newGold = Mathf.RoundToInt((goldHit /** (buildController.MyBuildInstance.GetLastXp() / buildController.MyBuildInstance.GetActualXp())*/));
        StatController.MyInstance.HitBag(newGold);
        Debugger.MyTowerInstance.lastGoldHit.text = "Last GH" + newGold.ToString();
    }

    public override void GoldHits(int hits)
    {
        int newGold = 0;
        newGold = Mathf.RoundToInt((goldHit /** (buildController.MyBuildInstance.GetLastXp() / buildController.MyBuildInstance.GetActualXp())*/));
        StatController.MyInstance.HitBag(newGold * hits);
        Debugger.MyTowerInstance.lastGoldHit.text = "Last GH" + newGold.ToString();
    }

    public float ManaWaste()
    {

        return manaCost;
    }

    private float ManaRegen()
    {
        return 1.0f * Time.deltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
    // Update is called once per frame
    public override void Tick(StatBar stat)
    {
        stat.Vidactual += ManaRegen();

        if (TimeController.MyTimeInstance.freezeEventOn && coldCd > cooldownTime)
        {
            coldCd -= Map(1.2f, 0, 10f, 0, 1.0f) * Time.deltaTime;
            Debug.Log("Actual Cold Cd: " + coldCd);
        }
    }
}
