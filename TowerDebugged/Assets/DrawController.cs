using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawController : MonoBehaviour
{

    private Camera _cam;
    [SerializeField] private Line _linePrefab;
    public GameObject parent;

    public const float RESOLUTION = .1f;
    public GameObject gc;

    private Line _currentLine;

    public Line CurrentLine { get => _currentLine; set => _currentLine = value; }

    private static DrawController drawInstance;

    public static DrawController MyDrawInstance
    {
        get
        {
            if (drawInstance == null)
            {
                drawInstance = FindObjectOfType<DrawController>();
            }
            return drawInstance;
        }


    }

    void Start()
    {
        _cam = Camera.main;
        gc = GameObject.FindWithTag("GameController");
    }


    void Update()
    {
        //if (gc.GetComponent<skillController>().casting == true)
        //{
        //    Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        //    Debug.Log("Mouse Pos X:" + mousePos.x + "Mouse Pos Y:" + mousePos.y + "Mouse Pos Z:" + mousePos.z);

        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        CurrentLine = Instantiate(_linePrefab, mousePos, Quaternion.identity);
        //        CurrentLine.GetComponent<LineRenderer>().useWorldSpace = false;
        //        CurrentLine.gameObject.transform.SetParent(parent.transform);
        //        CurrentLine.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //    }

        //    if (Input.GetMouseButtonDown(0))
        //        CurrentLine.SetPosition(mousePos);
        //}
    }

    public void CleanUp()
    {
        //Debug.Log("CleaningUp");
        //Debug.Log(parent.transform.childCount);
        //if (parent.transform.childCount > 0)
        //{
        //    for (int i = 0; i < parent.transform.childCount; i++)
        //    {
        //        Destroy(parent.transform.GetChild(i).gameObject);
        //    }
        //}
    }
   
}
