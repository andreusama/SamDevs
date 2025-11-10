using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderClamp : MonoBehaviour
{
    private static SliderClamp sliderClamp;

    public static SliderClamp MySliderClamp
    {
        get
        {
            if (sliderClamp == null)
            {
                sliderClamp = FindObjectOfType<SliderClamp>();
            }
            return sliderClamp;
        }


    }

    public enum States
    {
        DRAGGING,
        INERTIA,
        SNAP,
        ONSNAP,
        IDLE
    }

    public States actualState = States.IDLE;
    
    public int index = 0;

    private bool lerping = false;

    private float _time = 0.0f;
    
    public RectTransform contentPanel;

    [SerializeField]
    private RectTransform scrollRect;

    [SerializeField]
    private List<RectTransform> billboardRects = new List<RectTransform>();


    [SerializeField]
    private int levelUnlockedIndex = -1;
    
    public List<RectTransform> GetBillboardRects { get => billboardRects; set => billboardRects = value; }

    public Transform center;
    
    public bool dragging = false;

    private bool lateStart = false;

    private bool started = true;
    //make a public getter named nearestBillboard
    [SerializeField]
    private RectTransform nearestBillboard;

    [SerializeField]
    private RectTransform aimBillboard = null;

    private Vector2 snapInitialPos = Vector2.zero;
    [SerializeField]
    private float snapTime = 0.2f;

    public RectTransform nearestGetter
    {
        get
        {
            return NearestBillboard();
        }
    }

    public enum Direction 
    {
        Y,
        X
    }

    public Direction dir;

    public void Awake()
    {
        
    }
    public void Start()
    {
        //get the childs of content and add them to the list
        //Debug.Log("Billboard Count" + billboardRects.Count);

        for (int i = 0; i < contentPanel.childCount; i++)
        {
            billboardRects.Add(contentPanel.GetChild(i).GetComponent<RectTransform>());
            if (contentPanel.GetChild(i).GetComponent<LevelHolder>() != null && contentPanel.GetChild(i).GetComponent<LevelHolder>().level.Locked == false)
            {
                levelUnlockedIndex = i;
            }
            if (contentPanel.GetChild(i).GetComponent<craftHolder>() != null && contentPanel.GetChild(i).GetComponent<craftHolder>().actualRecipe.locked == false)
            {
                levelUnlockedIndex = i;
            }
        }

        //if (billboardRects.Count == 0)
        //{
        //    this.gameObject.SetActive(false);
        //}
        
        started = true;
        actualState = States.INERTIA;

        //Debug.Log("Done setting the nearest");
    }

    public void Update()
    {
        switch (actualState)
        {
            case States.DRAGGING:
                //if the state is dragging
                break;
            case States.INERTIA:
                //if the state is inertia
                if (this.GetComponent<ScrollRect>().decelerationRate >= _time)
                {
                    _time += Time.smoothDeltaTime;
                }
                else
                {
                    if (NearestBillboard() == null)
                    {
                        actualState = States.IDLE;
                        return;
                    }
                    
                    _time = 0.0f;
                    
                    //setting the values for the SNAP, if this is not setted we lose performance aswell as the initialPos will be constantly modified.
                    snapInitialPos = contentPanel.anchoredPosition;

                    Debug.Log("Snapping to the nearest!");
                    aimBillboard = NearestBillboard();

                    if (levelUnlockedIndex != -1 && started == true)
                    {
                        Debug.Log("Snapping to the last unlocked!");
                        aimBillboard = billboardRects[levelUnlockedIndex];
                        started = false;
                    }

                    //RectTransform nearest = NearestBillboard();
                    //nearestBillboard = nearest;
                    //aimBillboard = nearest;
                    this.GetComponent<ScrollRect>().velocity = Vector2.zero;
                    actualState = States.SNAP;
                }
                break;
            case States.SNAP:
                Debug.Log("Snapping!");
                if (_time <= snapTime)
                {
                    Canvas.ForceUpdateCanvases();

                    if (aimBillboard == null)
                        return;

                    contentPanel.anchoredPosition = Vector2.Lerp(snapInitialPos, (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) - (Vector2)scrollRect.transform.InverseTransformPoint(aimBillboard.position), _time / snapTime);
                }
                else
                {
                    contentPanel.anchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) - (Vector2)scrollRect.transform.InverseTransformPoint(aimBillboard.position);
                    actualState = States.ONSNAP;
                    _time = 0f;
                }
                _time += Time.smoothDeltaTime;
                //if the state is snap
                break;
            case States.ONSNAP:
                actualState = States.IDLE;
                break;
            case States.IDLE:
                //if the state is idle
                break;
            default:
                break;
        }
    }
    
    public void SnapToIndex(int _index)
    {
        if (_index < billboardRects.Count)
        {
            aimBillboard = billboardRects[_index];
            actualState = States.SNAP;
        }
        else
        {
            //Debug.Log("It was tried to snap to an index that is out of range");
        }
    }
    public void SnapTo(RectTransform target)
    {
        if (!lerping)
            StartCoroutine(SnapToCo(target));
    }

    public IEnumerator SnapToCo(RectTransform target)
    {
        //create a while that waits for scrollRect.inertia to be stopped
        //Debug.Log("Setting the scroll to false");
        lerping = true;
        
        Canvas.ForceUpdateCanvases();

        float smoothTime = 0f;
        float aim = 0.2f;
        
        Vector2 initialPos = contentPanel.anchoredPosition;

        while (smoothTime <= aim)
        {
            if (target == null)
                yield break;
            
            contentPanel.anchoredPosition = Vector2.Lerp(initialPos, (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position), smoothTime/aim);
            smoothTime += Time.smoothDeltaTime;
            yield return new WaitForSeconds(Time.smoothDeltaTime);
        }
        
        contentPanel.anchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
        contentPanel.MMSetLeft(0);
        contentPanel.MMSetRight(0);

        lerping = false;
        actualState = States.ONSNAP;

        yield break;
    }

    //create a function named cancellDrag that sets dragging to false sets enabled to true and sets the content panel to the nearest billboard
    public void CancelDrag()
    {
        StopCoroutine("SnapToCo");
        lerping = false;
        scrollRect.GetComponent<ScrollRect>().enabled = true;
        //this.GetComponent<ScrollRect>().velocity = Vector2.zero;
        _time = 0f;
    }

    //create a function that returns the distance between the center of the panel and a given card.
    private float DistanceFromCenter(RectTransform _billRect)
    {
        if (dir == Direction.X)
        {
            return Mathf.Abs(_billRect.position.x - center.position.x);

        }
        else if (dir == Direction.Y)
        {
            return Mathf.Abs(_billRect.position.y - center.position.y);

        }
        return 0;
    }
    
    //create a function that returns the card with the smallest distance from the center.
    private RectTransform NearestBillboard()
    {
        if (billboardRects.Count == 0)
        {
            Debug.Log("There are no billboards!");
            return null;
        }

        RectTransform nearest = billboardRects[0];
        float minDistance = DistanceFromCenter(nearest);

        for (int i = 0; i < billboardRects.Count; i++)
        {
            float distance = DistanceFromCenter(billboardRects[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = billboardRects[i];
                index = i;
            }
        }

        
        return nearest;
    }
    
    //create a function that detects the drag of the screen and snaps the card to the nearest one.
    public void EndDrag()
    {
        this.transform.GetComponent<SliderClamp>().actualState = SliderClamp.States.INERTIA;
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        float outValue = (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        //Debug.Log("Out value" + outValue);

        return outValue;
        //vida escalable con vida actual
    }

    public void DragFalse()
    {
        dragging = false;
    }

    public void DragTrue()
    {
        dragging = true;
    }

    public void ClearBillboard()
    {
        billboardRects.Clear();
    }

    public void AddBillboard(RectTransform rect)
    {
        this.gameObject.SetActive(true);

        billboardRects.Add(rect);
        //we set it to true when we add something, elsewhere we deactivated in order for better scrolling through the menu
    }
    //set the index of the card to the index of the nearest card.

    //create a function that deletes a billboard from the list.
    public void DeleteBillboard(RectTransform rect)
    {
        billboardRects.Remove(rect);
    }
}



