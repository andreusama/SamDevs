using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ink : Element
{
    public string stringType;
    public Ink(string name, string type, Sprite sprite)
    {
        this.name = name;
        this.stringType = type;
        this.sprite = sprite;
        this.dropChance = 0.50f;
    }

    public override void init(Sprite sprite)
    {
        this.name = "Ink";
        this.sprite = sprite;
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
