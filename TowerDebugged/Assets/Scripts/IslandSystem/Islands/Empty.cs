using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Empty : Island
{
    public Empty(string name, Vector3 position)
    {
        this.name = name;
        typeId = 99;
        type = Type.EMPTY;
        megaIsland = false;
        storage = 0f;
        buildPosition = position;
    }
    // Start is called before the first frame update
    public override float Recolect()
    {
        return storage;
    }
}
