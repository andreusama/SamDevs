using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGang : MonoBehaviour
{
    public GameObject hunters;
    
    public Sprite grassSprite;

    [SerializeField]
    private BoardMovement boardReference;

    [SerializeField]
    private int boardPos;

    private int bufferWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        boardPos = 63;
        bufferWaypoint = boardPos - 1;

        this.transform.position = boardReference.GetWaypointList()[0].transform.position;
        boardReference.GetTileOfWaypoint(boardReference.GetWaypointList()[62]).pigVisuals.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (boardPos >= 0)
        {
            if (Vector3.Distance(this.transform.position, boardReference.GetWaypointList()[bufferWaypoint].position) > 0.1f)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, boardReference.GetWaypointList()[bufferWaypoint].position, 1.5f);
            }
            else
            {
                if (Vector3.Distance(this.transform.position, boardReference.GetWaypointList()[0].position) < 0.1f)
                {
                    boardPos = 0;
                    boardReference.GetWaypointList()[bufferWaypoint].GetComponentInChildren<Tile>().PlayTileFeedback();
                    //change the sprite of the tile to burned
                    boardReference.GetWaypointList()[bufferWaypoint].GetComponentInChildren<Tile>().SetSprite(grassSprite, true);

                    return;
                }

                boardReference.GetWaypointList()[bufferWaypoint].GetComponentInChildren<Tile>().PlayTileFeedback();
                //change the sprite of the tile to burned
                boardReference.GetWaypointList()[bufferWaypoint].GetComponentInChildren<Tile>().SetSprite(grassSprite, true);

                bufferWaypoint--;
            }
        }
    }
}
