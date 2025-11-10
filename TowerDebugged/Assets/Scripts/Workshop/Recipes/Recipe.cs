using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Recipe : ScriptableObject
{
    private int price = 0;
    public Element result;
    public Skill skillResult;
    public string name;
    public float productionTime;
    public bool locked;
    
    public virtual void Craft() { }

    public virtual int GetPrice() { return price; }
    public virtual void SetPrice(int newPrice) { price = newPrice; }

    public virtual void Unlock(bool _newlocked) 
    { 
        locked = _newlocked;
    }

    public virtual void SetProgressionLevel(int initialLevel)
    {
        skillResult.SetProgressionLevel(initialLevel);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void NewGame()
    {
        if (skillResult.id != 0)
        {
            locked = true;
        }
        skillResult.NewGame();
    }
}
