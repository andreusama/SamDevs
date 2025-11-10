using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatBar 
{
    [SerializeField]
    public VidaBarra Vbarra;
    [SerializeField]
    private float vidactual;
    [SerializeField]
    private float vidaM;

    void Start()
    {
        
    }

    
    public float Vidactual
    {
        get
        {
            return vidactual;
        }

        set
        {

            this.vidactual = Mathf.Clamp(value, 0, VidaM);
            //UPDATE BAR
            Vbarra.Value = vidactual;
            //Update Tower bar!
            if (buildController.MyBuildInstance != null && buildController.MyBuildInstance.actualTower != null)
                buildController.MyBuildInstance.actualTower.GetComponent<TowerHolder>().UpdateBar();

        }
    }

    public float VidaM
    {
        get
        {
            return vidaM;
        }

        set
        {

            this.vidaM = value;
            Vbarra.vidamax = vidaM;
        }
    }
    public void IniciarV()
    {
        //Debug.Log("Setting up the variables of the bar;");
        this.VidaM = vidaM;
        this.Vidactual = vidactual;
    }
}
