using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Rock : Element
{
    string stringType;
    public Rock(string name, string type, Sprite sprite)
    {
        this.name = name;
        this.stringType = type;
        this.sprite = sprite;
        this.dropChance = 0.75f;
    }

    public override void init(Sprite sprite)
    {
        this.name = "Rock";
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
