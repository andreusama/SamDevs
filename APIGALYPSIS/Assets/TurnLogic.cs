using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnLogic : MonoBehaviour
{
    public enum TurnState
    {
        Player1,
        Player2,
    }
    
    [SerializeField]
    private Button rollButton;
    [SerializeField]
    private TextMeshProUGUI buttonText;

    [SerializeField]
    private TextMeshProUGUI turnText;

    private int turnCount = 1;
    // Start is called before the first frame update
    void Start()
    {
        turnCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //create a function that increases the turn by 1
    public void IncreaseTurn()
    {
        turnCount++;
        turnText.text = "Turn: " + turnCount;
    }
}
