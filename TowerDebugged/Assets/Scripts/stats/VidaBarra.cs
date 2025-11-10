using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VidaBarra : MonoBehaviour {

    
    private float fillAmount;
    [SerializeField]
    private float lerpspeed;
    [SerializeField]
    private TextMeshProUGUI current_value;
    [SerializeField]
    private TextMeshProUGUI max_value;

    public MMProgressBar bar;

    private GameObject gc;
    public float vidamax { get; set; }//vida màxima

    //--VIDA ACTUAL--
    public float Value
    {
        set
        {
            string[] tmp = current_value.text.Split(' ', '/');
            //create a case where if the current_value is greater than 1000 the text will be displayed as 1k
            current_value.text = tmp[0] + ' ' + StatController.Aproximation(value) + ' ';
            //current_value.text = tmp[0] + ' ' + Mathf.Round(value) + ' ';
            //UPDATEBAR
            //Debug.Log("Bar_" + bar.name);
            bar.UpdateBar(Mathf.Round(value), 0, vidamax);
            fillAmount = Map(Mathf.Round(value), 0, vidamax, 0, 1);
            //create a case where if the vidamax is greater than 1000 the text will be displayed as 1k
            string[] tmpm = current_value.text.Split(' ', '/', 'm', 'a', 'x', ':');
            max_value.text = tmpm[0] + '/' + StatController.Aproximation(vidamax);
            //max_value.text = tmpm[0] + '/' + vidamax;
        }
    }
    //--VIDA MAX--

    void Awake()
    {
        gc = GameObject.FindWithTag("GameController");
        /*PlayerStats.MyInstance.Mineral.Vidactual = 0;
        PlayerStats.MyInstance.Water.Vidactual = 0;
        PlayerStats.MyInstance.Grass.Vidactual = 0;
        PlayerStats.MyInstance.Salud.Vidactual = 0;*/

        //create a normalized value taking in account the current value and the max value
        
        bar.UpdateBar01(0);
        current_value.text = ' ' + 0.ToString() + ' ';
        max_value.text = '/' + vidamax.ToString();
    }
    void Start ()
    {
        
    }

	void Update ()
    {
        
	}

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        //vida escalable con vida actual
    }
}




