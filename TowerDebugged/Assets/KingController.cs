using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class KingController : MonoBehaviour
{
    private static KingController kingInstance;

    public static KingController MyKingInstance
    {
        get
        {
            if (kingInstance == null)
            {
                kingInstance = FindObjectOfType<KingController>();
            }
            return kingInstance;
        }


    }

    [SerializeField]
    private GameObject scrollParent;
    [SerializeField]
    private GameObject cardPrefab;

    public List<GameObject> cardHolders;

    [SerializeField]
    private List<Card> cards;

    public List<Color> rarenessColors;
    public List<Color> rarenessColorsDeco;
    public List<Color> overlaysColors;

    public CardHolder lastKing = null;
    public CardHolder actualKing = null;

    private bool await;
    private bool dir;

    private int slidePosition;

    public SliderClamp cardSlider = null;

    private CardHolder actualCard = null;

    [SerializeField]
    private List<Sprite> commonSprites;

    [SerializeField]
    private List<Sprite> rareSprites;

    [SerializeField]
    private List<Sprite> uncommonSprites = new List<Sprite>();

    [SerializeField]
    private List<Sprite> epicSprites = new List<Sprite>();

    [SerializeField]
    private List<Sprite> legendarySprites = new List<Sprite>();



    // Start is called before the first frame update
    void Start()
    {
        await = true;
        slidePosition = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddCard(cards[0]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddCard(cards[1]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddCard(cards[2]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AddCard(cards[3]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AddCard(cards[4]);
        }
    }

    private void AddCard(Card card)
    {
        Debug.Log(card.GetName());
        GameObject newCard = Instantiate(cardPrefab, scrollParent.transform);
        //modify the scale of the card to 0.9
        newCard.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        newCard.transform.GetComponent<CardHolder>().SetCard(card);
        cardHolders.Add(newCard);
        cardSlider.AddBillboard(newCard.GetComponent<RectTransform>());
    }

    public void SafeCrowning(CardHolder cardHolder)
    {
        if(actualKing == null)
        {
            actualKing = cardHolder;
        }

        if (actualKing == cardHolder)
            return;

        if(actualKing != null)
        {
            lastKing = actualKing;
            lastKing.GetCard().SetIsKing(false);
            lastKing.highlight.SetActive(false);
            lastKing.KingUiFeedback.SetActive(false);
            lastKing.CrownButton.SetActive(true);
            actualKing = cardHolder;
        }
    }

    public Sprite GetRandomSprite(int rareness)
    {
        //create a switch case for each rareness
        switch (rareness)
        {
            case 0:
                return commonSprites[Random.Range(0, commonSprites.Count)];
            case 1:
                return rareSprites[Random.Range(0, rareSprites.Count)];
            case 2:
                return uncommonSprites[Random.Range(0, uncommonSprites.Count)];
            case 3:
                return epicSprites[Random.Range(0, epicSprites.Count)];
            case 4:
                return legendarySprites[Random.Range(0, legendarySprites.Count)];

            default:
                return commonSprites[Random.Range(0, commonSprites.Count)];
        }
    }

    public void SetRareness(Image image, Image deco, int rareness)
    {
        switch (rareness)
        {
            case 0:
                image.color = rarenessColors[0];
                deco.color = rarenessColorsDeco[0];
                break;
            case 1:
                image.color = rarenessColors[1];
                deco.color = rarenessColorsDeco[1];
                break;
            case 2:
                image.color = rarenessColors[2];
                deco.color = rarenessColorsDeco[2];
                break;
            case 3:
                image.color = rarenessColors[3];
                deco.color = rarenessColorsDeco[3];
                break;
            case 4:
                image.color = rarenessColors[4];
                deco.color = rarenessColorsDeco[4];
                break;
            default:
                break;
        }
    }

    public void SelectCard()
    {
        //get the nearest billboard
        RectTransform nearestBillboard = cardSlider.nearestGetter;
        //get the levelholder from the nearest billboard
        actualCard = nearestBillboard.transform.GetComponent<CardHolder>();

    }

    public static string GetRandomName(int len, int rareness)
    {
        //create a basic name generator
        int r = 0;
        string[] consonants = { "r" };
        //create a switch that gets rareness and changes the name length
        switch (rareness)
        {
            case 0:
                consonants = new string[] { "r" };
                break;
            case 1:
                consonants = new string[] { "t", "k"};
                break;
            case 2:
                consonants = new string[] { "r" };
                break;
            case 3:
                consonants = new string[] {"l"};
                break;
            case 4:
                consonants = new string[] { "g" };
                break;
            default:
                break;
        }

        string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
        string Name = "";
        Name += consonants[Random.Range(0, consonants.Count())].ToUpper();
        Name += vowels[Random.Range(0, vowels.Count())];
        int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
        while (b < len)
        {
            Name += consonants[Random.Range(0, consonants.Count())];
            b++;
            Name += vowels[Random.Range(0, vowels.Count())];
            b++;
        }

        return Name;
    }

    public void Crown()
    {

    }
    
    public void CreateWorkers()
    {
        
    }
    
    public void SetAwait(bool set)
    {
        await = set;
    }

    public bool GetAwait()
    {
        return await;
    }

    public void DTGtoCards(float DamageToGet, int iteration)
    {
        if (iteration == -1)
        {
            return;
        }
        //damage to get is the damage we must add to our contract in order for the next level to be balanced
        //the logic is the next one: a switch will be done with the damage of each card. Starting from the bigger one to the smaller one
        //if the substraction of this damage to the damage to get is bigger than 0, then we add this card to the list of cards to be instantiated
        //we'll do that recursivelly until the damage to get is 0 or less than the damage of the weakest card
        //do that
        while (DamageToGet > cards[iteration].damage)
        {
            Debug.Log("Iteration n*" + iteration);
            AddCard(cards[iteration]);

            Debug.Log("Damage to was modified from " + DamageToGet + " to ");

            DamageToGet -= cards[iteration].damage;

            Debug.Log("New VALUE: " + DamageToGet);
        }

        DTGtoCards(DamageToGet, iteration - 1);
    }

}
