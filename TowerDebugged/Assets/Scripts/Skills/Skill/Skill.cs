using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Skill : ScriptableObject 
{
    public new string name;
    public float cooldownTime;
    public int id;
    public string type;
    public Sprite sprite;
    public float stat;
    public float actualStat;
    public bool isBuff;
    public Buff buff;
    public bool broke;
    public AudioClip fx;
    public float goldHit;
    public int quantity;
    public StatBar internalBar;

    //bioma debuff
    public float coldCd;
    public int level;
    public int visualLevel = 1;

    [SerializeField]
    protected int levelUpPrice = 0;
    [SerializeField]
    protected int lastLevelUpPrice = 0;
    //private float criticalBonus;

    public enum SkillRareness
    {
        [InspectorName("Common")]
        grey = 0,
        [InspectorName("Unusual")]
        green = 1,
        [InspectorName("Rare")]
        blue = 2,
        [InspectorName("Epic")]
        purple = 3,
        [InspectorName("Legendary")]
        ambar = 4
    }
    [SerializeField]
    public SkillRareness rareness;

    public virtual void SummonSpell(StatBar stat = null) {}

    public virtual void Init() {}

    public virtual void Refresh() {}

    public virtual void Description(TextMeshProUGUI textObject) {}
    public virtual void StaticDescription(TextMeshProUGUI textObject) {}
    public virtual void GoldHit() {}
    public virtual void GoldHits(int hits) {}

    public virtual void LevelUp() {}
    public virtual void SetProgressionLevel(int levelToSet) {}

    public virtual void SetBar() {
        internalBar.VidaM = this.stat;
        internalBar.Vidactual = 0;
        //internalBar.Vidactual = this.actualStat;
    }
    
    public virtual Color GetColorByRareness()
    {
        { return Color.white; }
    }



    public virtual void SetBiomaDebuffs(bool isActive) {}

    public virtual float GetScaledBaseDmg() { return 0; }
    public virtual float GetScaledCriticalDmg() { return 0; }

    public virtual int GetLevelUpPrice() { return 0; }
    public virtual int GetLastLevelUpPrice() { return 0; }
    public virtual AudioClip GetFx() { return fx; }

    public virtual float Map(float value, float inMin, float inMax, float outMin, float outMax) { return 0; }
    public virtual void CleanUp() {}

    public virtual void NewGame() { }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Tick(StatBar stat) {}

    public virtual float CalcOffsetDamage(int _level){return 0f;}
}
