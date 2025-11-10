using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UIElements.Experimental;
using System;

public class skillController : MonoBehaviour
{
    //Physics.autoSimulation = false;
    public int signTries = 0;
    private float repairValue;
    
    private GameObject gc;
    [Header("Skill Images")]
    public List<Sprite> sprites;

    [Header("Buttons")]
    [SerializeField]
    private GameObject buttonLeft;
    [SerializeField]
    private GameObject buttonMiddle;
    [SerializeField]
    private GameObject buttonRight;
    [SerializeField]
    private GameObject selectedDurabilitySpot;

    public StatBar selectedStat;



    [SerializeField]
    private GameObject statSpot0;
    [SerializeField]
    private GameObject statSpot1;
    [SerializeField]
    private GameObject statSpot3;
    [SerializeField]
    private GameObject statBarPrefab;

    public bool statIncreasing = false;
    public bool statDecreasing = false;

    [Header("Cooldowns Text")]
    [SerializeField]
    public TextMeshProUGUI statText;

    public bool draggingSign = false;

    [Header("Spell Zone")]
    [SerializeField]
    private GameObject spellZone;
    public GameObject spellInteractable;
    public GameObject spellGeneralSpace;
    private bool pressed = false;
    private bool exit = false;
    public bool GetPressed() { return pressed; }
    public int ticksCompleted;
    private List<spellInteracter> tickArray = new List<spellInteracter>(0);
    private spellInteracter actualTick;

    [Header("Buff List")]
    [SerializeField]
    private List<GameObject> buffList;
    public GameObject buffPrefab;
    public GameObject buffPivot;
    public List<GameObject> GetBuffList() { return buffList; }

    public StatBar GetSelectedStat() { return selectedStat; }

    public bool castCancelled = false;
    public bool casting = false;
    private int actualSkillId { get; set; }
    public int GetSkillId { get { return actualSkillId; } set { actualSkillId = value; } }
    private int actualButtonId;

    [Header("Contract Names")]
    public List<string> contractNames;

    public MMFeedbacks DamageFeedback;
    public bool resting;

    //Awake Start Declarations
    public skillButton skillButtonScript;

    public ActiveButton activeScript;

    //make a Skill named skillToEquip and create a getter and a sette
    
    public skillButton GetSkillButtonScript()
    {
        if (skillButtonScript != null)
        {
            return skillButtonScript;
        }

        return null;
    }

    buildController buildScript;
    UIController uiScript;

    private static skillController skillInstance;

    public static skillController MySkillInstance
    {
        get
        {
            if (skillInstance == null)
            {
                skillInstance = FindObjectOfType<skillController>();
            }
            return skillInstance;
        }


    }

