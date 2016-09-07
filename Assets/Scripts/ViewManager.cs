using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Linq;
using System.Linq;


public class ViewManager : MonoBehaviour
{
    [SerializeField]
    List<Button> minionSlotButtons;
    [SerializeField]
    List<Transform> oppSlotPos;
    [SerializeField]
    List<Transform> SlotPos;
    public List<GameObject> handCards;
    public List<GameObject> minions;
    public List<GameObject> oppMinions;
    public List<GameObject> oppHandCards;
    [SerializeField]
    GameObject minionPrefab;
    [SerializeField]
    GameObject handcardPrefab;
    public XDocument doc;
    public string nameMatched;
    public Response res;
    [SerializeField]
    Vector2 textureScale;
    [SerializeField]
    Vector2 textureOffset;
    [SerializeField]
    Vector3 handCardOffset;
    [SerializeField]
    Transform handCardOrigin;
    [SerializeField]
    Transform oppHandCardOrigin;


    // Use this for initialization
    void Start()
    {
        TextAsset cardXMLFile = Resources.Load("card-XML") as TextAsset;
        //Debug.Log(cardXMLFile.text);
        doc = XDocument.Parse(cardXMLFile.text);
        res = gameObject.GetComponent<SockM>().res;
    }

    public string GetImgFromName(string nameMatched)
    {
        var all = from item in doc.Element("lushi").Elements()
                  where item.Element("name").Value.Equals(nameMatched)
                  select new
                  {
                      name = item.Element("name"),
                      image = item.Element("image"),
                  };
        foreach (var card in all)
        {
            return card.image.Value;
        }
        return "image not found!";
    }

    //public var GetCardInfoFromName(string nameMatched)
    //{
    //    var all = from item in doc.Element("lushi").Elements()
    //              where item.Element("name").Value.Equals(nameMatched)
    //              select new Dictionary<string>
    //              {
    //                  name = item.Element("name"),
    //                  image = item.Element("image"),
    //                  set = item.Element("set"),
    //                  quality = item.Element("quality"),
    //                  type = item.Element("type"),
    //                  cost = item.Element("cost"),
    //                  attack = item.Element("attack"),
    //                  health = item.Element("health"),
    //                  cnname = item.Element("cnname"),
    //                  cndescription = item.Element("cndescription"),

    //              };
    //    foreach (var card in all)
    //    {
    //        return card;
    //    }
    //    return "image not found!";
    //}

    // Update is called once per frame
    void Update()
    {

    }
    public void ShowGameView()
    {
        ShowHandCards();
        ShowMinions();
        UpdateHeroHealth();
        HideSlotButtons();
        return;
    }

    public void UpdateHeroHealth()
    {
        GameObject.FindWithTag("MyHeroHealth").GetComponent<UILabel>().text = res.game.players[0].hero.health.ToString();
        GameObject.FindWithTag("EnemyHeroHealth").GetComponent<UILabel>().text = res.game.players[1].hero.health.ToString();

    }

    public void EraseHandCards()
    {
        foreach (var card in handCards)
        {
            handCards.Remove(card);
            Destroy(card);
        }
        foreach (var cardOpp in oppHandCards)
        {
            oppHandCards.Remove(cardOpp);
            Destroy(cardOpp);
        }
    }
    public void ShowHandCards()
    {
        EraseHandCards();
        Hand[] handInfo = res.game.players[0].hand;
        var originPos = handCardOrigin.position;
        int len = handInfo.Length;
        for (int i = 0; i < len; i++)
        {
            var handCard = Instantiate(handcardPrefab, originPos+i*handCardOffset, Quaternion.identity) as GameObject;
            SetCardFromInfo(handCard, handInfo[i].name);
            handCards.Add(handCard);
            handCard.GetComponent<CardModel>().indexCard = i;
            handCard.GetComponent<CardModel>().side = 0;

        }
        Hand[] oppCardsInfo = res.game.players[1].hand;
        int oppLen = oppCardsInfo.Length;
        for (int i = 0; i < oppLen; i++)
        {
            var oppHandCard = Instantiate(handcardPrefab, originPos + i * handCardOffset, Quaternion.identity) as GameObject;
            SetCardFromInfo(oppHandCard, oppCardsInfo[i].name);
            oppHandCards.Add(oppHandCard);
            oppHandCard.GetComponent<CardModel>().side = 1;
            oppHandCard.GetComponent<CardModel>().indexCard = i;

        }
    }
    public void SetCardFromInfo(GameObject card, string cardName)
    {
        card.transform.FindChild("pic").gameObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find(" Diffuse"));
        var mat = card.transform.FindChild("pic").gameObject.GetComponent<MeshRenderer>().material;
        string imgName = GetImgFromName(cardName);
        mat.mainTexture = (Resources.Load("/card_pics/" + imgName) as Texture2D);
    }

    public void ShowMinions()
    {
        EraseMinions();
        Minion[] minionsInfo = res.game.players[0].minions;
        int len = minionsInfo.Length;
        for (int i = 0; i < len; i++)
        {
            var minion = Instantiate(minionPrefab, SlotPos[i].position, Quaternion.identity) as GameObject;
            SetMinionAttrFromInfo(minion, minionsInfo[i]);
            minions.Add(minion);
            minion.GetComponent<MinionModel>().indexMinion = i;
        }
        Minion[] oppMinionsInfo = res.game.players[1].minions;
        int oppLen = oppMinionsInfo.Length;
        for (int i = 0; i < oppLen; i++)
        {
            var oppMinion = Instantiate(minionPrefab, oppSlotPos[i].position, Quaternion.identity) as GameObject;
            SetMinionAttrFromInfo(oppMinion, oppMinionsInfo[i]);
            oppMinions.Add(oppMinion);
        }
    }
    public void SetMinionAttrFromInfo(GameObject minion, Minion minionInfo)
    {
        minion.transform.FindChild("attack").GetComponent<UILabel>().text = minionInfo.attack.ToString();
        minion.transform.FindChild("health").GetComponent<UILabel>().text = (minionInfo.max_health - minionInfo.damage).ToString();
        minion.transform.FindChild("pic").GetComponent<MeshRenderer>().material = new Material(Shader.Find(" Diffuse"));
        var mat = minion.transform.FindChild("pic").GetComponent<MeshRenderer>().material;
        string imgName = GetImgFromName(minionInfo.name);
        mat.mainTexture = (Resources.Load("/card_pics/" + imgName) as Texture2D);
        mat.SetTextureScale("Tiling", textureScale);
        mat.SetTextureOffset("Offset", textureOffset);
        if (minionInfo.exhausted)
        {
            minion.transform.FindChild("lz").gameObject.SetActive(true);
        }
    }

    void EraseMinions()
    {
        foreach (var minion in minions)
        {
            minions.Remove(minion);
            Destroy(minion);
        }
        foreach (var minion in oppMinions)
        {
            oppMinions.Remove(minion);
            Destroy(minion);
        }

    }

    public void ShowSlotButtons()
    {
        int num = minions.Count;
        for (int i = 0; i < num; i++)
        {
            minionSlotButtons[i].enabled = true;
        }
    }
    public void HideSlotButtons()
    {
        foreach (var slotButton in minionSlotButtons)
        {
            slotButton.enabled = false;
        }
    }
}
