using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexController : MonoBehaviour {

    // Use this for initialization
    Transform position;
    private GameObject gc;
 

    void Start ()
    {
        gc = GameObject.FindWithTag("GameController");
        position = this.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseOver()
    {
        if(gc.GetComponent<buildController>().build == true)
        {
            if (gc.GetComponent<shopController>().destroyMode == true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    gc.GetComponent<buildController>().hexList.Remove(position.position);
                    Destroy(this.gameObject);
                }
            }
            /*
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //error, pasando la posicion del hex i no la de los hexs colindantes
                    gc.GetComponent<gameController>().createSelectionTiles(position.position);
                    
                }
            }
            */
        }
    }
}
