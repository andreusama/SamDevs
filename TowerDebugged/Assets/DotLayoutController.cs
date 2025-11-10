using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DotLayoutController : MonoBehaviour
{

    public Image dot;
    // Start is called before the first frame update
    void Start()
    {
        dot.transform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reached()
    {
        dot.transform.gameObject.SetActive(true);
    }
}
