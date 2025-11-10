using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Tools;

[CreateAssetMenu]
public class Weapon : Skill
{
    private GameObject gc;
    [SerializeField]
    private float baseDamage;
    [SerializeField]
    private float criticalChance;
    [SerializeField]
    private float scaleFactor;

    private float lastDamage = 0;

    [SerializeField]
    private float durability;
    [SerializeField]
    private float maxDurability;

    [SerializeField]
    private int defuffScale;

    private bool rage = false;
    private float rageCounter = 0f;

    [SerializeField]
    private int originalPrice;

    private float damageOffset = 0f;
    public Weapon(int id, float cooldown, string name, string type, Sprite sprite, float baseDamage, float criticalChance, int price = 5)
    {
        this.id = id;
        this.cooldownTime = cooldown;
        this.name = name;
        this.type = type;
        this.sprite = sprite;
        this.baseDamage = baseDamage;
        this.criticalChance = criticalChance;
        this.broke = false;
        this.coldCd = 1.0f;
        this.levelUpPrice = price;
        gc = GameObject.FindWithTag("GameController");
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
        textObject.text = "Damage: " + DamageCalc().ToString() + " (" + GetScaledBaseDmg().ToString() + "+" + "<color=red>"+ (StrenghtBonus()) + "</color>" + ") \n" + 
            "Cooldown: " + cooldownTime.ToString() + " s \n" +
            "Critical Chance: " + (criticalChance).ToString() + " % \n" +
            "Durability: " + durability.ToString() + "/" + maxDurability.ToString();
    }

    public override void StaticDescription(TextMeshProUGUI textObject)
    {
        textObject.text = "Damage: " + DamageCalc().ToString() + " (" + GetScaledBaseDmg().ToString() + "+" + "<color=red>" + (StrenghtBonus()) + "</color>" + ") \n" +
            "Cooldown: " + cooldownTime.ToString() + " s \n" +
            "Critical Chance: " + (criticalChance).ToString() + " % \n"; /*+
            "Durability: " + maxDurability.ToString() + "/" + maxDurability.ToString();*/
    }

    public override void CleanUp()
    {

    }

    public override void GoldHit()
    {
        int newGold = 0;
        newGold =  Mathf.RoundToInt((goldHit));
        StatController.MyInstance.HitBag(newGold);
    }

    public override void GoldHits(int hits)
    {
        int newGold = 0;
        newGold = Mathf.RoundToInt((goldHit));
        StatController.MyInstance.HitBag(newGold * hits);
    }

    public override void SetBiomaDebuffs(bool isActive)
    {
        if (isActive == true)
        {
            cooldownTime = Map(PlayerStats.MyInstance.Debuff.Vidactual / PlayerStats.MyInstance.Debuff.VidaM, 0, 1, 0.02f, 1f);
            //Debug.Log("Cooldown freezed: " + cooldownTime);
            //cooldownTime = 1f;
        }
        else
        {
            cooldownTime = 0.01f;
        }

        //Debug.Log("Cold Cd Setted At: " + coldCd);
    }
    public override void SummonSpell(StatBar stat)
    {
        GoldHit();

        if (skillController.MySkillInstance.resting == false)
            //skillController.MySkillInstance.castCancelled = true;

        lastDamage = 0;
        //CHECK IF BUFF
        if (isBuff == true && buff != null)
        skillController.MySkillInstance.CreateBuff(buff, sprite, 0, 0);

        
        //ADD LIFE
        //Debug.Log("Weapon DMG:" + baseDamage + /*CriticalHit()*/ "hp");

        if (rage == true)
        {
            lastDamage = DamageCalc() * RageBonus();
            FeedbackController.MyFeedbackInstance.FeedbackIra();
        }
        else
        {
            lastDamage = (DamageCalc() * CriticalHit());
        }

        //DAMAGE FEEDBACK
        if (gc.GetComponent<buildController>().actualTower != null)
        {
            gc.GetComponent<buildController>().SwitchFloatTextDir(0, 200);
            gc.GetComponent<buildController>().SetFeedbackIntensity(GetScaledBaseDmg(), GetScaledCriticalDmg());
            gc.GetComponent<buildController>().DamageFeedback(2, lastDamage);
        }
        
        //AND THEN APPLY IT
        if (gc.GetComponent<buildController>().actualTower != null && PlayerStats.MyInstance.Debuff.Vidactual > 0)
        {
            PlayerStats.MyInstance.Debuff.Vidactual -= lastDamage;

            cooldownTime = Map(PlayerStats.MyInstance.Debuff.Vidactual / PlayerStats.MyInstance.Debuff.VidaM, 0, 1, 0.02f, 1f);
            Debug.Log("Cooldown freezed: " + cooldownTime);
        }
        else
        {
            buildController.MyBuildInstance.SumLife(lastDamage);
        }

        //buildController.MyBuildInstance.actualTower.GetComponent<TowerHolder>().Hitted = true;

        durability--;

        if (rage == false)
        {
            stat.Vidactual += Ira();
        }
        
        actualStat = stat.Vidactual;



        if (durability <= 0)
        {
            broke = true;
        }
    }

