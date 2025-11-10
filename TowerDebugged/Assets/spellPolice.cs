using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spellPolice : MonoBehaviour
{
    private GameObject gc;
    
    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindWithTag("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BegginDrag()
    {
        skillController.MySkillInstance.draggingSign = true;
        Debug.Log("Beggining Drag on Spell Police");
    }
    
    public void EndDrag()
    {
        Debug.Log("EXIT drag!!!" + "Value of firstCheckPoint is: " + skillController.MySkillInstance.draggingSign);
        skillController.MySkillInstance.draggingSign = false;
        skillController.MySkillInstance.SetExit(true);
    }
}
