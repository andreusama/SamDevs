using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BoardMovement;

public class Hunters : MonoBehaviour
{
    public Sprite burnedSprite;
    
    [SerializeField]
    private BoardMovement boardReference;

    [SerializeField]
    private int boardPos;

    private int bufferWaypoint;

    public float sleepTime = 10f;

    [SerializeField]
    private float counter = 0f;

    private float speed = 0.02f;

    public float SetSpeed
    {
        set { speed = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        boardPos = -1;
        bufferWaypoint = boardPos + 1;

        //this.transform.position = boardReference.GetWaypointList()[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (boardReference.turnLogic.sucess == TurnLogic.Sucess.WIN)
        {
            return;
        }
        
        counter += Time.deltaTime;
        if (counter < sleepTime)
        {
            return;
        }
        
        if (boardPos < 63)
        {
            if (Vector3.Distance(this.transform.position, boardReference.GetWaypointList()[bufferWaypoint].position) > 0.1f)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, boardReference.GetWaypointList()[bufferWaypoint].position, speed);
            }
            else
            {
                if (Vector3.Distance(this.transform.position, boardReference.GetWaypointList()[62].position) < 0.1f)
                {
                    boardPos = 62;
                    boardReference.turnLogic.sucess = TurnLogic.Sucess.LOSE;
                    boardReference.turnLogic.gameState = TurnLogic.GameState.TOEND;
                    boardReference.SetPhase(TurnPhase.STOPPED);
                    
                    boardReference.GetTileOfWaypoint(boardReference.GetWaypointList()[62]).pigVisuals.gameObject.SetActive(false);
                    boardReference.GetWaypointList()[bufferWaypoint].GetComponentInChildren<Tile>().PlayTileFeedback();
                    //change the sprite of the tile to burned
                    boardReference.GetWaypointList()[bufferWaypoint].GetComponentInChildren<Tile>().SetSprite(burnedSprite, true);
                    boardReference.GetWaypointList()[bufferWaypoint].GetComponentInChildren<Tile>().deadTile = true;
                    boardReference.GetWaypointList()[bufferWaypoint].GetComponentInChildren<Tile>().SetType(Tile.TYPE.DEAD);
                    
                    return;
                }
                
                
                boardReference.GetWaypointList()[bufferWaypoint].GetComponentInChildren<Tile>().PlayTileFeedback();
                //change the sprite of the tile to burned
                boardReference.GetWaypointList()[bufferWaypoint].GetComponentInChildren<Tile>().SetSprite(burnedSprite, true);
                boardReference.GetWaypointList()[bufferWaypoint].GetComponentInChildren<Tile>().deadTile = true;
                boardReference.GetWaypointList()[bufferWaypoint].GetComponentInChildren<Tile>().SetType(Tile.TYPE.DEAD);

                bufferWaypoint++;
            }
        }
    }

    
}
