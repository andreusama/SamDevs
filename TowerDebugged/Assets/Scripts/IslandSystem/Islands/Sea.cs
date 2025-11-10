using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Sea : Island
{
    public Sea(string name, Vector3 position)
    {
        this.name = name;
        type = Type.SEA;
        typeId = 1;
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
