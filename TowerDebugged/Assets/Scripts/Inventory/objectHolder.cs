using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class objectHolder : MonoBehaviour
{
    public Element objectClass;

    public Image image;
    public TextMeshProUGUI objectName;
    public TextMeshProUGUI objectQuantity;
    public TextMeshProUGUI rareness;
    public int quantity;
    public int price;

    private GameObject detailPanel;
    // Start is called before the first frame update
    void Start()
    {
        detailPanel = GameObject.FindWithTag("DetailTag");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        objectName.text = objectClass.name;
        image.sprite = objectClass.sprite;
    }

    public void SetDetailPanel()
    {
        detailPanel.gameObject.GetComponent<DetailHolder>().UpdateDetails(this);
    }

    public void StackElement(int number) { quantity += number; objectQuantity.text = quantity.ToString(); }
}
