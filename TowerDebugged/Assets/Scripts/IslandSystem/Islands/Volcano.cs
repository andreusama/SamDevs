using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Volcano : Island
{
    public Volcano(string name, Vector3 position)
    {
        this.name = name;
        typeId = 2;
        type = Type.VOLCANO;
        megaIsland = false;
        storage = 0f;
        buildPosition = position;
    }

    public override void MegaIsland()
    {
        Debug.Log("PARKOUR");
    }
    // Start is called before the first frame update
    public override float Recolect()
    {
        return storage;
    }
    
}
