using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class buildableTile : Island
{
    public buildableTile(string name, float newPrice, Vector3 position)
    {
        this.name = name;
        typeId = -1;
        type = Island.Type.BUILDABLE;
        megaIsland = false;
        Price = newPrice;
        buildPosition = position;
    }

    public override void MegaIsland()
    {
        Debug.Log("PARKOUR");
    }
    // Start is called before the first frame update
}
