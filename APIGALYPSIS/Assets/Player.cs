using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class MovementState
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
        PREBLOCKED,
        BLOCKED,
        POSTBLOCKED
    }

    [SerializeField]
    public State state;

    [SerializeField]
    private int turnsBlocked;

    public int TurnsBlocked
    {
        get { return turnsBlocked; }
        set 
        {
            turnsBlocked = value;

            if (value > 0 && state == State.READY)
            {
                Debug.Log("READY and turnsBlocked setted to : " + turnsBlocked);
                state = State.PREBLOCKED;
            }
            else if (value == 0)
            {
                state = State.POSTBLOCKED;
            }
        }
    }
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

    public MovementState MovementState
    {
        get { return movementState; }
        set { movementState = value; }
    }

    public Vector3 newPosition = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        boardPos = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BlockUI()
    {
        switch (movementState.state)
        {
            case MovementState.State.POSTBLOCKED:
                this.transform.GetComponent<Image>().color = Color.green;
                break;
            case MovementState.State.PREBLOCKED:
                this.transform.GetComponent<Image>().color = Color.red;
                break;
            default:
                break;
        }
    }
}
