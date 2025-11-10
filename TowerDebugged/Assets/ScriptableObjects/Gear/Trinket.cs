using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Active", menuName = "Gear/Active/Trinket")]
public class Trinket : Active
{
    public override void SummonActive(float intensity)
    {
        //Debug.Log("Circular ticking");
        
    }

    private float CalcHeat(float intensity)
    {

        return 0f;
    }

    public override float CalcIntensity(float velocity, float minVel, float maxVel)
    {
        return 0f;
    }
}
