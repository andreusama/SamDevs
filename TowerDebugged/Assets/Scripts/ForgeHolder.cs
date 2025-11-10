using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForgeHolder : MonoBehaviour
{
    
    private Workshop forge;
    public TextMeshProUGUI name;
    public Image bgDeco;
    public Color color;
    public Image icon;

    public Sprite refineRequirement;

    [SerializeField]
    private List<Recipe> initialRecipes;

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

    public List<Recipe> GetRecipes()
    {
        return this.InitialRecipes;
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

    private GameObject InitializePrefab(GameObject recipePrefab, Recipe recipe)
    {
        //gameobject.name = recipe.name;
        //gameobject.resultSprite = recipe.icon
        /*
        for (int i = 0; i < recipe.elementsInRecipe.Count; i++)
        {
            Instantiate(InitializeRequiredElements(recipe.elementsInRecipe[i].position.parent, quat));
            //add a + text;
        }
        */
        //gameobject.requiredElements[1] = reciper.requiredElements[1]
        return recipePrefab;
    }

    private GameObject InitializeRequiredElements(GameObject requiredElement)
    {



        return requiredElement;
    }

    private void AddObjectFurniture(GameObject createdObject)
    {
        //Instantiate...;
        //gameobject.name = recipe.name;
        //gameobject.resultSprite = recipe.icon;
        //gameobject.requiredElements[1] = reciper.requiredElements[1];
    }


    

}
