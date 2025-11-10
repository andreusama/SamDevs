using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMatrix : MonoBehaviour
{
    public GameObject patternElement;
    // Start is called before the first frame update
    private void Awake()
    {
        for (int i = 0; i < TimeController.MyTimeInstance.GetPatternPeriod; i++)
        {
            GameObject newPatternElement = Instantiate(patternElement, transform);
            Debugger.MyTowerInstance.patternDebug.Add(newPatternElement.GetComponent<Image>());
        }
    }
    void Start()
    {
        //instantiate the same number as patternSteps exists in the TimeController
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