    public override void Refresh()
    {
        broke = false;
        durability = maxDurability;
    }
    public void AddQuantity()
    {
        quantity++;
    }
    public void SubstractQuantity()
    {
        quantity--;
    }

    private float DamageCalc()
    {
        //Damage Formula
        float baseDamage = Mathf.Round((Mathf.Sqrt(GetRelativeHp(this.level)) * (100 / scaleFactor)) + damageOffset);
        //Debug.Log("Damage from Weapon: " + Mathf.Round((Mathf.Sqrt(GetRelativeHp(this.level)) * (100 / scaleFactor))));
        //Debug.Log("Damage from OFFset of last weapon: " + damageOffset);
        if (buildController.MyBuildInstance != null)
        {
            //Debug.Log("When intensity is: " + buildController.MyBuildInstance.actualTower.GetComponent<TowerHolder>().DamageFeedback.Feedbacks[10].Timing.IntensityIntervalMin);
        }
        return baseDamage;
    }

    public override float GetScaledBaseDmg()
    {
        float scaledDamage = Mathf.Round((Mathf.Sqrt(GetRelativeHp(this.level)) * (100 / scaleFactor)) + damageOffset);
        return scaledDamage;
    }

    public override float CalcOffsetDamage(int _level)
    {
        float scaledDamage = Mathf.Round((Mathf.Sqrt(GetRelativeHp(_level)) * (100 / scaleFactor)));
        Debug.Log("Scaled Damage: " + scaledDamage + "of the last weapon:" + this.name.ToUpper());
        return scaledDamage;
    }

    public override float GetScaledCriticalDmg()
    {
        if (buildController.MyBuildInstance == null)
        {
            return 0;

        }
        float scaledDamage = Mathf.Round((Mathf.Sqrt(GetRelativeHp(this.level)) * (100 / scaleFactor)) + damageOffset);
        //Debug.Log("SCALED DAMAGE ANTES: " + scaledDamage);
        scaledDamage *= 1.5f;
        //Debug.Log("CRITICAL DAMAGE DESPUES: " + scaledDamage);

        return scaledDamage;
    }
    private float RageBonus()
    {
        if(rage == true)
        {
            //Debug.Log("rage bonus");
            //cooldownTime = 0.1f;
            return 3;
        }
        else
        {
            return 1;
        }
    }

    private float StrenghtBonus()
    {
        //float damageAddition = StatController.MyInstance.strenght * 0.1f;
        return 0;
    }
    private float CriticalHit()
    {
        int chance = Random.Range(0, 100);
        if (chance < criticalChance)
        {
            FeedbackController.MyFeedbackInstance.FeedbackCritical();
            //Debug.Log("Critical!");
            return 1.5f;
        }
        else
        {
            return 1;
        }
    }

    private float SafeCritical()
    {
        //FeedbackController.MyFeedbackInstance.FeedbackCritical();
        return 1.5f;
    }

    private float Ira()
    {
        return Map(lastDamage, 0, PlayerStats.MyInstance.Salud.VidaM, 0, 100);
    }

