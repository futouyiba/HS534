using UnityEngine;
using System.Collections;
using BestHTTP;
using UnityEngine.UI;
using HutongGames.PlayMaker;


public class HttpManager : MonoBehaviour {
	public string ServerAddress;
	public Text text;
	public PlayMakerFSM GameMFSM;
	public string URLSuffix;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void GetFromServer(){
		HTTPRequest request = new HTTPRequest (new System.Uri (ServerAddress+"/"+URLSuffix), OnGetFinished);
		request.Send();
	}

	public void SendNewGame(){
		HTTPRequest request = new HTTPRequest (new System.Uri (ServerAddress + "/newgame"), OnGetFinished);
		request.Send();
	}

	public void OnGetFinished(HTTPRequest request, HTTPResponse response){
		string respText ;
		respText = response.DataAsText;
		Debug.Log("Request Finished! Text received: " + response.DataAsText);
//		text.text = "Request Finished! Text received: " + response.DataAsText;
		if (respText == "1"){
			GameMFSM.SendEvent ("NetSuccess");

		}
	}

	void parseJson(string jsonData){
		
	}

}
