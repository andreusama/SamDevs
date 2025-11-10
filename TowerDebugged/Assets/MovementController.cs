using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

class MovementController : MonoBehaviour
{
    public Camera Camera;
    public Transform position;
    public bool Rotate;
    protected Plane Plane;
    public float speed;
    private float dist;
    private bool dragging = false;
    private Vector2 offset;
    private Transform toDrag;

    private void Awake()
    {
        if (Camera == null)
            Camera = Camera.main;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
 
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Up");
            rotation_Camera.MyCameraInstance.Plus();
            
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            rotation_Camera.MyCameraInstance.Minus();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            rotation_Camera.MyCameraInstance.Maximum();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rotation_Camera.MyCameraInstance.Minimum();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            rotation_Camera.MyCameraInstance.Minimum();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            rotation_Camera.MyCameraInstance.Minimum();
        }


        if (Input.touchCount >= 1)
            Plane.SetNormalAndPosition(transform.forward, transform.position);
        else
            return;

        var Delta1 = Vector3.zero;
        var Delta2 = Vector3.zero;


        
        if (Input.touchCount >= 1)
        {
            //
            Touch touch;
            touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Moved)
            {
                if (rotation_Camera.MyCameraInstance.position.localPosition.y <= 0f && touch.deltaPosition.y < 0f)
                {
                    return;
                }

                if (rotation_Camera.MyCameraInstance.position.localPosition.y >= (buildController.MyBuildInstance.floorsList[rotation_Camera.MyCameraInstance.maxLevel].checkPoint.y - rotation_Camera.MyCameraInstance.offsetY) && touch.deltaPosition.y > 0f)
                {
                    return;
                }

                rotation_Camera.MyCameraInstance.position.localPosition = new Vector3(rotation_Camera.MyCameraInstance.position.localPosition.x, rotation_Camera.MyCameraInstance.position.localPosition.y + (touch.deltaPosition.y * Time.deltaTime) * speed);

            }
            if (touch.phase == TouchPhase.Ended)
            {
                //Snap the local position of the camera with SnapIt()
                rotation_Camera.MyCameraInstance.GoToLevel(SnapIt(rotation_Camera.MyCameraInstance.position.localPosition.y));
            }


        }

        
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

        return Mathf.RoundToInt(level);
    }
    protected Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        //delta
        var rayBefore = Camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = Camera.ScreenPointToRay(touch.position);
        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        //not on plane
        return Vector3.zero;
    }

    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        //position
        var rayNow = Camera.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
}