    private float IraWaste()
    {
        if (skillController.MySkillInstance.GetIncreasing() == true)
        {
            return 0;
        }
        else
        {
            return 8.0f * Time.deltaTime;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void Tick(StatBar stat)
    {
        if(rage == false)
        {
            stat.Vidactual -= IraWaste();
        }
        
        actualStat = stat.Vidactual;

       

        // we make sure enough time has passed since the last time this enemy took damage
        if (rageCounter < 3 && rage == true)
        {
            //Debug.Log("Rage Counter:" + rageCounter);
            rageCounter += Time.deltaTime;
        }
        else
        {
            rage = false;
            
            gc.GetComponent<UIController>().RageSpace.SetActive(false);
        }

        if (stat.VidaM == stat.Vidactual)
        {
            Rage(stat);
        }

       
        if (TimeController.MyTimeInstance.freezeEventOn == true && coldCd > (cooldownTime / defuffScale))
        {
            coldCd -= Map(1.2f, 0, 10f, 0, 1.0f) * Time.deltaTime;
            //Debug.Log("Actual Cold Cd: " + coldCd);
        }
    }

    public void Rage(StatBar stat)
    {
        rage = true;
        stat.Vidactual = 0;
        rageCounter = 0f;
        gc.GetComponent<UIController>().RageSpace.SetActive(true);
    }

    public float GetDamage()
    {
        return baseDamage;
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        //vida escalable con vida actual
    }

    public override void LevelUp()
    {
        visualLevel++;
        level++;
        int newLevelBuffer = levelUpPrice + ((levelUpPrice - lastLevelUpPrice) + 25);
        lastLevelUpPrice = levelUpPrice;
        levelUpPrice = newLevelBuffer;
        CraftController.MyWorkshopInstance.RefreshRecipes();
    }

    public override void SetProgressionLevel(int levelToSet)
    {
        //set the level
        level = levelToSet;
        visualLevel = 1;

        Debug.Log("Adding the damage of the last weapon: " + CraftController.MyWorkshopInstance.craftHolder.forgeRecipes[this.id - 1]);
        if (CraftController.MyWorkshopInstance.craftHolder.forgeRecipes[this.id - 1] != null)
        {
            damageOffset = CraftController.MyWorkshopInstance.craftHolder.forgeRecipes[this.id - 1].skillResult.CalcOffsetDamage(levelToSet);
            Debug.Log("Getting a damage offset of: " + damageOffset);
        }

        GoldProgression(level);
    }

    private void GoldProgression(int _steps)
    {
        levelUpPrice = originalPrice;
        lastLevelUpPrice = 0;
        
        for (int i = 0; i < _steps; i++)
        {
            int newLevelBuffer = levelUpPrice + ((levelUpPrice - lastLevelUpPrice) + 25);
            lastLevelUpPrice = levelUpPrice;
            levelUpPrice = newLevelBuffer;
        }

        CraftController.MyWorkshopInstance.RefreshRecipes();
    }
    public override int GetLevelUpPrice()
    {
        //Debug.Log("LevelUp price is:" + levelUpPrice);
        return levelUpPrice;
    }

    public override int GetLastLevelUpPrice()
    {
        return lastLevelUpPrice;
    }

    public override void NewGame()
    {
        level = 1;
        visualLevel = 1;
        damageOffset = 0f;
        levelUpPrice = originalPrice;
        lastLevelUpPrice = 0;
    }

    public float GetRelativeHp(int level)
    {
        float startingPoint = 50f;
        float lastXp = 0f;
        float bufferMaxHp = 0f;

        if (level == 1)
        {
            bufferMaxHp = startingPoint;
            return bufferMaxHp;
        }

        //Debug.Log("Last Xp in iteration 1" + level + " : " + lastXp);
        //Debug.Log("Actual Xp in interation 1" + level + " : " + startingPoint);

        while (level > 0)
        {
            bufferMaxHp = Mathf.Round((startingPoint + ((startingPoint / 1.2f) - (lastXp / 1.2f) + 10)));
            //Debug.Log("Max hp reached:" + bufferMaxHp);
            lastXp = startingPoint;
            startingPoint = bufferMaxHp;

            level--;
        }
        return bufferMaxHp;
    }
}
