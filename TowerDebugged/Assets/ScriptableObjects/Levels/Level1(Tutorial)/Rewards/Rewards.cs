using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rewards", menuName = "Level/Rewards")]
public class Rewards : ScriptableObject 
{
    [SerializeField]
    private float amount;

    //getter of extraCoins
    public float GetAmount
    {
        get { return amount; }
        set { amount = value; }
    }

    [SerializeField]
    private Sprite sprite;

    public Sprite GetSprite
    {
        get { return sprite; }
    }

    public virtual void ApplyReward()
    {
        //Debug.Log("Applying reward");
    }
}
