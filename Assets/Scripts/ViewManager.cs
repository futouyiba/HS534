using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Linq;
using System.Linq;


public class ViewManager : MonoBehaviour {
    [SerializeField]
    List<Button> minionSlots;
    [SerializeField]
    List<Transform> oppinionSlotPos;
    [SerializeField]
    List<Transform> SlotPos;

    public XDocument doc;
    public string nameMatched;

    // Use this for initialization
    void Start () {
        TextAsset cardXMLFile = Resources.Load("card-XML") as TextAsset;
        //Debug.Log(cardXMLFile.text);
        doc = XDocument.Parse(cardXMLFile.text);
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
	
	// Update is called once per frame
	void Update () {
	
	}
}
