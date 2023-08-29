using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
class MovementState
{
    //create a constructor and a destructor
    public MovementState()
    {
        state = State.READY;
        turnsBlocked = 0;
    }

    public enum State
    {
        READY,
        MOVING,
        TOSTOP,
        STOPPED,
        BLOCKED
    }

    public State state;
    
    public int turnsBlocked;
}

public class Player : MonoBehaviour
{
    [SerializeField]
    private int boardPos = 0;

    public int BoardPos
    {
        get { return boardPos; }
        set { boardPos = value; }
    }

    [SerializeField]
    private MovementState movementState = new MovementState();

    public Vector3 newPosition = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        boardPos = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (movementState.state)
        {
            case MovementState.State.READY:
                
                break;
            case MovementState.State.MOVING:
               
                break;
            case MovementState.State.TOSTOP:
                
                break;
            case MovementState.State.STOPPED:
                
                break;
            case MovementState.State.BLOCKED:
                
                break;
            default:
                break;
        }
    }
}
