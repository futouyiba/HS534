using UnityEngine;
using System.Collections;

public class DragableCard : MonoBehaviour
{
    public float distance = 8;
    public bool inPlacingState = false;
    [SerializeField]
    GameObject placeManager;
    PlaceManager mgr;
    CardModel cardModel;
    public Transform handCardOriginPos;
    // Use this for initialization
    void Start()
    {
        mgr = placeManager.GetComponent<PlaceManager>();
        cardModel = gameObject.GetComponent<CardModel>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = objPosition;
        if (inPlacingState)
        {
            mgr.CardDragMove();
        }
    }
    void OnMouseUp()
    {
        if (inPlacingState)
        {
            mgr.ConfirmPlaceCard(cardModel);
        }
        else
        {
            inPlacingState = false;
            transform.position = new Vector3(0.3f, -0.5f, -2.55f);
            //TODO after we have handcard position list , this position reset should be done.
   //         gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    // OnMouseOver is called every frame while the mouse is over the GUIElement or Collider
    public void OnMouseOver()
    {

    }

    // OnMouseEnter is called when the mouse entered the GUIElement or Collider
    public void OnMouseEnter()
    {

    }

    // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
    public void OnMouseDown()
    {
        //      gameObject.GetComponent<BoxCollider>().enabled = true;
        placeManager.GetComponent<BoxCollider>().enabled = true;
    }

    // OnMouseExit is called when the mouse is not any longer over the GUIElement or Collider
    public void OnMouseExit()
    {

    }

    // This function is called when the MonoBehaviour will be destroyed
    public void OnDestroy()
    {

    }
}
