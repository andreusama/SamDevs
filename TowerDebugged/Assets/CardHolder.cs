using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Feel;
using MoreMountains.Tools;
using TMPro;
using UnityEngine.UI;

public class CardHolder : MonoBehaviour
{
    private GameObject gc;

    public MMFeedbacks feedback;

    private Card card;

    [SerializeField]
    private TextMeshProUGUI cardName;
    [SerializeField]
    private TextMeshProUGUI statText;
    [SerializeField]
    private TextMeshProUGUI definitionText;
    [SerializeField]
    private TextMeshProUGUI quote;
    [SerializeField]
    private Image backGround;
    [SerializeField]
    private Image backgroundDeco;
    [SerializeField]
    private Image art;
    [SerializeField]
    private Image rareness;
    [SerializeField]
    private List<Image> overlays;
    [SerializeField]
    public GameObject highlight;
    [SerializeField]
    public GameObject KingUiFeedback;
    [SerializeField]
    public GameObject CrownButton;

    [SerializeField]
    private Image illustration;


    // Start is called before the first frame update
    private void Awake()
    {
        gc = GameObject.FindWithTag("GameController");
    }
    void Start()
    {
        feedback.Initialization();
        statText.text = "+" + card.damage.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Card GetCard()
    {
        return card;
    }
    public void SetCard(Card newCard)
    {
        card = newCard;
        
        quote.text = card.GetQuote();
        SetStatText();
        SetPasiveText();
        SetRareness(((int)card.rareness));
    }

    public void SetAwait(bool set)
    {
        gc.GetComponent<KingController>().SetAwait(set);
    }

    public void SetCrowned()
    {
        gc.GetComponent<KingController>().SafeCrowning(this);
        highlight.SetActive(true);
        KingUiFeedback.SetActive(true);
        CrownButton.SetActive(false);
    }

    private void SetStatText()
    {
        foreach (var item in card.stats)
        {
            statText.text = "+" + item.amount;
        }
    }

    private void SetPasiveText()
    {
        definitionText.text = card.GetDefinition();
    }

    private void SetRareness(int rareness)
    {
        switch (rareness)
        {
            case 0:
                backGround.color = gc.GetComponent<KingController>().rarenessColors[0];
                backgroundDeco.color = gc.GetComponent<KingController>().rarenessColorsDeco[0];
                illustration.sprite = KingController.MyKingInstance.GetRandomSprite(0);
                cardName.text = KingController.GetRandomName(3, 0);
                for (int i = 0; i < overlays.Count; i++)
                {
                    overlays[i].color = gc.GetComponent<KingController>().overlaysColors[0];
                }
                break;
            case 1:
                backGround.color = gc.GetComponent<KingController>().rarenessColors[1];
                backgroundDeco.color = gc.GetComponent<KingController>().rarenessColorsDeco[1];
                illustration.sprite = KingController.MyKingInstance.GetRandomSprite(1);
                cardName.text = KingController.GetRandomName(3, 1);
                for (int i = 0; i < overlays.Count; i++)
                {
                    overlays[i].color = gc.GetComponent<KingController>().overlaysColors[1];
                }
                break;
            case 2:
                backGround.color = gc.GetComponent<KingController>().rarenessColors[2];
                backgroundDeco.color = gc.GetComponent<KingController>().rarenessColorsDeco[2];
                illustration.sprite = KingController.MyKingInstance.GetRandomSprite(2);
                cardName.text = KingController.GetRandomName(5, 2);
                for (int i = 0; i < overlays.Count; i++)
                {
                    overlays[i].color = gc.GetComponent<KingController>().overlaysColors[2];
                }
                break;
            case 3:
                backGround.color = gc.GetComponent<KingController>().rarenessColors[3];
                backgroundDeco.color = gc.GetComponent<KingController>().rarenessColorsDeco[3];
                illustration.sprite = KingController.MyKingInstance.GetRandomSprite(3);
                cardName.text = KingController.GetRandomName(2, 3);
                for (int i = 0; i < overlays.Count; i++)
                {
                    overlays[i].color = gc.GetComponent<KingController>().overlaysColors[3];
                }
                break;
            case 4:
                backGround.color = gc.GetComponent<KingController>().rarenessColors[4];
                backgroundDeco.color = gc.GetComponent<KingController>().rarenessColorsDeco[4];
                illustration.sprite = KingController.MyKingInstance.GetRandomSprite(4);
                cardName.text = KingController.GetRandomName(7, 4);
                for (int i = 0; i < overlays.Count; i++)
                {
                    overlays[i].color = gc.GetComponent<KingController>().overlaysColors[4];
                }
                break;
            default:
                break;
        }
    }

    public void Hire()
    {
        LevelTraveler.MyTravelInstance.Gear.contract.AddBaseDamage(card.damage);
        Debug.Log("My name is" + cardName.text);

        MenuUI.MyMenuUiInstance.UpdateContractInfo();
        //make it destroy himself
        KingController.MyKingInstance.cardSlider.DeleteBillboard((RectTransform)gameObject.transform);
        Destroy(gameObject);
    }
}
