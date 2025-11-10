using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CraftHolder : ScriptableObject
{
    public List<List<Recipe>> generalList = new List<List<Recipe>>();
    //create a list of recipes
    public List<Recipe> forgeRecipes = new List<Recipe>();
    public List<Recipe> magicRecipes = new List<Recipe>();
    public List<Recipe> contractRecipes = new List<Recipe>();


    //create 3 namespaces for the 3 different types of recipes

    public void Initialize()
    {
        //add the recipes to the list
        generalList.Add(forgeRecipes);
        generalList.Add(magicRecipes);
        generalList.Add(contractRecipes);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


