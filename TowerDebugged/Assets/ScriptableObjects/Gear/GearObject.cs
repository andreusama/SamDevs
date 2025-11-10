using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gear", menuName = "Gear")]
public class GearObject : ScriptableObject
{
    public Skill equipedSkill;
    public Contract contract;
    public Spell signature;
    public Active active;

    private float DGT;

    public void SetDGT(float newDGT)
    {
        DGT = newDGT;
    }
    public float GetDGT()
    {
        return DGT;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

}
