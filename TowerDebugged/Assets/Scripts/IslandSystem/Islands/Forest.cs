using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Forest : Island
{
    public Forest(string name, Vector3 position)
    {
        this.name = name;
        typeId = 0;
        type = Type.FOREST;
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
