using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Networking.UnityWebRequest;

public class ActiveHolder : MonoBehaviour
{
    [SerializeField]
    private Active active;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private Image sprite;
        
    // Start is called before the first frame update
    void Start()
    {
        nameText.text = active.name;
        sprite.sprite = active.GetSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Equip()
    {
        MenuUI.MyMenuUiInstance.InitializeActivePanel(active.GetSprite(), active.name);
        LevelTraveler.MyTravelInstance.Gear.active = active;
        //FlipEquip();
        //StartCoroutine(VisualEquiping());
    }
    
    private IEnumerator VisualEquiping()
    {
        yield return new WaitForSeconds(0.3f);

        //MenuUI.MyMenuUiInstance.InitializeCraftPanel(result.sprite, holderNameText.text);
        LevelTraveler.MyTravelInstance.Gear.active = active;

        yield break;
    }

    public void FlipEquip()
    {
        //MenuFeedbacks.MyMenuFeedback.PlayFlipEquip(this);
    }
}
