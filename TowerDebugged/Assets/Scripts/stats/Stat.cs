using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField]
    public float amount;

    public enum Type
    {
        [InspectorName("STRENGHT")]
        Strenght = 0,
        [InspectorName("INTELLECT")]
        Intellect = 1,
        [InspectorName("ELOQUENCE")]
        Eloquence = 2,
        [InspectorName("CRITICAL HIT")]
        CriticalHit = 3,
    }

    [SerializeField]
    public Type type;

    void Start()
    {
        
    }
}

