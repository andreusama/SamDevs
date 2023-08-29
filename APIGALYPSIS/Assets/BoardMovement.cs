using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoardMovement : MonoBehaviour
{
    public TurnLogic turnLogic;
    
    private int MAX_POS = 62; 
    public int scene;
    
    [SerializeField]
    private List<Transform> waypoints = new List<Transform>();

    [SerializeField]
    private Transform wayPointsParent;

    [Header("Pig Movement")]
    public GameObject Pig;

    [Header("Dice elements")]
    [SerializeField]
    private Button rollButton;
    [SerializeField]
    private TextMeshProUGUI diceResult;

    private UnityAction TurnAction;

    [Header("UI ELEMENTS")]
    [SerializeField]
    private TextMeshProUGUI winIndicator;

    [Header("Special Tiles")]
    [SerializeField]
    private List<int> ocaAoca = new List<int>();

    [SerializeField]
    private List<int> bridgeTile = new List<int>();

    [SerializeField]
    private int delayTile = 19;

    [SerializeField]
    private int superDelayTile = 31;

    [SerializeField]
    private int backTile = 42;

    [SerializeField]
    private int prison = 52;

    [SerializeField]
    private int deadTile = 58;

    // Start is called before the first frame update
    void Start()
    {
        TurnAction += CreateDiceRoll;
        TurnAction += turnLogic.IncreaseTurn;
        rollButton.onClick.AddListener(TurnAction);

        if (scene == 1)
        {
            ocaAoca.Add(1);
            ocaAoca.Add(5);
            ocaAoca.Add(9);
            ocaAoca.Add(14);
            ocaAoca.Add(18);
            ocaAoca.Add(23);
            ocaAoca.Add(27);
            ocaAoca.Add(32);
            ocaAoca.Add(36);
            ocaAoca.Add(41);
            ocaAoca.Add(45);
            ocaAoca.Add(50);
            ocaAoca.Add(54);
            ocaAoca.Add(59);
            ocaAoca.Add(63);

            bridgeTile.Add(6);
            bridgeTile.Add(12);
        }
        foreach (Transform item in wayPointsParent.transform)
        {
            AddWayPoint(item);
        }

        SortWayPoints();
        SortTileTypes();
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
                waypoints[i].GetComponentInChildren<Tile>().PushSequencePos(ocaAoca);
            }
            else if (bridgeTile.Contains(i))
            {
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.BRIDGE);
                waypoints[i].GetComponentInChildren<Tile>().PushSequencePos(bridgeTile);
            }
            else if (i == delayTile)
            {
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.DELAY);
            }
            else if (i == superDelayTile)
            {
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.SUPERDELAY);
            }
            else if (i == backTile)
            {
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.BACK);
            }
            else if (i == prison)
            {
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.PRISON);
            }
            else if (i == deadTile)
            {
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.DEAD);
            }
            else
            {
                waypoints[i].GetComponentInChildren<Tile>().SetType(Tile.TYPE.NORMAL);
            }


            waypoints[i].GetComponentInChildren<Tile>().SetNameText();
        }
    }

    //create a function that given a waypoint returns the type of tile he's in
    public Tile.TYPE GetTileType(Transform waypoint)
    {
        return waypoint.GetComponentInChildren<Tile>().GetTileType();
    }

    public Tile GetTileFromWaypoint(Transform waypoint)
    {
        return waypoint.GetComponentInChildren<Tile>();
    }
    
    private int ParseIndexOnlyWaypoint(string waypointName)
    {
        int index = int.Parse(waypointName.Substring(waypointName.LastIndexOf("t") + 1));
        Debug.Log("Returning" + index + " for name: " + waypointName);
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

            Debug.Log("Moving to newPosition" + newPosition);
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
            Pig.transform.GetComponent<Player>().BoardPos = newPosition;
            Pig.transform.position = waypoints[newPosition].position;
            Finish();

            return;
        }
        
        if (newPosition > MAX_POS)
        {
            newPosition = MAX_POS - (newPosition - MAX_POS);

            Pig.transform.GetComponent<Player>().BoardPos = newPosition;
            Pig.transform.position = waypoints[newPosition].position;
            Debug.Log("The Player is actually in: " + GetTileType(waypoints[newPosition]).ToString());
        }
        else
        {
            Pig.transform.GetComponent<Player>().BoardPos = newPosition;
            Pig.transform.position = waypoints[newPosition].position;
            Debug.Log("The Player is actually in: " + GetTileType(waypoints[newPosition]).ToString());
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


    public void CreateDiceRoll()
    {
        //create a random number between 1 and 6
        int diceRoll = Random.Range(1, 7);
        //set the dice result text to the dice roll
        SetDiceResult(diceRoll.ToString());

        //set the pig to the waypoint position
        ParseWaypoint(diceRoll.ToString());
    }

}
