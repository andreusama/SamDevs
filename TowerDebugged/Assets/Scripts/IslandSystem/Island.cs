using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/*-----------------------
    Forest id = 0
    Sea id = 1
    Volcano id = 2
    Empty id = 99
    -------------------------- */


[Serializable]
public class Island
{
    public string name;
    public int typeId;
    public int id;
    public float productionTime;
    
    public int islandLevel = 0;

    public bool megaIsland = false;

    public bool refresh = false;

    public float price = 0f;

    private bool build = false;

    private bool updatePrice = false;

    private bool meshActive = true;

    //Resources Warehouse
    private float resources = 0f;
    public bool resourcesCollected = false;

    public float storage = 0f;
    public bool newStorage = false;

    public Vector3Data buildPosition = Vector3.zero;

    private MeshState meshState = MeshState.idle;
    public enum MeshState
    {
        toFalse,
        toTrue,
        idle
    }

    public Type type;
    public enum Type
    {
        FOREST,
        SEA,
        VOLCANO,
        BUILDABLE,
        EMPTY = 99
    }

    public virtual float Price
    {
        get { return price; }
        set
        {
            price = value;
            updatePrice = true;
        } 
    }

    public virtual bool Build { get { return build; } set { build = value; } }

    public virtual bool UpdatePrice { get { return updatePrice; } set { updatePrice = value; } }

    public virtual float Recolect() 
    {
        resourcesCollected = true;

        return storage; 
    }

    public virtual float Storage { get { return storage; } set { storage = value; newStorage = true; } }


    public virtual MeshState GetMeshState { get { return meshState; } set { meshState = value; } }


    public virtual void RefreshBuyable() { refresh = true; }

    public virtual void MegaIsland() { }

    public virtual float SetResources { set { resources = value; } }
}



