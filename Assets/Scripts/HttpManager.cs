﻿using UnityEngine;
using System.Collections;
using BestHTTP;
using UnityEngine.UI;
using HutongGames.PlayMaker;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Xml.Linq;
using BestHTTP.SocketIO;



public class HttpManager : MonoBehaviour {
	public string ServerAddress;
	public Text text;
	public PlayMakerFSM GameMFSM;
	public string URLSuffix;
    string respText;
    public CardsRepo cards;
	// Use this for initialization
	void Start () {
//		var manager = new SocketManager(new System.Uri("http://192.168.4.29:5000/socket.io/"));
//		Socket root = manager.Socket;
//        TextAsset asset = Resources.Load("card-info") as TextAsset;
//		dynamic stuff = JsonConvert.DeserializeObject(asset.text);
//		Debug.Log(stuff);
//        cards = JsonConvert.DeserializeObject<CardsRepo>(asset.text);

//        TextAsset asset = Resources.Load("cgRes") as TextAsset;
//		var deserialzedData = JsonConvert.DeserializeObject<Resp>(asset.text);
//        Debug.Log("Deserialized:" + deserialzedData);
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

    public void SendRequest(string suffix)
    {
        HTTPRequest request = new HTTPRequest(new System.Uri(ServerAddress + "/" + suffix), OnGetFinished);
        request.Send();
    }


	public void OnGetFinished(HTTPRequest request, HTTPResponse response){
		respText = response.DataAsText;

		Debug.Log("Request Finished! Text received: " + response.DataAsText);
		text.text = "Request Finished! Text received: " + response.DataAsText;
		if (respText != null){
			GameMFSM.SendEvent("NetSuccess");

		}
	}

	void parseJson(string jsonData){
		
	}

}

class HSGame
{

}

public class CardsRepo
{
    public List<Card> lushi;
}

public class Card
{
    public string image;
    public int set;
    public int quality;
    public int type;
    public int cost;
    public int attack;
    public int health;
    public string name;
    public string cnname;
    public string description;
    public string cndescrition;
}

public class TestData
{
    public string PlayerName { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public override string ToString()
    {
        return string.Format("PlayerName={0}, Level={1}, Exp={2}", PlayerName, Level, Exp);
    }
}

public class Resp{
	public int result;
	public string game;
}