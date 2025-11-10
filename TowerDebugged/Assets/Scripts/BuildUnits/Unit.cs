using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public struct Vector3Data
{
    public float x;
    public float y;
    public float z;

    public Vector3Data(float rx, float ry, float rz)
    {
        x = rx;
        y = ry;
        z = rz;
    }

    public static implicit operator Vector3(Vector3Data rValue)
    {
        return new Vector3(rValue.x, rValue.y, rValue.z);
    }

    public static implicit operator Vector3Data(Vector3 rValue)
    {
        return new Vector3Data(rValue.x, rValue.y, rValue.z);
    }
}
[System.Serializable]
public class Unit 
{
    //STATES
    //0 - FRAGMENT
    //1 - FULL
    public Vector3Data position;
    public int state;
    public Unit(Vector3 position, int state)
    {
        this.position = position;
        this.state = state;
    }

    public int GetState()
    {
        return state;
    }
    
}
