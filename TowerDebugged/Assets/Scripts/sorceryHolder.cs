using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sorceryHolder : MonoBehaviour
{
    private Workshop sorcery;
    public TextMeshProUGUI name;
    public Image bgDeco;
    public Color color;
    public Image icon;

    public Sprite refineRequirement;

    [SerializeField]
    private List<Recipe> initialRecipes;
    [SerializeField]
    private List<refineryRecipe> initialRefineRecipes;

    public List<Recipe> InitialRecipes
    {
        get
        {
            return initialRecipes;
        }
        set
        {
            initialRecipes = value;
        }
    }

    public List<refineryRecipe> InitialRefineRecipes
    {
        get
        {
            return initialRefineRecipes;
        }
        set
        {
            initialRefineRecipes = value;
        }
    }

    public List<refineryRecipe> GetRefineryRecipes()
    {
        if (initialRefineRecipes.Count > 0)
        {
            return this.InitialRefineRecipes;
        }
        return null;
    }
    public List<Recipe> GetRecipes()
    {
        if (initialRecipes.Count > 0)
        {
            return this.InitialRecipes;
        }
        return null;
    }

    public Recipe GetRecipe(int index)
    {
        return this.InitialRecipes[index];
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddRecipe(Recipe recipe)
    {
        //Instantiate(InitializePrefab(recipePrefab, recipe), position.parent, quat);
    }

}
