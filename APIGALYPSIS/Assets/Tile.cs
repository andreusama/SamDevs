using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public List<Sprite> tileSprites;
    public Image complementImage;
    public TextMeshProUGUI complementText;

    public GameObject FX;
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

    public void SetSprite()
    {
        switch (type)
        {
            case TYPE.NORMAL:
                image.sprite = tileSprites[0];
                complementImage.gameObject.SetActive(false);
                break;
            case TYPE.OCA:
                image.sprite = tileSprites[1];
                complementImage.gameObject.SetActive(false);
                break;
            case TYPE.BRIDGE:
                image.sprite = tileSprites[5];
                complementImage.gameObject.SetActive(false);
                break;
            case TYPE.DELAY:
                image.sprite = tileSprites[1];
                complementText.text = "1";
                complementImage.sprite = tileSprites[2];
                break;
            case TYPE.SUPERDELAY:
                image.sprite = tileSprites[1];
                complementText.text = "3";
                complementImage.sprite = tileSprites[2];
                break;
            case TYPE.BACK:
                image.sprite = tileSprites[3];
                complementImage.gameObject.SetActive(false);
                break;
            case TYPE.PRISON:
                image.sprite = tileSprites[1];
                complementText.text = "3";
                complementImage.sprite = tileSprites[2];
                break;
            case TYPE.DEAD:
                image.sprite = tileSprites[4];
                complementImage.gameObject.SetActive(false);
                break;
            default:
                break;
        }
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
                    if (tileSequence[index] == 62 || tileSequence[index] == 0)
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
                
                return -3;
            case TYPE.BACK:
                
                return 0;
            case TYPE.PRISON:
                
                return -3;
            case TYPE.DEAD:
                
                return -100;
            default:
                return 100;
        }
    }

}
