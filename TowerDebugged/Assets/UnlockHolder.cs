using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockHolder : MonoBehaviour
{
    //create a skill variable, an image variable and a TextMeshProGui variables
    public Unlock actualUnlock;
    public Skill skill;

    [Header("Skill Unlock")]
    public TextMeshProUGUI infoText;
    //public TextMeshProUGUI skillText;
    public Image skillImage;

    public Image rarenessBackground;
    public Image rarenessFront;
    
    public void Unlock(Unlock toUnlock)
    {
        infoText.text = "You have unlocked a new hammer!";
        skillImage.sprite = toUnlock.unlockedRecipe.skillResult.sprite;
        //call the function setRareness of KingController with the thwo images
        KingController.MyKingInstance.SetRareness(rarenessBackground, rarenessFront, (int)toUnlock.unlockedRecipe.skillResult.rareness);
    }

    public void Continue()
    {
        UnlockController.MyInstance.Continue(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
