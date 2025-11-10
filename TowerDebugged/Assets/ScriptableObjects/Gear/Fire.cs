using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Active", menuName = "Gear/Active/Fire")]
public class Fire : Active
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SummonActive(float intensity)
    {
        //Debug.Log("Circular ticking");
        if (buildController.MyBuildInstance.actualTower != null && PlayerStats.MyInstance.Debuff.Vidactual > 0)
        {
            float damage = CalcHeat(intensity);
            //buildController.MyBuildInstance.actualTower.GetComponent<TowerHolder>().passiveObject.SetIntensity(CalcIntensity(velocity));
            PlayerStats.MyInstance.Debuff.Vidactual -= damage;

            //Debug.Log("Applying fire!");
            if (buildController.MyBuildInstance.actualTower != null)
            {
                float visualDamage = Mathf.Round(damage);
                buildController.MyBuildInstance.SetFeedbackIntensity(visualDamage - 5, visualDamage);
                buildController.MyBuildInstance.DamageFeedback(0, visualDamage);
            }
        }
        buildController.MyBuildInstance.actualTower.GetComponent<TowerHolder>().Hitted = true;
    }

    private float CalcHeat(float intensity)
    {

        //we will get rid of the ice if we circle quick!
        return (((PlayerStats.MyInstance.Debuff.VidaM / 10) * (StatController.Map(intensity, 0f, 1f, 0f, 1.5f))));
    }

    public override float CalcIntensity(float velocity, float minVel, float maxVel)
    {
        float intensity = Mathf.Clamp(velocity, minVel, maxVel);

        intensity = StatController.Map(intensity, minVel, maxVel, 0f, 1f);

        //Debug.Log("Intensity:" + intensity);

        return intensity;
    }
}
