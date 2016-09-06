using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Text;
using Newtonsoft.Json;

public class SockM : MonoBehaviour {
    public string ipString;
    public int port;
    private static byte[] result = new byte[1024];
    public string alldata;
    public Socket clientSocket;
    TextAsset asset;
    // Use this for initialization
    void Start () {
        //Link();
        asset = Resources.Load("cgRes") as TextAsset;
        var deserialzedData = ParseResponse(asset.text);
        Debug.Log("Deserialized:" + deserialzedData);
    }

    // Update is called once per frame
    void Update () {
	
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
        }
        catch
        {
            Debug.Log("连接服务器失败，请按回车键退出！");
            return;
        }
    }

    public string Read()
    {
        //通过clientSocket接收数据  
        alldata = "";
        int receiveLength = clientSocket.Receive(result);
        //			alldata = Encoding.ASCII.GetString (result, 0, receiveLength);
        while (true)
        {
            Boolean state = false;
            for (int i = 0 ; i < result.Length; i++)
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
        Debug.Log("接收服务器消息："+alldata);
        return alldata;

    }

    public void Send(string message)
    {
        //通过 clientSocket 发送数据  
        try
        {
            Thread.Sleep(1000);    //等待1秒钟  
            clientSocket.Send(Encoding.ASCII.GetBytes(message));
            Debug.Log("向服务器发送消息：" + message);
        }
        catch
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }

    }

    public Response ParseResponse(string responseData)
    {
        return JsonConvert.DeserializeObject<Response>(responseData);
    }
}

public class Response
{
    public int result;
    public Game game;
}

public class Game
{
    public int turn_count;
    public List<Player> players;
}

public class Player
{
    public string weapon;
    public int mana;
}