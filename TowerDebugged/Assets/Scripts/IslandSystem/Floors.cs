using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


//[Serializable]
//public class BuildData
//{
//    public int buildStage = 0;
//}
[Serializable]
public class Floors
{
    public Unit towerUnitData;
    public float productionTime;
    public bool claimed;
    public bool full;
    public bool isTown;
    public List<Island> islands = new List<Island>(6);
    public Vector3Data buildingPoint;
    public Vector3Data checkPoint;
    public int floorLevel;
    public int level;
    public string townName;
    public bool edificable;
    //public BuildData buildData = new BuildData();

    public Floors(int level, List<Island> islands, Vector3 buildingPoint, Vector3 checkPoint, bool isfull, string name, bool edificable)
    {
        this.checkPoint = checkPoint;
        this.islands = islands;
        this.buildingPoint = buildingPoint;

        if (edificable == true)
        {
            this.floorLevel = level;
        }
        else
        {
            this.floorLevel = -1;
        }

        this.level = level;
        this.claimed = false;
        this.full = isfull;
        this.isTown = false;
        this.edificable = edificable;
        townName = name;
        //buildData.buildStage = 1;
        for (int i = 0; i < islands.Count; i++)
        {
            islands[i].id = i;
            //OJO, FLOOR LEVEL ES EL NIVEL RELATIVO DE LOS SUELOS NO EL DE LA TORRE
            islands[i].islandLevel = floorLevel;
        }
    }

    public Floors(int level, Vector3 buildingPoint, Vector3 checkPoint, string name, bool edificable)
    {
        this.checkPoint = checkPoint;
        this.buildingPoint = buildingPoint;

        if (edificable == true)
        {
            this.floorLevel = level;
        }
        else
        {
            this.floorLevel = -1;
        }

        this.level = level;
        this.claimed = false;
        this.full = false;
        this.isTown = false;
        this.edificable = edificable;
        townName = name;

        Debug.Log("Constructed");
    }

    public void Save(BinaryFormatter formatter, FileStream stream)
    {
        formatter.Serialize(stream, this);
    }

    public void UpdateFloorData(Unit newUnitData, int buildStage)
    {
        towerUnitData = newUnitData;
        //buildData.buildStage = buildStage;
    }
    /*-----------------------
    Forest id = 0
    Sea id = 1
    Volcano id = 2
    -------------------------- */
}
