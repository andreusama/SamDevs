using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class TheHandFeedback : MonoBehaviour
{
    public GameObject spellInteractablePrefab;
    public Transform position1;
    public Transform position2;
    public MMFeedbacks startFeedback;

    public List<GameObject> actualDemoInteracters = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TriggerStartFeedback()
    {
        startFeedback.PlayFeedbacks();
    }
    //create a method that moves the rect attached to this script to the position of the aimTarget, have into account that they may have different relative positions
    public void MoveHand(RectTransform aimTarget)
    {
        //Debug.Log("Moving Hand");
        //Debug.Log("Aim Target Position: " + aimTarget.position);
        //Debug.Log("Hand Position: " + transform.position);
    }
    
    public void LearnDragPhase(bool delete)
    {
        if (delete)
        {
            int correctCompro = 0;
            foreach (var item in actualDemoInteracters)
            {
                if (item.GetComponent<spellInteracter>().GetPressed() == true)
                {
                    correctCompro++;
                }
            }
            if (correctCompro == 2)
            {
                TutorialManager.Instance.NextPhase(TutorialManager.GAMEPLAY_TUTORIAL_PHASE.SIGNING);
                
            }
            
            correctCompro = 0;
            foreach (var item in actualDemoInteracters.ToList())
            {
                actualDemoInteracters.Remove(item);

                GameObject.Destroy(item.gameObject);
            }
        }
        else
        {
            actualDemoInteracters.Add(skillController.MySkillInstance.InstantiateInteracter(position1, new Vector3(1f, 1f, 1f), true));
            actualDemoInteracters[0].transform.GetComponent<spellInteracter>().Countdown();

            actualDemoInteracters.Add(skillController.MySkillInstance.InstantiateInteracter(position2, new Vector3(1f, 1f, 1f), true));
        }
    }
    //use the collider 2D to detect collision with other collider 2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<spellInteracter>() != null)
        {
            collision.gameObject.GetComponent<spellInteracter>().insideFeedback.PlayFeedbacks();
            collision.gameObject.GetComponent<spellInteracter>().circle.gameObject.SetActive(false);
            collision.gameObject.GetComponent<spellInteracter>().debugPressed = true;
            actualDemoInteracters[1].transform.GetComponent<spellInteracter>().Countdown();

        }
        //Debug.Log("Collision Detected");
        //Debug.Log("Collision Detected with: " + collision.gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<spellInteracter>() != null)
        {
            collision.gameObject.GetComponent<spellInteracter>().outsideFeedback.PlayFeedbacks();
        }
        //Debug.Log("Collision Detected");
        //Debug.Log("Collision Detected with: " + collision.gameObject.name);
    }

    
}
