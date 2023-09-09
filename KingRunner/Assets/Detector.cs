using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    //create both a function to detect on trigger enter and on trigger exit
    private void OnTriggerEnter(Collider other)
    {
        //if the object that enters the trigger is tagged as "Player"
        if (other.CompareTag("Tile"))
        {
            //set the player's position to the position of the detector
            Debug.Log("Enter and Collided!");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //if the object that exits the trigger is tagged as "Player"
        if (other.CompareTag("Tile"))
        {
            //set the player's position to the position of the detector
            Debug.Log("Exit and Collided!");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
