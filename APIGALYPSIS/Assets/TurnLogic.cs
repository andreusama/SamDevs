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

    public enum Sucess
    {
        NONE,
        WIN,
        LOSE
    }

    public Sucess sucess;

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
        sucess = Sucess.NONE;
        turnCount = 1;
    }

    //create a function that increases the turn by 1
    public void IncreaseTurn()
    {
        this.transform.GetComponent<UIArt>().PlayTurnChangeFeedbacks();
        turnCount++;
        turnText.text = "Turn: " + turnCount;
    }

}