    void Start()
    {
        signTries = 0;
        gc = GameObject.FindWithTag("GameController");
        draggingSign = false;
        SetActualButton(buttonMiddle.GetComponent<skillButton>());
        buildScript = gc.GetComponent<buildController>();
        uiScript = gc.GetComponent<UIController>();

        if (LevelTraveler.MyTravelInstance != null)
        {
            if (!LevelTraveler.MyTravelInstance.Level.isTutorial)
            {
                StartCoroutine(ContractCoroutine(LevelTraveler.MyTravelInstance.Gear.contract));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (skillButtonScript != null && skillButtonScript.actualSkill != null)
        {
            skillButtonScript.actualSkill.Tick(selectedStat);
        }

        //CREATE A DEBUG PRESSING KEY 1
    }

    public void SetActualButton(skillButton selectedScript)
    {
        Debug.Log("Script ID: " + selectedScript.id);
        skillButtonScript = selectedScript;

        switch (selectedScript.id)
        {
            case 0:
                selectedDurabilitySpot = statSpot0;
                selectedStat = PlayerStats.MyInstance.StatBar1;
                break;
            case 1:
                selectedDurabilitySpot = statSpot1;
                selectedStat = PlayerStats.MyInstance.StatBar2;
                break;
            case 2:
                selectedDurabilitySpot = statSpot3;
                selectedStat = PlayerStats.MyInstance.StatBar3;
                break;
            default:
                break;
        }
        
    }

    

    public float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        //vida escalable con vida actual
    }

    public void CreateBuff(Buff buff, Sprite sprite, int ticks, float time)
    {
        GameObject newFeedback = Instantiate(buffPrefab, Vector3.zero, Quaternion.identity);
        newFeedback.gameObject.GetComponent<buffHolder>().image.sprite = sprite;
        newFeedback.gameObject.transform.SetParent(buffPivot.transform);
        newFeedback.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        newFeedback.gameObject.GetComponent<buffHolder>().descriptionText.text = buff.description;
        buffList.Add(newFeedback);

        StartCoroutine(BuffLifeTime(newFeedback, ticks, time));
    }
    public void DestroyBuff(GameObject buffHolder)
    {
        buffList.Remove(buffHolder);
        Destroy(buffHolder);
    }


    public void ContractSummon(Contract contract)
    {
        StartCoroutine(ContractCoroutine(contract));
    }
    //SKILLS
    IEnumerator ContractCoroutine(Contract contract)
    {
        if (contract.isBuff == true && contract.buff != null)
            CreateBuff(contract.buff, contract.sprite, contract.GetNumberTicks(), contract.GetTimeBtwTicks());
        
        float debug = 0;
        int i = 0;
        contract.SetBaseDamage(contract.GetBaseDamage());
        castCancelled = false;
        while(i < contract.GetNumberTicks())
        {
            if(resting)
            {
                yield return new WaitForSeconds(contract.GetTimeBtwTicks());
            }
            else
            {
                debug += contract.GetTimeBtwTicks();
                //Debug.Log("BUFF TIME" + debug);

                if (castCancelled == true)
                {
                    StopCoroutine("BuffLifeTime");
                    foreach (var item in buffList)
                    {
                        Destroy(item);
                    }
                    buffList.Clear();
                    //DestroyBuff(buff);
                    Debug.Log("Cancelled");
                    castCancelled = false;
                    yield break;
                }

                //Debug.Log("damage" + contract.GetBaseDamage());
                if (buildScript.actualTower != null && PlayerStats.MyInstance.Debuff.Vidactual > 0)
                {
                    PlayerStats.MyInstance.Debuff.Vidactual -= contract.GetBaseDamage();
                }
                else if(buildScript.actualTower != null)
                {
                    buildController.MyBuildInstance.SumLife(contract.GetBaseDamage());
                }
                //buildController.MyBuildInstance.actualTower.GetComponent<TowerHolder>().Hitted = true;

                if (buildScript.actualTower != null)
                {
                    gc.GetComponent<buildController>().SwitchFloatTextDir(700, 250);
                    contract.GoldHit();
                    float visualDamage = Mathf.Round(contract.GetBaseDamage());
                    buildScript.SetFeedbackIntensity(contract.GetBaseDamage(), (contract.GetBaseDamage() + 5f));
                    buildScript.DamageFeedback(0, visualDamage);
                }
                
                i++;
                yield return new WaitForSeconds(contract.GetTimeBtwTicks());
            }
        }
    }


    public void SpellSummon(Spell spellSummoned)
    {
        castCancelled = true;
        casting = true;
        uiScript.InitializeSpell(spellSummoned.GetNumberTicks());

        TickSpell(0, spellSummoned);
        //StopCoroutine("SpellCoroutine");
        //StartCoroutine(SpellCoroutine(spellSummoned));
    }

    public void TickSpell(int tickIndex, Spell actualSpell)
    {
        RectTransform rt = (RectTransform)spellZone.transform;
        Vector3 vect = UnityEngine.Random.insideUnitCircle * rt.rect.width;
        Debug.Log("The new vector BEFORE the checkings is placed at the quadrant: " + GetQuadrant(vect));

        if (tickArray.Count > 0)
        {
            if (GetQuadrant(tickArray[tickArray.Count - 1].transform.localPosition) == GetQuadrant(vect))
            {
                Debug.Log("The quadrant was the same so we onw to change it!");
                Debug.Log("The LAST vector was at the quadrant: " + GetQuadrant(tickArray[tickArray.Count - 1].transform.localPosition));
                Debug.Log("The ACTUAL vector is at the quadrant: " + GetQuadrant(vect));
                int newQuadrant = GetQuadrant(vect);
                Vector2 vector = GetVectorInQuadrant(GetRandomElement(GetOtherQuadrants(newQuadrant))) * rt.rect.width;
                vect = vector;
            }
        }
        //INSTANTIATE THE TICK
        Debug.Log("The new vector AFTER the checkings is placed at the quadrant: " + GetQuadrant(vect));

        GameObject dot = InstantiateInteracter(spellZone.transform, new Vector3(2f, 2f, 2f));
        dot.gameObject.transform.localPosition = vect;

        //THIS TICK WILL COUNT IN AN INDEPENDENT WAY

        //SET TO THE ACTUAL TICK IN ORDER TO CHECK OR DEBUG THINGS
        actualTick = dot.transform.GetComponent<spellInteracter>();
        actualTick.SetSpell(actualSpell);
        actualTick.id = tickIndex;
        actualTick.Countdown();
        tickArray.Add(actualTick);
    }

    public void SetPressed(spellInteracter _sucessInteracter)
    {
        uiScript.SpellProgressCheck(_sucessInteracter.id);

        //Debug.Log("Entering Set Pressed with success interacter ID: " + _sucessInteracter.id);
        //Debug.Log("Entering Set Pressed with the AIM OF: " + _sucessInteracter.GetSpell().GetNumberTicks());
        //here we gotta check success
        if (_sucessInteracter.GetSpell().GetNumberTicks() == _sucessInteracter.id + 1)
        {
            //Debug.Log("SUCCESS");
            SuccessSummon(_sucessInteracter.GetSpell(), _sucessInteracter.id);
            return;
        }
        
        //Debug.Log("Entering Tick Spell" + _sucessInteracter.id);
        TickSpell(_sucessInteracter.id + 1, _sucessInteracter.GetSpell());
    }

    //create a function that given a Vector2 Random.insideUnitCircle returns you the quadrant this vector is in
    public int GetQuadrant(Vector2 vector)
    {
        if (vector.x > 0 && vector.y > 0)
        {
            return 1;
        }
        else if (vector.x < 0 && vector.y > 0)
        {
            return 2;
        }
        else if (vector.x < 0 && vector.y < 0)
        {
            return 3;
        }
        else if (vector.x > 0 && vector.y < 0)
        {
            return 4;
        }
        else
            return 0;
    }

    //create a function that returns a vector2 inside of a quadrant
    public Vector2 GetVectorInQuadrant(int quadrant)
    {
        Vector2 vector = Vector2.zero;
        switch (quadrant)
        {
            case 1:
                vector = UnityEngine.Random.insideUnitCircle;
                break;
            case 2:
                vector = UnityEngine.Random.insideUnitCircle;
                vector.x *= -1;
                break;
            case 3:
                vector = UnityEngine.Random.insideUnitCircle;
                vector.x *= -1;
                vector.y *= -1;
                break;
            case 4:
                vector = UnityEngine.Random.insideUnitCircle;
                vector.y *= -1;
                break;
            default:
                break;
        }
        return vector;
    }

    public List<int> GetOtherQuadrants(int quadrant)
    {
        List<int> quadrants = new List<int>();
        switch (quadrant)
        {
            case 1:
                quadrants.Add(2);
                quadrants.Add(3);
                quadrants.Add(4);
                break;
            case 2:
                quadrants.Add(1);
                quadrants.Add(3);
                quadrants.Add(4);
                break;
            case 3:
                quadrants.Add(1);
                quadrants.Add(2);
                quadrants.Add(4);
                break;
            case 4:
                quadrants.Add(1);
                quadrants.Add(2);
                quadrants.Add(3);
                break;
            default:
                break;
        }
        return quadrants;
    }

    //create a function that randomly selects a element from a list
    public int GetRandomElement(List<int> list)
    {
        int index = UnityEngine.Random.Range(0, list.Count);
        return list[index];
    }

    public GameObject InstantiateInteracter(Transform parent, Vector3 scale, bool clamp = false)
    {
        GameObject dot = Instantiate(spellInteractable, Vector3.zero, Quaternion.identity);
        dot.gameObject.transform.SetParent(parent);
        dot.gameObject.transform.localScale = scale;

        if (clamp)
        {
            UIController.SetAnchoredPos(UIController.AnchorPresets.MiddleCenter, (RectTransform)dot.transform, (RectTransform)parent);
        }
        return dot;
    }
    public void SetExit(bool setExit)
    {
        exit = setExit;
    }
    public void SuccessSummon(Spell spellSummoned, int ticksCompleted)
    {
        uiScript.CleanUpSpell();

        
        spellSummoned.GoldHits(ticksCompleted);

        
        foreach (Transform child in spellZone.transform)
        {
            Destroy(child.gameObject);
        }

        pressed = false;

        buildController.MyBuildInstance.SucessEnd();
    }
    public void FailedSummon(Spell spellSummoned = null, int ticksCompleted = 0)
    {
        //casting setted to false here in order to avoid second simultaneous FailedSummon;
        casting = false;
        
        if (signTries < 2)
        {
            ResetSummon();
            UIController.MyUiInstance.signCrosses[signTries].gameObject.SetActive(true);
            signTries++;
            return;
        }
        uiScript.CleanUpSpell();

        FeedbackController.MyFeedbackInstance.FeedbackSpellPoint(true);
        foreach (Transform child in spellZone.transform)
        {
            Destroy(child.gameObject);
        }
        pressed = false;

        buildController.MyBuildInstance.myState = buildController.AliveState.DEADBYSIGN;
        buildController.MyBuildInstance.FailureEnd();
    }

    public void ResetSummon()
    {
        tickArray.Clear();
        
        foreach (Transform child in UIController.MyUiInstance.ProgressLayout.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        
        foreach (Transform child in spellZone.transform)
        {
            Destroy(child.gameObject);
        }

        FeedbackController.MyFeedbackInstance.signFailFeedbacks.PlayFeedbacks();
    }

    IEnumerator BuffLifeTime(GameObject buff, int ticks, float time)
    {
        int i = 0;
        while (i < ticks)
        {
            if(resting == true)
            {
                buff.gameObject.GetComponent<buffHolder>().freezed.SetActive(true);
                yield return new WaitForSeconds(time);
            }
            else
            {
                buff.gameObject.GetComponent<buffHolder>().freezed.SetActive(false);
                i++;
                yield return new WaitForSeconds(time);
            }
        }

        DestroyBuff(buff);
        yield break;
    }

    public void DestroyGameObject()
    {
        selectedStat.Vidactual = 0;
        skillButtonScript.holder.cooldown = 0;
        skillButtonScript.holder.internalSkill.actualStat = 0;
        InventoryController.MyInventory.DestroySkill(skillButtonScript.holder.transform.gameObject);
    }

    public void SetIncreasing(bool set)
    {
        statIncreasing = set;
    }

    public bool GetIncreasing()
    {
        return statIncreasing;
    }

}

    