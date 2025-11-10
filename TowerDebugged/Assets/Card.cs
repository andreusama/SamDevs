using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Feel;
using MoreMountains.Tools;

[CreateAssetMenu]
public class Card : ScriptableObject
{
    [SerializeField]
    private string cardName;
    public enum CardRareness
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
    public CardRareness rareness;

    [SerializeField]
    public List<Stat> stats;

    public Sprite sprite;

    [SerializeField]
    private string quote;

    private bool isKing;

    public float damage;

    [SerializeField]
    private string definition;

    public bool GetIsKing()
    {
        return isKing;
    }

    public void SetIsKing(bool setter)
    {
        isKing = setter;
    }

    public string GetName()
    {
        return cardName;
    }

    public string GetQuote()
    {
        return quote;
    }

    public void Crown()
    {
        
        isKing = true;
    }

    public void Decrown()
    {
        isKing = false;
    }

    public string GetDefinition()
    {
        return definition;
    }
}
