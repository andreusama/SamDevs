using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Element : ScriptableObject
{
    public Sprite sprite;
    public new string name;
    public float dropChance;
    public int id;

    public enum ElementRareness
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
    public ElementRareness rareness;

    public enum Type
    {
        [InspectorName("consumible")]
        consumible = 0,
        [InspectorName("crafty")]
        crafty = 1,
        [InspectorName("especial")]
        especial = 2,
    }

    public Type type;
    public virtual void init(Sprite sprite)
    {
        
    }
    public virtual void Use() { }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
