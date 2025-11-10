using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebuffHolder : MonoBehaviour
{
    public SpriteRenderer lifeImage;
    private Material barMaterial;
    public TextMeshPro text;

    public Color idleColor;
    public Color freezedColor;

    public GameObject freezeDeco;
    // Start is called before the first frame update

    private void Start()
    {
    }
    public Material GetMaterial()
    {
        return barMaterial;
    }

    public void SetColor(Color newColor)
    {
        if (barMaterial == null)
        {
            barMaterial = lifeImage.material;
        }
        barMaterial.color = newColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDebuffBar()
    {
        float actualHp = PlayerStats.MyInstance.Debuff.Vidactual;
        float maxHp = PlayerStats.MyInstance.Debuff.VidaM;
        barMaterial.SetFloat("_Fill", Map(actualHp, 0, maxHp, 0, 1));
        text.text = StatController.Aproximation(actualHp) + " / " + StatController.Aproximation(maxHp);
    }

    public void InitializeMaterial()
    {
        barMaterial = lifeImage.material;
    }
    public void InitializeDebuffBar()
    {
        barMaterial = lifeImage.material;
        PlayerStats.MyInstance.Debuff.VidaM = PlayerStats.MyInstance.Salud.VidaM;
        PlayerStats.MyInstance.Debuff.Vidactual = 0;
        //barMaterial.SetFloat("_Fill", 0);
        //text.text = "0 / 0";
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        //vida escalable con vida actual
    }
}