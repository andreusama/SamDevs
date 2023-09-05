using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoardMovement : MonoBehaviour
{
    public TurnLogic turnLogic;
    private UIArt uiArt;
    public GameObject TheGang;
    public GameObject huntersGO;
    public enum TurnPhase
    {
        ACTING,
        MOVING,
        TOSTOP,
        STOPPED
    }

    [SerializeField]
    private TurnPhase actualPhase;

    public TurnPhase SetPhase(TurnPhase newPhase)
    {
        actualPhase = newPhase;
        return actualPhase;
    }
        

    private int MAX_POS = 62; 
    public int scene;
    
    [SerializeField]
    private List<Transform> waypoints = new List<Transform>();

    public List<Transform> GetWaypointList()
    {
        return waypoints;
    }
    [SerializeField]
    private Transform wayPointsParent;

    [Header("Pig Movement")]
    public GameObject Pig;
    [SerializeField]
    private int aimWaypoint;

    [SerializeField]
    private int bufferWaypoint;

    [SerializeField]
    private float movingTime = 1f;

    [SerializeField]
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

    private int delayTile = -1;

    private int superDelayTile = -1;

    private int backTile = -1;

    private int prison = -1;

    private int deadTile = -1;

    // Start is called before the first frame update
    void Start()
    {
        uiArt = this.transform.GetComponent<UIArt>();
        NewTurn(Pig);

        if (turnLogic != null)
            turnLogic.buttonText.text = "ROLL THE DICE!";
        
        TurnAction += CreateDiceRoll;

        if (turnLogic != null)
            TurnAction += turnLogic.IncreaseTurn;
        
        //rollButton.onClick.AddListener(TurnAction);

        if (scene != 0)
        {
            delayTile = 18;

            superDelayTile = 30;

            backTile = 41;

            prison = 51;

            deadTile = 57;
    
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

        
    }


    //create a function to set the position directly without the need of the dice
    public void InitPig(int pos)
    {
        Pig.SetActive(true);
        if (pos > MAX_POS)
        {
            pos = MAX_POS;
        }
        else if (pos < 0)
        {
            pos = 0;
        }
        Pig.transform.position = waypoints[pos].position;
        Pig.transform.GetComponent<Player>().BoardPos = pos;
    }
    public void Update()
    {
        //create a input for keys 1,2,3 and 4
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            turnLogic.buttonText.text = "ROLLING...";

            StartCoroutine(AnimateDiceRoll(11));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            turnLogic.buttonText.text = "ROLLING...";

            StartCoroutine(AnimateDiceRoll(18));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            turnLogic.buttonText.text = "ROLLING...";

            StartCoroutine(AnimateDiceRoll(30));
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            turnLogic.buttonText.text = "ROLLING...";

            StartCoroutine(AnimateDiceRoll(41));
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            turnLogic.buttonText.text = "ROLLING...";

            StartCoroutine(AnimateDiceRoll(57));
        }
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

                //check if waypoints[bufferWaypoint].position is part of the collection or is out of range
                if (Vector3.Distance(Pig.transform.position, waypoints[bufferWaypoint].position) > 0.1f)
                {
                    if (Pig.transform.GetComponent<Player>().MovementState.direction == MovementState.Direction.FORWARD)
                        waypoints[bufferWaypoint - 1].GetComponentInChildren<Tile>().PlayTileFeedback();
                    else
                        waypoints[bufferWaypoint + 1].GetComponentInChildren<Tile>().PlayTileFeedback();

                    if (turnLogic != null)
                        turnLogic.buttonText.text = "MOVING...";
                    
                    //check if when we moving the tile changes state and is dead
                    
                    
                    
                    Pig.transform.position = Vector3.Lerp(Pig.transform.position, waypoints[bufferWaypoint].position, 0.1f);
                }
                else
                {
                    if (Vector3.Distance(Pig.transform.position, waypoints[aimWaypoint].position) < 0.1f)
                    {
                        Pig.transform.GetComponent<Player>().BoardPos = aimWaypoint;
                        actualPhase = TurnPhase.TOSTOP;

                        return;
                    }

                    if (Pig.transform.GetComponent<Player>().MovementState.direction == MovementState.Direction.FORWARD)
                    {
                        Debug.Log("Buffer waypoint increasing from MOVING" + bufferWaypoint);
                        waypoints[bufferWaypoint].GetComponentInChildren<Tile>().PlayTileFeedback();

                        bufferWaypoint++;
                        SetBufferWaypoint(bufferWaypoint);
                    }
                    else
                    {
                        Debug.Log("Buffer waypoint decreasing from MOVING" + bufferWaypoint);
                        waypoints[bufferWaypoint].GetComponentInChildren<Tile>().PlayTileFeedback();

                        bufferWaypoint--;
                        SetBufferWaypoint(bufferWaypoint);
                    }
                }

                break;
            case TurnPhase.TOSTOP:

                int jumpCase = GetTileOfWaypoint(waypoints[aimWaypoint]).TileEffect(aimWaypoint, winIndicator);

                int blockCase = GetTileOfWaypoint(waypoints[aimWaypoint]).TileEffect(aimWaypoint, winIndicator);

                int deadCase = GetTileOfWaypoint(waypoints[aimWaypoint]).TileEffect(aimWaypoint, winIndicator);

                if (jumpCase != 1 && jumpCase >= 0)
                {
                    Pig.transform.position = waypoints[GetTileOfWaypoint(waypoints[aimWaypoint]).TileEffect(aimWaypoint, winIndicator)].position;
                    Pig.transform.GetComponent<Player>().BoardPos = GetTileOfWaypoint(waypoints[aimWaypoint]).TileEffect(aimWaypoint, winIndicator);
                    if (CheckFinish(GetTileOfWaypoint(waypoints[aimWaypoint]).TileEffect(aimWaypoint, winIndicator)) == true)
                    {
                        if (turnLogic != null)
                        {
                            turnLogic.sucess = TurnLogic.Sucess.WIN;
                            turnLogic.gameState = TurnLogic.GameState.TOEND;
                        }
                    }
                }

                if (blockCase < 0 && blockCase != -100)
                {
                    Debug.Log("Block case was: " + Mathf.Abs(blockCase));
                    Pig.transform.GetComponent<Player>().MovementState.TurnsBlocked += Mathf.Abs(blockCase);
                }

                if (deadCase == -100)
                {
                    if (turnLogic != null)
                    {
                        turnLogic.sucess = TurnLogic.Sucess.LOSE;
                        turnLogic.gameState = TurnLogic.GameState.TOEND;
                    }
                }

                actualPhase = TurnPhase.STOPPED;
                break;
            case TurnPhase.STOPPED:
                
                if (turnLogic == null)
                    return;
                
                if (turnLogic.gameState == TurnLogic.GameState.TOEND)
                {
                    //take the tile we're on (last one) and set false the pigVisuals
                    
                    turnLogic.gameState = TurnLogic.GameState.END;
                    TurnAction -= CreateDiceRoll;
                    TurnAction -= turnLogic.IncreaseTurn;

                    //NICE PLACE TO PUT THE FINISH GAME FEEDBACK

                    
                    Finish(turnLogic.sucess);

                    return;
                }
                
                
                NewTurn(Pig);
                
                break;
            default:
                break;
        }
    }

    //create a method that moves the pig from a waypoint x to a waypoint y
    public void SetBufferWaypoint(int x)
    {
        if (x < 0 || x > MAX_POS)
        {
            Debug.LogError("The waypoint is out of range");
            return;
        }

        bufferWaypoint = x;
    }

    public void TurnCallback()
    {
        TurnAction?.Invoke();
    }

    private void NewTurn(GameObject Pig)
    {
        if (turnLogic == null)
        {
            return;
        }

        rollButton.interactable = true;

        switch (Pig.GetComponent<Player>().MovementState.state)
        {
            case MovementState.State.READY:
                turnLogic.buttonText.text = "ROLL THE DICE!";
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

                if (scene == 1)
                    waypoints[i].GetComponentInChildren<Tile>().SetColor(Color.blue);

                waypoints[i].GetComponentInChildren<Tile>().PushSequencePos(ocaAoca);
            }
            else if (bridgeTile.Contains(i))
            {
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.BRIDGE);

                if (scene == 1)
                    waypoints[i].GetComponentInChildren<Tile>().SetColor(Color.cyan);

                waypoints[i].GetComponentInChildren<Tile>().PushSequencePos(bridgeTile);
            }
            else if (i == delayTile)
            {
                Debug.Log("Created DELAY in: " + i);

                if (scene == 1)
                    waypoints[i].GetComponentInChildren<Tile>().SetColor(new Color(148f,76f,0f,200f));

                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.DELAY);
            }
            else if (i == superDelayTile)
            {
                Debug.Log("Created SUPERDELAY in: " + i);

                if (scene == 1)
                    waypoints[i].GetComponentInChildren<Tile>().SetColor(Color.magenta);

                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.SUPERDELAY);
            }
            else if (i == backTile)
            {
                if (scene == 1)
                    waypoints[i].GetComponentInChildren<Tile>().SetColor(new Color(0, 45, 148f, 200f));
                
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.BACK);
            }
            else if (i == prison)
            {
                if (scene == 1)
                    waypoints[i].GetComponentInChildren<Tile>().SetColor(Color.red);
                
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.PRISON);
            }
            else if (i == deadTile)
            {
                if (scene == 1)
                    waypoints[i].GetComponentInChildren<Tile>().SetColor(Color.black);
                
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.DEAD);
            }
            else
            {
                if (scene == 0 || scene == 1)
                    waypoints[i].GetComponentInChildren<Tile>().SetColor(Color.grey);
                
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.NORMAL);
            }

            if (scene == 0 || scene == 1)
            {
                waypoints[i].GetComponentInChildren<Tile>().SetNameText();
                waypoints[i].GetComponentInChildren<Tile>().DisableSprite();
            }
            else
            {
                waypoints[i].GetComponentInChildren<Tile>().SetSprite();
            }
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

            SetPigNextMovement(newPosition);
        }
        else
        {
            Debug.LogError("Error parsing waypoint index: " + newWaypoint);
        }
    }
        
    private void SetPigNextMovement(int newPosition)
    {
        //here we have two cases, if the newPosition is exactly the same as the MAX_POS we'll set the pig to the newPosition and show the win indicator
        //in the case that the newPosition is greater than the MAX_POS we'll set the pig to the substraction of the newPosition and the MAX_POS
        //and the last case is that it is between the initialPos and the max one and we'll just move the pig to the waypoint
        if (newPosition == MAX_POS)
        {
            //Pig.transform.GetComponent<Player>().BoardPos = newPosition;
            //Pig.transform.position = waypoints[newPosition].position;
            if(turnLogic != null)
            {
                turnLogic.sucess = TurnLogic.Sucess.WIN;
                turnLogic.gameState = TurnLogic.GameState.TOEND;
            }
       
            SetBufferWaypoint(bufferWaypoint = (Pig.transform.GetComponent<Player>().BoardPos + 1));
            
            Pig.transform.GetComponent<Player>().MovementState.direction = MovementState.Direction.FORWARD;
            
            Debug.Log("Setting buffer from Set Pig Next Movement MAX_POS CASE " + bufferWaypoint);

            aimWaypoint = newPosition;
            actualPhase = TurnPhase.MOVING;

            return;
        }
        
        if (newPosition > MAX_POS)
        {
            newPosition = MAX_POS - (newPosition - MAX_POS);

            //Pig.transform.GetComponent<Player>().BoardPos = newPosition;
            //Pig.transform.position = waypoints[newPosition].position;

            if (newPosition == Pig.transform.GetComponent<Player>().BoardPos)
            {
                SetBufferWaypoint(bufferWaypoint = Pig.transform.GetComponent<Player>().BoardPos);
            }
            else if (newPosition < Pig.transform.GetComponent<Player>().BoardPos)
            {
                Pig.transform.GetComponent<Player>().MovementState.direction = MovementState.Direction.BACKWARDS;
                SetBufferWaypoint(bufferWaypoint = (Pig.transform.GetComponent<Player>().BoardPos - 1));
            }
            else if (newPosition > Pig.transform.GetComponent<Player>().BoardPos)
            {
                Pig.transform.GetComponent<Player>().MovementState.direction = MovementState.Direction.FORWARD;
                SetBufferWaypoint(bufferWaypoint = (Pig.transform.GetComponent<Player>().BoardPos + 1));
            }

            Debug.Log("Setting buffer from Set Pig Next Movement REBOUND MAX_POS CASE " + bufferWaypoint);
            aimWaypoint = newPosition;
            actualPhase = TurnPhase.MOVING;
        }
        else
        {

            SetBufferWaypoint(bufferWaypoint = (Pig.transform.GetComponent<Player>().BoardPos + 1));
            Pig.transform.GetComponent<Player>().MovementState.direction = MovementState.Direction.FORWARD;
            Debug.Log("Setting buffer from Set Pig Next Movement NORMAL CASE " + bufferWaypoint);
            aimWaypoint = newPosition;
            actualPhase = TurnPhase.MOVING;
        }
    }

    private void Finish(TurnLogic.Sucess sucess)
    {
        Pig.SetActive(false);
        
        switch (sucess)
        {
            case TurnLogic.Sucess.NONE:
                Debug.Log("Sucess was NOT SET");
                break;
            case TurnLogic.Sucess.WIN:
                if (winIndicator != null)
                    winIndicator.text = "APIGALYPSIS!";

                huntersGO.gameObject.SetActive(false);
                TheGang.gameObject.SetActive(true);
                break;
            case TurnLogic.Sucess.LOSE:
                if (winIndicator != null)
                    winIndicator.text = "DEAD!";

                huntersGO.GetComponent<Hunters>().SetSpeed = 1.5f;
                
                break;
            default:
                break;
        }
        

        

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

        turnLogic.buttonText.text = "ROLLING...";

        StartCoroutine(AnimateDiceRoll(diceRoll));
    }

    //create a coroutine that just gets the text of the dice result and animate the number with random ones until it reaches the dice roll given as an argument
    public IEnumerator AnimateDiceRoll(int diceRoll)
    {
        rollButton.interactable = false;
        uiArt.PlayRollFeedback();
        
        string diceResultString = diceResult.text;

        string result = "";

        int random = 0;

        int counter = 0;

        while (counter < 10)
        {
            //get a random number between 1 and 6
            random = Random.Range(1, 7);
            //set the result to the random number
            result = random.ToString();
            //set the text of the dice result to the result
            diceResult.text = result;
            //wait for 0.1 seconds
            yield return new WaitForSeconds(0.04f);
            //increase the counter
            counter++;
            //if the counter is greater than 10
        }

        if (diceRoll > 6)
        {
            result = "6";
        }
        else
        {
            result = diceRoll.ToString();
        }


        //set the text of the dice result to the dice roll
        SetDiceResult(diceRoll.ToString());
        //set the pig to the waypoint position
        ParseWaypoint(diceRoll.ToString());
    }
}
