using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoardMovement : MonoBehaviour
{
    public TurnLogic turnLogic;
    
    public enum TurnPhase
    {
        ACTING,
        MOVING,
        TOSTOP,
        STOPPED
    }

    [SerializeField]
    private TurnPhase actualPhase;
    
    private int MAX_POS = 62; 
    public int scene;
    
    [SerializeField]
    private List<Transform> waypoints = new List<Transform>();

    [SerializeField]
    private Transform wayPointsParent;

    [Header("Pig Movement")]
    public GameObject Pig;
    [SerializeField]
    private int aimWaypoint;

    private float movingTime = 0.1f;
    private float timeCounter = 0f;

    [Header("Dice elements")]
    [SerializeField]
    private Button rollButton;
    [SerializeField]
    private TextMeshProUGUI diceResult;

    [Header("Unity Event")]
    [SerializeField]
    private UnityAction TurnAction;

    [Header("UI ELEMENTS")]
    [SerializeField]
    private TextMeshProUGUI winIndicator;

    [Header("Special Tiles")]
    [SerializeField]
    private List<int> ocaAoca = new List<int>();

    [SerializeField]
    private List<int> bridgeTile = new List<int>();

    private int delayTile = 18;

    private int superDelayTile = 30;

    private int backTile = 41;

    private int prison = 51;

    private int deadTile = 57;

    // Start is called before the first frame update
    void Start()
    {
        NewTurn(Pig);

        turnLogic.buttonText.text = "ROLL THE DICE!";
        TurnAction += CreateDiceRoll;
        TurnAction += turnLogic.IncreaseTurn;
        
        //rollButton.onClick.AddListener(TurnAction);

        if (scene == 1)
        {
            ocaAoca.Add(0);
            ocaAoca.Add(4);
            ocaAoca.Add(8);
            ocaAoca.Add(13);
            ocaAoca.Add(17);
            ocaAoca.Add(22);
            ocaAoca.Add(26);
            ocaAoca.Add(31);
            ocaAoca.Add(35);
            ocaAoca.Add(40);
            ocaAoca.Add(44);
            ocaAoca.Add(49);
            ocaAoca.Add(53);
            ocaAoca.Add(58);
            ocaAoca.Add(62);

            bridgeTile.Add(5);
            bridgeTile.Add(11);
        }
        foreach (Transform item in wayPointsParent.transform)
        {
            AddWayPoint(item);
        }

        actualPhase = TurnPhase.ACTING;
        
        SortWayPoints();
        SortTileTypes();

        ParseWaypoint("0");
    }

    public void Update()
    {
        //create a debug where i press G 
        if (Input.GetKeyDown(KeyCode.G))
        {
            System.Delegate[] invokeList;

            invokeList = TurnAction.GetInvocationList();
            foreach(System.Delegate d in invokeList)
            {
                Debug.Log(d.Method);
            }
        }
        switch (actualPhase)
        {
            case TurnPhase.ACTING:
                
                break;
            case TurnPhase.MOVING:
                Pig.transform.position = Vector3.Lerp(Pig.transform.position, waypoints[aimWaypoint].position, 0.1f);
                if (Vector3.Distance(Pig.transform.position, waypoints[aimWaypoint].position) < 0.1f)
                {
                    Pig.transform.GetComponent<Player>().BoardPos = aimWaypoint;
                    actualPhase = TurnPhase.TOSTOP;
                }
                break;
            case TurnPhase.TOSTOP:

                int jumpCase = GetTileOfWaypoint(waypoints[aimWaypoint]).TileEffect(aimWaypoint);

                int blockCase = GetTileOfWaypoint(waypoints[aimWaypoint]).TileEffect(aimWaypoint);


                if (jumpCase != 1 && jumpCase >= 0)
                {
                    Pig.transform.position = waypoints[GetTileOfWaypoint(waypoints[aimWaypoint]).TileEffect(aimWaypoint)].position;
                    Pig.transform.GetComponent<Player>().BoardPos = GetTileOfWaypoint(waypoints[aimWaypoint]).TileEffect(aimWaypoint);
                    if (CheckFinish(GetTileOfWaypoint(waypoints[aimWaypoint]).TileEffect(aimWaypoint)) == true)
                    {
                        turnLogic.gameState = TurnLogic.GameState.TOEND;
                    }
                }

                if (blockCase < 0)
                {
                    Debug.Log("Block case was: " + Mathf.Abs(blockCase));
                    Pig.transform.GetComponent<Player>().MovementState.TurnsBlocked += Mathf.Abs(blockCase);
                }


                actualPhase = TurnPhase.STOPPED;
                break;
            case TurnPhase.STOPPED:

                if (turnLogic.gameState == TurnLogic.GameState.TOEND)
                {
                    turnLogic.gameState = TurnLogic.GameState.END;

                    TurnAction -= CreateDiceRoll;
                    TurnAction -= turnLogic.IncreaseTurn;

                    //NICE PLACE TO PUT THE FINISH GAME FEEDBACK

                    Finish();
                    return;
                }
                
                NewTurn(Pig);
                
                break;
            default:
                break;
        }
    }

    public void TurnCallback()
    {
        TurnAction?.Invoke();
    }

    private void NewTurn(GameObject Pig)
    {
        switch (Pig.GetComponent<Player>().MovementState.state)
        {
            case MovementState.State.READY:
                actualPhase = TurnPhase.ACTING;
                break;
            case MovementState.State.PREBLOCKED:

                turnLogic.buttonText.text = "BLOCKED!";
                TurnAction -= CreateDiceRoll;
                TurnAction += WastedRoll;
                Pig.transform.GetComponent<Player>().BlockUI();

                Pig.GetComponent<Player>().MovementState.state = MovementState.State.BLOCKED;
                
                
                break;
            case MovementState.State.BLOCKED:

                break;
            case MovementState.State.POSTBLOCKED:

                turnLogic.buttonText.text = "ROLL THE DICE!";
                TurnAction += CreateDiceRoll;
                TurnAction -= WastedRoll;
                Pig.transform.GetComponent<Player>().BlockUI();

                Pig.GetComponent<Player>().MovementState.state = MovementState.State.READY;
                break;
            default:
                break;
        }
    }
    //sort all the waypoints and identify the type of each Tile (Oca, Bridge, etc) with the actual index
    private void SortTileTypes()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            waypoints[i].GetComponentInChildren<Tile>().index = i;
            if (ocaAoca.Contains(i))
            {
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.OCA);
                //waypoints[i].GetComponentInChildren<Tile>().SetColor(Color.blue);
                waypoints[i].GetComponentInChildren<Tile>().PushSequencePos(ocaAoca);
            }
            else if (bridgeTile.Contains(i))
            {
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.BRIDGE);
                //waypoints[i].GetComponentInChildren<Tile>().SetColor(Color.cyan);
                waypoints[i].GetComponentInChildren<Tile>().PushSequencePos(bridgeTile);
            }
            else if (i == delayTile)
            {
                Debug.Log("Created DELAY in: " + i);
                //waypoints[i].GetComponentInChildren<Tile>().SetColor(new Color(148f,76f,0f,200f));
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.DELAY);
            }
            else if (i == superDelayTile)
            {
                Debug.Log("Created SUPERDELAY in: " + i);
                //waypoints[i].GetComponentInChildren<Tile>().SetColor(Color.magenta);
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.SUPERDELAY);
            }
            else if (i == backTile)
            {
                //waypoints[i].GetComponentInChildren<Tile>().SetColor(new Color(0, 45, 148f, 200f));
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.BACK);
            }
            else if (i == prison)
            {
                //waypoints[i].GetComponentInChildren<Tile>().SetColor(Color.red);
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.PRISON);
            }
            else if (i == deadTile)
            {
                //waypoints[i].GetComponentInChildren<Tile>().SetColor(Color.black);
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.DEAD);
            }
            else
            {
                //waypoints[i].GetComponentInChildren<Tile>().SetColor(Color.grey);
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.NORMAL);
            }

            waypoints[i].GetComponentInChildren<Tile>().SetSprite();
            //waypoints[i].GetComponentInChildren<Tile>().SetNameText();
        }
    }

    //create a function that given a waypoint returns the type of tile he's in
    public Tile.TYPE GetTileType(Transform waypoint)
    {
        return waypoint.GetComponentInChildren<Tile>().GetTileType();
    }

    public Tile GetTileOfWaypoint(Transform waypoint)
    {
        return waypoint.GetComponentInChildren<Tile>();
    }
    
    private int ParseIndexOnlyWaypoint(string waypointName)
    {
        int index = int.Parse(waypointName.Substring(waypointName.LastIndexOf("t") + 1));
        return index;
    }

    private void AddWayPoint(Transform wayPoint)
    {
        waypoints.Add(wayPoint);
    }

    private void SortWayPoints()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            for (int j = 0; j < waypoints.Count - 1; j++)
            {
                if (ParseIndexOnlyWaypoint(waypoints[j].name) > ParseIndexOnlyWaypoint(waypoints[j + 1].name))
                {
                    Transform temp = waypoints[j];
                    waypoints[j] = waypoints[j + 1];
                    waypoints[j + 1] = temp;
                }
            }
        }
    }
    
    //create a function that will set the pig to the waypoint position delivered
    public void ParseWaypoint(string newWaypoint)
    {
        int parsedNewWayPoint = 0;
        if (int.TryParse(newWaypoint, out parsedNewWayPoint))
        {
            int newPosition = Pig.transform.GetComponent<Player>().BoardPos + parsedNewWayPoint;

            MovePigToWayPoint(newPosition);
        }
        else
        {
            Debug.LogError("Error parsing waypoint index: " + newWaypoint);
        }
    }
        
    private void MovePigToWayPoint(int newPosition)
    {
        //here we have two cases, if the newPosition is exactly the same as the MAX_POS we'll set the pig to the newPosition and show the win indicator
        //in the case that the newPosition is greater than the MAX_POS we'll set the pig to the substraction of the newPosition and the MAX_POS
        //and the last case is that it is between the initialPos and the max one and we'll just move the pig to the waypoint
        if (newPosition == MAX_POS)
        {
            //Pig.transform.GetComponent<Player>().BoardPos = newPosition;
            //Pig.transform.position = waypoints[newPosition].position;
            turnLogic.gameState = TurnLogic.GameState.TOEND;

            aimWaypoint = newPosition;
            actualPhase = TurnPhase.MOVING;

            return;
        }
        
        if (newPosition > MAX_POS)
        {
            newPosition = MAX_POS - (newPosition - MAX_POS);

            //Pig.transform.GetComponent<Player>().BoardPos = newPosition;
            //Pig.transform.position = waypoints[newPosition].position;

            aimWaypoint = newPosition;
            actualPhase = TurnPhase.MOVING;
            //Debug.Log("The Player is actually in: " + GetTileType(waypoints[newPosition]).ToString());
        }
        else
        {
            //Pig.transform.GetComponent<Player>().BoardPos = newPosition;
            //Pig.transform.position = waypoints[newPosition].position;
            
            aimWaypoint = newPosition;
            actualPhase = TurnPhase.MOVING;
            //Debug.Log("The Player is actually in: " + GetTileType(waypoints[newPosition]).ToString());
        }
    }

    private void Finish()
    {
        if (winIndicator != null)
            winIndicator.text = "APIGALYPSIS!!!!!";

        return;
    }

    private void SetDiceResult(string result)
    {
        diceResult.text = result;
    }

    private bool CheckFinish(int index)
    {
        if (index == MAX_POS)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void WastedRoll()
    {
        //Debug.Log("Wasted roll!");
        Pig.GetComponent<Player>().MovementState.TurnsBlocked--;
    }
    public void CreateDiceRoll()
    {
        //Debug.Log("Creating dice roll!");
        //create a random number between 1 and 6
        int diceRoll = Random.Range(1, 7);
        //set the dice result text to the dice roll
        SetDiceResult(diceRoll.ToString());

        //set the pig to the waypoint position
        ParseWaypoint(diceRoll.ToString());
    }

}
