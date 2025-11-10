using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class refineryRecipe : ScriptableObject
{
    public Element result;
    public int elementAmount;
    public string name;
    public float productionTime;

    public Sprite requirement;
    public int elementId;

    //forest = 0
    //river = 1
    //volcano = 2

    public virtual void Refine() { }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
