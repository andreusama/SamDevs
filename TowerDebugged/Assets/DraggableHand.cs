using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.ParticleSystem;

public class DraggableHand : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IInitializePotentialDragHandler
{
    public bool dragging = false;
    Touch actualTouch = new Touch();

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;
        Debug.Log("Starting hand drag!");
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        if(skillController.MySkillInstance.casting == true)
        {
            skillController.MySkillInstance.FailedSummon();
        }
        Debug.Log("Finishing hand drag");
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        //start the cast
        dragging = true;
        Debug.Log("Checking Potential Drag!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("On trigger entered!");
        if (collision.transform.GetComponent<spellInteracter>() != null)
        {
            collision.transform.GetComponent<spellInteracter>().Inside(false, dragging);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<spellInteracter>() != null)
        {
            collision.transform.GetComponent<spellInteracter>().Outside(dragging);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Check here if inside or Exit!;
        if (collision.transform.GetComponent<spellInteracter>() != null)
        {
            collision.transform.GetComponent<spellInteracter>().Inside(false, dragging);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
        if (skillController.MySkillInstance.casting == true)
        {
            if (Input.touchCount > 0)
                switch (Input.GetTouch(0).phase)
                {
                    case TouchPhase.Began:
                        Debug.Log("Touch began!");
                        dragging = true;
                        break;
                    case TouchPhase.Moved:
                        Debug.Log("Touch moved!");
                        dragging = true;
                        break;
                    case TouchPhase.Stationary:
                        Debug.Log("Touch stationary!");
                        dragging = true;
                        break;
                    case TouchPhase.Ended:
                        Debug.Log("Touch ended!");
                        if (skillController.MySkillInstance.casting == true)
                        {
                            skillController.MySkillInstance.FailedSummon();
                        }
                        dragging = false;
                        break;
                    case TouchPhase.Canceled:
                        Debug.Log("Touch canceled!");
                        if (skillController.MySkillInstance.casting == true)
                        {
                            skillController.MySkillInstance.FailedSummon();
                        }
                        dragging = false;
                        break;
                    default:
                        break;
                }
        }
    }
}
