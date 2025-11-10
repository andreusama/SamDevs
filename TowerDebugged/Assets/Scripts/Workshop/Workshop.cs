using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workshop 
{
    private string name;
    public List<Recipe> recipes;
    private int element;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public virtual void Craft() { }
    public virtual void Refine() { }

    //smth a workshop have to do
    // Update is called once per frame
    void Update()
    {
        
    }
}
