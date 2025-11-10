using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bioma : ScriptableObject
{
    private bool biomaActive = false;
    public bool BiomaActive { get => biomaActive; set => biomaActive = value; }

    public virtual void SetBioma(bool active) { }
    public virtual void StartBioma(bool isActive) { }


    public virtual void EndBioma(bool isActive) { }


    public virtual void Tick(bool updating, int step) {}

    //the pattern that the bioma will follow when steps go, like a boss fight or a puzzle
    [SerializeField]
    protected List<int> Pattern = new List<int>();

    public List<int> GetPattern
    {
        get { return Pattern; }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void StopBioma(bool isActive)
    {
        
    }

    public virtual bool HasDeco()
    {
        return false;
    }
    public string anticipationMessage;
}
