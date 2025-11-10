using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using TMPro;

public class LevelHolder : MonoBehaviour
{
    public Level level;

    [SerializeField]
    private TextMeshProUGUI levelName;

    [SerializeField]
    private TextMeshProUGUI levelDescription;

    [SerializeField]
    private GameObject lockedUI;

    [SerializeField]
    private GameObject biomaDeco;
    
    // Start is called before the first frame update
    void Start()
    {
    }
    private void VisualInit()
    {
        levelName.text = level.flavorName;
        levelDescription.text = level.GetDescription;

        //set the locked
        if (level.Locked == true)
        {
            lockedUI.SetActive(true);
        }
        else
        {
            lockedUI.SetActive(false);
        }

        //here we set the visual deco of the level
        biomaDeco.SetActive(level.GetBioma().HasDeco());
    }
    public void SetLevel(Level _level)
    {
        level = _level;
        VisualInit();
    }
    // Update is called once per frame
    void Update()
    {
        if (level.updateParent == true)
        {
            VisualInit();
            level.updateParent = false;
        }
    }
}
