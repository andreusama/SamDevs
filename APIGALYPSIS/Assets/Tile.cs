using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class Tile : MonoBehaviour
{
    public List<Sprite> tileSprites;
    public Image complementImage;
    public TextMeshProUGUI complementText;
    public GameObject complement;

    public GameObject FX;

    public bool deadTile = false;
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

    public Image pigVisuals;
    
    public Image SetSprite(Sprite sprite, bool deleteDeco)
    {
        if (deleteDeco == true)
        {
            complement.SetActive(false);
            pigVisuals.gameObject.SetActive(false);
        }
        
        image.sprite = sprite;
        return image;
    }

    [SerializeField]
    private List<int> tileSequence;

    public MMFeedbacks tileFeedback;

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

    public void DisableSprite()
    {
        image.sprite = null;
    }
    public void SetSprite()
    {
        switch (type)
        {
            case TYPE.NORMAL:
                image.sprite = tileSprites[0];
                complement.gameObject.SetActive(false);
                break;
            case TYPE.OCA:
                if (index == 62)
                {
                    image.sprite = tileSprites[1];
                    complementText.text = "";
                    complement.gameObject.SetActive(true);
                    pigVisuals.gameObject.SetActive(true);
                }
                else
                {
                    image.sprite = tileSprites[1];
                    complement.gameObject.SetActive(false);
                }
                break;
            case TYPE.BRIDGE:
                image.sprite = tileSprites[5];
                complement.gameObject.SetActive(false);
                break;
            case TYPE.DELAY:
                image.sprite = tileSprites[1];
                complementText.text = "1";
                complement.gameObject.SetActive(true);
                complementImage.sprite = tileSprites[2];
                break;
            case TYPE.SUPERDELAY:
                image.sprite = tileSprites[1];
                complementText.text = "3";
                complement.gameObject.SetActive(true);
                complementImage.sprite = tileSprites[2];
                break;
            case TYPE.BACK:
                image.sprite = tileSprites[3];
                complement.gameObject.SetActive(false);
                break;
            case TYPE.PRISON:
                image.sprite = tileSprites[1];
                complementText.text = "3";
                complement.gameObject.SetActive(true);
                complementImage.sprite = tileSprites[2];
                break;
            case TYPE.DEAD:
                deadTile = true;
                image.sprite = tileSprites[4];
                complement.gameObject.SetActive(false);
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

    public void PlayTileFeedback()
    {
        tileFeedback.PlayFeedbacks();
    }
    public int TileEffect(int actualTile, TextMeshProUGUI infoText)
    {
        switch (type)
        {
            case TYPE.NORMAL:
                infoText.text = "NORMAL TILE";
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
                        infoText.text = "JUMP! TILE";
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
                    infoText.text = "BRIDGE TILE";
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
                infoText.text = "THE FARMERS TRAPPED YOU!";
                return -1;
            case TYPE.SUPERDELAY:
                infoText.text = "THE FARMERS TRAPPED YOU";
                return -3;
            case TYPE.BACK:
                infoText.text = "THE WATER CURRENT IS TOO STRONG...";
                return 0;
            case TYPE.PRISON:
                infoText.text = "THE FARMERS TRAPPED YOU";
                return -3;
            case TYPE.DEAD:
                infoText.text = "DEAD TILE";
                return -100;
            default:
                return 100;
        }
    }

}
