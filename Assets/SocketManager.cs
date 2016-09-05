using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Text;


namespace SocketClient
{
	class Program:MonoBehaviour
	{
		private static byte[] result = new byte[1024];
		public string alldata;

		public void Link()
		{  
			//设定服务器IP地址  
			IPAddress ip = IPAddress.Parse ("192.168.4.29");  
			Socket clientSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  
			try {  
				clientSocket.Connect (new IPEndPoint (ip, 5000)); //配置服务器IP与端口  
				Console.WriteLine ("连接服务器成功");  
			} catch {  
				Console.WriteLine ("连接服务器失败，请按回车键退出！");  
				return;  
			}  
			//通过clientSocket接收数据  
			alldata = "";
			int receiveLength = clientSocket.Receive (result);  
			//			alldata = Encoding.ASCII.GetString (result, 0, receiveLength);
			while (true) {
				Boolean state = false;
				int i=0;
				for (; i < result.Length; i++) {
					if (result [i] == '\0') {
						state = true;
						break;
					}

				}
				alldata += Encoding.ASCII.GetString (result, 0, receiveLength);
				if (state) {
					break;
				} else {
					receiveLength = clientSocket.Receive (result);
				}

			}
			Console.WriteLine ("接收服务器消息：{0}", alldata);  
			//通过 clientSocket 发送数据  
			for (int i = 0; i < 10; i++) {  
				try {  
					Thread.Sleep (1000);    //等待1秒钟  
					string sendMessage = "client send Message Hellp" + DateTime.Now;  
					clientSocket.Send (Encoding.ASCII.GetBytes (sendMessage));  
					Console.WriteLine ("向服务器发送消息：{0}" + sendMessage);  
				} catch {  
					clientSocket.Shutdown (SocketShutdown.Both);  
					clientSocket.Close ();  
					break;  
				}  
			}  
			Console.WriteLine ("发送完毕，按回车键退出");  
			Console.ReadLine ();  
		}
	}
} 