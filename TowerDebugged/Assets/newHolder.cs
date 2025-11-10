using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class newHolder : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TextMeshProUGUI text;

    public Animator animator;

    public TextMeshProUGUI GetText()
    {
        return text;
    }

    public void SetText(string value)
    {
        text.text = value;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
