using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    
    public enum TYPE
    {
        NORMAL,
        OCA,
        BRIDGE,
        DELAY,
        SUPERDELAY,
        BACK,
        PRISON,
        DEAD
    }

    [SerializeField]
    private TYPE type;

    public TYPE GetTileType()
    {
        return type;
    }

    public void SetType(TYPE type)
    {
        this.type = type;
    }

    [SerializeField]
    private TextMeshProUGUI numberText;

    [SerializeField]
    private TextMeshProUGUI nameText;

    public int index;

    [SerializeField]
    private Image image;

    [SerializeField]
    private List<int> tileSequence;

    public void PushSequencePos(List<int> bridgeList)
    {
        foreach (int item in bridgeList)
        {
            tileSequence.Add(item);
        }
    }


    public void SetIndexText(int index)
    {
        numberText.text = index.ToString();
    }

    public void SetNameText()
    {
        nameText.text = type.ToString().ToUpper();
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }
    // Start is called before the first frame update
    void Start()
    {
        SetIndexText(ParseIndexOnlyWaypoint(transform.parent.name));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int ParseIndexOnlyWaypoint(string waypointName)
    {
        int index = int.Parse(waypointName.Substring(waypointName.LastIndexOf("t") + 1));
        Debug.Log("Returning" + index + " for name: " + waypointName);
        return index;
    }

    public int TileEffect(int actualTile)
    {
        switch (type)
        {
            case TYPE.NORMAL:
                
                return 1;
            case TYPE.OCA:
                if (tileSequence.Contains(actualTile))
                {
                    int index = tileSequence.IndexOf(actualTile);
                    if (tileSequence[index] == 62)
                    {
                        return 1;
                    }
                    else
                    {
                        return tileSequence[index + 1];
                    }
                }
                else
                {
                    return 1;
                }
            case TYPE.BRIDGE:
                if (tileSequence.Contains(actualTile))
                {
                    int index = tileSequence.IndexOf(actualTile);
                    if (index == 0)
                    {
                        return tileSequence[1];
                    }
                    else
                    {
                        return tileSequence[0];
                    }
                }
                else
                {
                    return 1;
                }
            case TYPE.DELAY:
                
                return -1;
            case TYPE.SUPERDELAY:
                
                return -2;
            case TYPE.BACK:
                
                return 0;
            case TYPE.PRISON:
                
                return -2;
            case TYPE.DEAD:
                
                return -100;
            default:
                return 100;
        }
    }

}
