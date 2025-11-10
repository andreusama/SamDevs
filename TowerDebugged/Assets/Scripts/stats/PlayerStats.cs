using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
    private static PlayerStats instance;

    public static PlayerStats MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerStats>();
            }
            return instance;
        }


    }
    [SerializeField]
    public float SaludReg;
    [SerializeField]
    public float ManaReg;
    [SerializeField]
    public StatBar Salud;
    [SerializeField]
    public StatBar Debuff;
    [SerializeField]
    public StatBar StatBar1;
    [SerializeField]
    public StatBar StatBar2;
    [SerializeField]
    public StatBar StatBar3;
    [SerializeField]
    public StatBar XpBar;

    private void Awake()
    {
        //check if they are null, if they are don't initialize them
        if (Salud.Vbarra != null)
        {
            Salud.IniciarV();
        }
        if (Debuff.Vbarra != null)
        {
            Debuff.IniciarV();
        }
        if (StatBar1.Vbarra != null)
        {
            StatBar1.IniciarV();
        }
        if (StatBar2.Vbarra != null)
        {
            StatBar2.IniciarV();
        }
        if (StatBar3.Vbarra != null)
        {
            StatBar3.IniciarV();
        }
        if (XpBar.Vbarra != null)
        {
            XpBar.IniciarV();
        }
    }

    // Update is called once per frame
    void Update ()
    {
	 
    }
    
  
}
