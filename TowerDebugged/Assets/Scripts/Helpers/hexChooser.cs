using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hexChooser : MonoBehaviour {

    private string nombre;
    public GameObject gc;

    public GameObject forestTile;
    public GameObject riverTile;
    public GameObject lavaTile;
    // Use this for initialization
    void Start ()
    {
        nombre = this.gameObject.name;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnMouseOver()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Debug.Log(nombre);
        //    if(nombre == "lavaSelection(Clone)")
        //    {
        //        gc.GetComponent<selectionHex>().buildHex(lavaTile);
        //    }
        //    if(nombre == "waterSelection(Clone)")
        //    {
        //        gc.GetComponent<selectionHex>().buildHex(riverTile);
        //    }
        //    if(nombre == "forestSelection(Clone)")
        //    {
        //        gc.GetComponent<selectionHex>().buildHex(forestTile);
        //    }
        //}
    }
}
