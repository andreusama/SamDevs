using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class buffHolder : MonoBehaviour
{
    public Image image;
    public GameObject freezed;
    public GameObject description;
    public TextMeshProUGUI descriptionText;
    private bool open;

    // Start is called before the first frame update
    void Start()
    {
        freezed.SetActive(true);
        description.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RequestDestroy()
    {
        GameObject gc = GameObject.FindWithTag("GameController");
        gc.GetComponent<skillController>().DestroyBuff(this.transform.gameObject);
    }

    public void InfoDescription()
    {
        open = !open;
        description.SetActive(open);
    }
}
