using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class shopSlot : MonoBehaviour {

    //river id 0
    //volcano id 1
    //forest id 2
    public int id;
    public Color32 color;
    public Image image;
    public int counter;
    public TextMeshProUGUI actualText;

    public int riverCounter = 1;
    public int volcanoCounter = 1;
    public int forestCounter = 1;

    public Button button;

    public string slotType;

    private enum type
    {
        riverTile,
        lavaTile,
        forestTile
    };
    // Use this for initialization
    void Start ()
    {
        if(id == 0)
        {
            counter = riverCounter;
            slotType = type.riverTile.ToString();
            actualText = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
            button = this.gameObject.GetComponentInChildren<Button>();
            actualText.text = "x" + counter.ToString();
        }
        else if(id == 1)
        {
            counter = forestCounter;
            slotType = type.forestTile.ToString();
            actualText = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
            button = this.gameObject.GetComponentInChildren<Button>();
            actualText.text = "x" + counter.ToString();
        }
        else if(id == 2)
        {
            counter = volcanoCounter;
            slotType = type.lavaTile.ToString();
            actualText = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
            button = this.gameObject.GetComponentInChildren<Button>();
            actualText.text = "x" + counter.ToString();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void SetCounter(string type)
    {
       
    }
}
