using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(ScrollRect))]
public class DragController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //create all the variables needed
    private bool _underInertia = false;
    private float _time = 0.0f;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _curPosition = Vector3.zero;
    private float _smoothTime = 0.5f;
    private ScrollRect _scrollRect;

    void Update()
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //to be able to re-drag when it's under inertia
        //if (this.transform.GetComponent<SliderClamp>().actualState == SliderClamp.States.SNAP || this.transform.GetComponent<SliderClamp>().actualState == SliderClamp.States.INERTIA)
        //{
        //    Debug.Log("Cancelling on state:" + this.transform.GetComponent<SliderClamp>().actualState);
        //    this.transform.GetComponent<SliderClamp>().CancelDrag();
        //    //return;
        //}
        //Get the absolute values of the x and y differences so we can see w$$anonymous$$ch one is bigger and scroll the other scroll rect accordingly
        float horizontal = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
        float vertical = Mathf.Abs(eventData.position.y - eventData.pressPosition.y);
        this.transform.GetComponent<SliderClamp>().actualState = SliderClamp.States.DRAGGING;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.GetComponent<SliderClamp>().EndDrag();
    }

    //IDragHandler
    public void OnDrag(PointerEventData eventData)
    {
        
    }


}
