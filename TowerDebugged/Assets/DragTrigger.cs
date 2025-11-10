using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using System;

public class DragTrigger : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private enum States
    {
        DRAGGING,
        INTERTIA,
        SNAP,
        IDLE
    }

    [SerializeField]
    private States actualState;
    
    private Vector3 _velocity = Vector3.zero;
    private bool _underInertia = false;
    private float _time = 0.0f;

    private Vector3 _curPosition = Vector3.zero;

    private float SmoothTime = 0.5f;    
    public void Update()
    {
        switch (actualState)
        {
            case States.DRAGGING:
                
                break;
            case States.INTERTIA:
                if (_underInertia && _time <= SmoothTime)
                {
                    rotation_Camera.MyCameraInstance.position.localPosition = BoundedInertia(rotation_Camera.MyCameraInstance.position.localPosition - (_velocity * Time.smoothDeltaTime * speed));
                    _velocity = Vector3.Lerp(_velocity, Vector3.zero, _time);
                    _time += Time.smoothDeltaTime;
                }
                else
                {
                    _underInertia = false;
                    _time = 0.0f;
                    actualState = States.SNAP;
                }
                break;
            case States.SNAP:

                Debug.Log("Snaping!");
                
                rotation_Camera.MyCameraInstance.GoToLevel(SnapIt(rotation_Camera.MyCameraInstance.position.localPosition.y));

                actualState = States.IDLE;
                break;
            case States.IDLE:
                
                break;
            default:
                break;
        }
        
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = true;
    }
    
    public float speed;
    public void OnBeginDrag(PointerEventData eventData)
    {
        actualState = States.DRAGGING;
        _underInertia = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(SuperDrag(_velocity))
        {
            actualState = States.SNAP;
            return;
        }
        else
        {
            actualState = States.INTERTIA;

            _underInertia = true;
        }
    }

    //IDragHandler
    public void OnDrag(PointerEventData eventData)
    {
        _velocity = new Vector3(0, eventData.delta.y, 0);

        Debug.Log("Dragging with a Y of: " + eventData.delta.y);

        rotation_Camera.MyCameraInstance.position.localPosition = new Vector3(rotation_Camera.MyCameraInstance.position.localPosition.x, BoundedValue(rotation_Camera.MyCameraInstance.position.localPosition.y - (eventData.delta.y * Time.smoothDeltaTime) * speed), 0f);
    }

    public int SnapIt(float yCoord)
    {
        float level = 0;

        //Debug.Log("Position Y of Floor 1 CP: " + buildController.MyBuildInstance.floorsList[1].checkPoint.y);
        //Debug.Log("Y coord: " + yCoord);
        yCoord = yCoord - buildController.MyBuildInstance.floorsList[1].checkPoint.y;
        //Debug.Log("Y coord to process: " + yCoord);
        level = yCoord / 3.4f;
        level += 1;

        Debug.Log("It would be snapped to: " + Mathf.RoundToInt(level));

        //check that the level returned is not 0 or negative


        return SafeValue(Mathf.RoundToInt(level));
    }

    private int SafeValue(int safeLevel)
    {
        if (safeLevel < 1)
        {
            safeLevel = 1;
        }
        else if (safeLevel > rotation_Camera.MyCameraInstance.maxLevel)
        {
            safeLevel = rotation_Camera.MyCameraInstance.maxLevel;
        }

        return safeLevel;
    }

    float BoundedValue(float futureValue)
    {
        if (futureValue > 0f)
        {
            return futureValue;
        }
        else if (futureValue < 0f)
        {
            return 0f;
        }
        
        if(futureValue < buildController.MyBuildInstance.floorsList[rotation_Camera.MyCameraInstance.maxLevel].checkPoint.y)
        {
            return futureValue;
        }
        else
        {
            return buildController.MyBuildInstance.floorsList[rotation_Camera.MyCameraInstance.maxLevel].checkPoint.y;
        }
    }

    Vector3 BoundedInertia(Vector3 futurePos)
    {
        if (futurePos.y > 0f)
        {
            return new Vector3(0.09f, futurePos.y, 0f);
        }
        else if (futurePos.y < 0f)
        {
            return new Vector3(0.09f, 0f, 0f);
        }

        if (futurePos.y < buildController.MyBuildInstance.floorsList[rotation_Camera.MyCameraInstance.maxLevel].checkPoint.y)
        {
            return new Vector3(0.09f, futurePos.y, 0f);
        }
        else
        {
            return new Vector3(0.09f, buildController.MyBuildInstance.floorsList[rotation_Camera.MyCameraInstance.maxLevel].checkPoint.y, 0f);
        }
    }

    public bool Inertia(float coordY)
    {
        if (SnapIt(coordY) > 1 && SnapIt(coordY) < rotation_Camera.MyCameraInstance.maxLevel)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool SuperDrag(Vector3 velocity)
    {
        if (velocity.y > 300f)
        {
            rotation_Camera.MyCameraInstance.GoToLevel(1);
            return true;
        }
        else if (velocity.y < -300f)
        {
            rotation_Camera.MyCameraInstance.GoToLevel(rotation_Camera.MyCameraInstance.maxLevel);
            return true;
        }
        return false;
    }
}