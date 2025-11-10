using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class TowerHolder : MonoBehaviour
{
    [SerializeField]
    public Unit thisUnit;

    [SerializeField]
    public GameObject lifeBar;
    public MMFeedbacks DamageFeedback;
    public MMFeedbacks MagicFeedback;
    public MMFeedbacks HammerFeedback;
    public GoldFeedback GoldFeedback;
    private GameObject gc;

    private float windowTime = 0f;

    public bool startFreezed;

    private bool hitted;

    public Light colorLight;
    public bool Hitted { get => hitted; set => hitted = value; }

    public PassiveObject passiveObject;

    
    //1 for wood
    //2 for stone
    //3 for iron
    [SerializeField]
    private List<GameObject> visualApparition;

    public void SetVisualApparition(int index)
    {
        for (int i = 0; i < visualApparition.Count; i++)
        {
            if (i == index)
            {
                visualApparition[i].SetActive(true);
            }
            else
            {
                visualApparition[i].SetActive(false);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindWithTag("GameController");

        if (lifeBar != null)
        {
            lifeBar.GetComponent<LifeHolder>().InitializeBar();
            lifeBar.GetComponent<LifeHolder>().SetIdleBarActive();
            lifeBar.GetComponent<LifeHolder>().debuffGm.GetComponent<DebuffHolder>().InitializeMaterial();
            lifeBar.GetComponent<LifeHolder>().debuffGm.GetComponent<DebuffHolder>().InitializeDebuffBar();
            lifeBar.GetComponent<LifeHolder>().UpdateBar();
        }

        hitted = false;

        if (MagicFeedback != null)
        {
            MagicFeedback.Initialization();
        }
        if (DamageFeedback != null)
        {
            DamageFeedback.Initialization();
        }
        if (HammerFeedback != null)
        {
            HammerFeedback.Initialization();
        }


        if (thisUnit.GetState() != 1)
            StatController.MyInstance.AddGoldFromHitBag();

    }

    // Update is called once per frame
    void Update()
    {
        //here we'll have to determie if the bioma attack is active or not
        //in case it is activated we have to debuff the lifebar
        //the doubt here is, can we stop it?
        //you gotta check it every 0.15s because at 0.2 is the weapon and at 0.1 the passive, that means that the passive is the only way to surpass it
        if (thisUnit.state == 1 || buildController.MyBuildInstance.GetProgress() == buildController.Progress.FINISHED)
        {
            return;
        }
        
        if (TimeController.MyTimeInstance.GetPatternState() == TimeController.PatternState.DANGER)
        {
            //Debug.Log("Freezed... in tower with name: " + this.transform.name);
            windowTime += Time.deltaTime;

            if (windowTime >= 0.2f)
            {
                //Debug.Log("Debuffing");
                windowTime = 0f;

                //here we have to check if in this window a hitted was made and if it was the case don't do anything!
                if (hitted)
                {
                    hitted = false;
                    return;
                }
                lifeBar.GetComponent<LifeHolder>().Debuff(true);
            }
        }
        else
        {
            windowTime = 0f;
            //Debug.Log("Non Freezed... in tower with name: " + this.transform.name);
        }
    }

    public void UpdateBar()
    {
        if (lifeBar.GetComponent<LifeHolder>() != null)
        {
            lifeBar.GetComponent<LifeHolder>().UpdateBar();
        }
    }
    public void TakeDamage(int typeOfDamage, float damage)
    {
        //contractDamage = 0
        //magicDamage = 1
        //weaponDamage = 2

        //Debug.Log("Type Of Damage:" + typeOfDamage);

        if (typeOfDamage == 1)
        {
            MagicFeedback.PlayFeedbacks();
        }
        if (typeOfDamage == 2)
        {
            HammerFeedback.PlayFeedbacks();
        }
        // we make sure enough time has passed since the last time this enemy took damage
        //if (Time.time - _lastDamageTakenAt < DamageCooldown)
        //{
        //    return;
        //}
        //_lastDamageTakenAt = Time.time;

        DamageFeedback?.PlayFeedbacks(this.transform.position, damage);
    }

}
