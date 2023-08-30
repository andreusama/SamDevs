using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnLogic : MonoBehaviour
{
    public enum GameState
    {
        START,
        PLAYING,
        TOEND,
        END
    }

    public GameState gameState;
    
    public enum TurnState
    {
        Player1,
        Player2,
    }

    public TurnState actualPlayer;
    
    [SerializeField]
    private Button rollButton;
    [SerializeField]
    public TextMeshProUGUI buttonText;

    [SerializeField]
    private TextMeshProUGUI turnText;

    private int turnCount = 1;
    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.PLAYING;
        actualPlayer = TurnState.Player1;
        turnCount = 1;
    }

    //create a function that increases the turn by 1
    public void IncreaseTurn()
    {
        turnCount++;
        turnText.text = "Turn: " + turnCount;
    }

}
