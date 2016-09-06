using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class MinionModel : MonoBehaviour
{
    public int indexMinion;
    public int side;
    public GameObject gameManager;
    public SockM sockM;
    public ViewManager viewM;
    public PlayMakerFSM mgrFSM;

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager") as GameObject;
        sockM = gameManager.GetComponent<SockM>() as SockM;
        viewM = gameManager.GetComponent<ViewManager>() as ViewManager;
        mgrFSM = gameManager.GetComponent<PlayMakerFSM>() as PlayMakerFSM;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseOver()
    {
        //TODO add hover preview effect
    }

    public void OnMouseUp()
    {
        if (side == 1) { return; }
        if (mgrFSM.ActiveStateName != "TurnReady") { return; }
        sockM.suffix = "/attack/" + indexMinion;
        mgrFSM.SendEvent("ChooseAction");

    }
}
