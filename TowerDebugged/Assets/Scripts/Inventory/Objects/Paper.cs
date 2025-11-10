using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Paper : Element
{
    string stringType;
    public Paper(string name, string type, Sprite sprite)
    {
        this.name = name;
        this.stringType = type;
        this.sprite = sprite;
        this.dropChance = 0.50f;
    }

    public override void init(Sprite sprite)
    {
        this.name = "Paper";
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
