using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniMapHolder : MonoBehaviour
{
    [SerializeField]
    private int level;
    public TextMeshProUGUI text;
    public Transform tutorialAnchor;
    public int Level { get => level; set => level = value; }

    // Start is called before the first frame update
    void Start()
    {
        text.text = "Lvl: " + Level.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoTo()
    {
        rotation_Camera.MyCameraInstance.GoToLevel(Level);
    }

    public void GoToTutorial()
    {
        if (TutorialManager.Instance.isTutorial == true)
        {
            TutorialManager.Instance.NextPhase(TutorialManager.GAMEPLAY_TUTORIAL_PHASE.MAP);
        }
        rotation_Camera.MyCameraInstance.GoToLevel(Level);
    }
}
