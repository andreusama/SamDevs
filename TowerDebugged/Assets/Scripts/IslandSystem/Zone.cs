using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    private int id;

    private Vector3 startingPoint;
    private Vector3 finishingPoint;

    private int initialLevel;
    private int finalLevel;

    public Zone(int initialLevel, int finalLevel, Vector3 startingPoint, Vector3 finishingPoint, int id, bool active)
    {
        this.InitialLevel = initialLevel;
        this.finalLevel = finalLevel;

        this.startingPoint = startingPoint;
        this.finishingPoint = finishingPoint;
        
        this.id = id;

    }

    public int InitialLevel { get => initialLevel; set => initialLevel = value; }
    public int FinalLevel { get => finalLevel; set => finalLevel = value; }
    public Vector3 StartingPoint { get => startingPoint; set => startingPoint = value; }
    public Vector3 FinishingPoint { get => finishingPoint; set => finishingPoint = value; }

    
}
