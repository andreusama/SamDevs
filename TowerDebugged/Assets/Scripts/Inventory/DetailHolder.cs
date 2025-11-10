using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetailHolder : MonoBehaviour
{
    public objectHolder objectHolder;
    public Image sprite;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI typeText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateDetails(objectHolder objectHolder)
    {
        Debug.Log(objectHolder.objectName.text);
        Debug.Log("entered Update");

        Debug.Log(objectHolder.objectName.text);
        sprite.sprite = objectHolder.image.sprite;
        nameText.text = objectHolder.objectName.text;
        quantityText.text = objectHolder.objectQuantity.text;
        priceText.text = objectHolder.price.ToString();
        typeText.text = objectHolder.rareness.text;
      
    }
}
