using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Text;
using Newtonsoft.Json;
using HutongGames.PlayMaker;

public class SockM : MonoBehaviour
{
    public string ipString;
    public int port;
    private static byte[] result = new byte[1024];
    public string alldata;
    public Socket clientSocket;
    public string inputFieldString;
    public Response res;
    TextAsset asset;
    public PlayMakerFSM MgrFSM;
    public string suffix;
    // Use this for initialization
    void Start()
    {
        //Link();
        asset = Resources.Load("cgRes") as TextAsset;
        MgrFSM = gameObject.GetComponent<PlayMakerFSM>();
        Link();
        Read();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Link()
    {
        //设定服务器IP地址  
        IPAddress ip = IPAddress.Parse(ipString);
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(new IPEndPoint(ip, port)); //配置服务器IP与端口  
            Debug.Log("连接服务器成功");
            MgrFSM.SendEvent("NetSuccess");
        }
        catch
        {
            Debug.Log("连接服务器失败，请按回车键退出！");
            MgrFSM.SendEvent("NetFailure");
            return;
        }
    }
    public void SetSuffix(string suf)
    {
        suffix = suf;
    }
    public void Read()
    {
        //通过clientSocket接收数据  
        alldata = "";
        int receiveLength = clientSocket.Receive(result);
        //			alldata = Encoding.ASCII.GetString (result, 0, receiveLength);
        while (true)
        {
            Boolean state = false;
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] == '\0')
                {
                    state = true;
                    break;
                }
            }
            alldata += Encoding.ASCII.GetString(result, 0, receiveLength);
            if (state)
            {
                break;
            }
            else
            {
                receiveLength = clientSocket.Receive(result);
            }
        }
        Debug.Log("接收服务器消息：" + alldata);
        MgrFSM.SendEvent("NetSuccess");
        //return alldata;
        ParseResponse(alldata);
        if (res.result == 0)
        {
            MgrFSM.SendEvent("Result0");
        }
        else if (res.next == "choose_action")
        {
            MgrFSM.SendEvent("NextMoveChAction");
        }
        else if (res.next == "choose_index") { MgrFSM.SendEvent("NextMoveChIndex"); }
        else if (res.next == "choose_target") { MgrFSM.SendEvent("NextMoveChTarget"); }
    }

    public void Send(string message)
    {
        //通过 clientSocket 发送数据  
        try
        {
            Thread.Sleep(1000);    //等待1秒钟  
            clientSocket.Send(Encoding.ASCII.GetBytes(message + "\0"));
            Debug.Log("向服务器发送消息：" + message);
        }
        catch
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            MgrFSM.SendEvent("NetFailure");

        }
        Read();
    }
    public void SendSuffix()
    {
        Send(suffix);
    }

    public void ParseResponse(string responseData)
    {
        res = JsonConvert.DeserializeObject<Response>(responseData);
        Debug.Log("res hash is: " + res.GetHashCode());
    }
    //public void Set
}

public class Response
{
    public int result;
    public Game game;
    public string next;
}

public class Game
{
    public int turn_count;
    public Player[] players;
}

public class Player
{
    //public string weapon;
    public int mana;
    public DeckCard[] deck;
    public int active_player;
    public int current_sequence_id;
    public int turn_count;
    public string[] hand;
    public Minion[] minions;
}

public class DeckCard
{
    public string name;
    public bool used;
}

public class Minion
{
    public string name;
    public int sequence_id;
    public int damage;
    public int max_health;
    public int attack;
    public bool divine_shield;
    public bool exhausted;
    public bool already_attacked;
    public bool windfury_used;
    public int frozen_for;
    //public Effect[]    effects;
    //public Aura[]    auras;
}

