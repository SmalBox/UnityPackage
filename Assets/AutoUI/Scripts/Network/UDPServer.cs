using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmalBox.AutoUI;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPServer : MonoBehaviour
{
    public Action<string> SocketReceiveCallBack;

    private string clientIP;
    private string clientPort;

    private Socket serverSocket;

    private string recStr;
    public string RecStr
    {
        get
        {
            return recStr;
        }
        set
        {
            recStr = value;
            // 添加回调方法
            SocketReceiveCallBack?.Invoke(recStr);
        }
    }
    private string sendStr;

    private byte[] recData = new byte[1024];
    private byte[] sendData = new byte[1024];

    private Thread serverThread;

    private void Awake()
    {
        InitUDPServer();
    }
    private void InitUDPServer()
    {
        // 获取绑定的端口
        int serverPort = System.Convert.ToInt32(AutoUIUtilities.GetInfoForConfig("UDPServerPort"));
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), serverPort));

        serverThread = new Thread(new ThreadStart(Receive));
        serverThread.Start();
    }
    private void Send(string sendStr, EndPoint endPoint)
    {
        sendData = new byte[1024];
        sendData = Encoding.ASCII.GetBytes(sendStr);
        serverSocket.SendTo(sendData, sendData.Length, SocketFlags.None, endPoint);
    }
    private void Receive()
    {
        while (true)
        {
            recData = new byte[1024];
            EndPoint clientEnd = new IPEndPoint(IPAddress.Any, 0);
            var recLen = serverSocket.ReceiveFrom(recData, ref clientEnd);
            RecStr = Encoding.ASCII.GetString(recData, 0, recLen);
            Debug.Log("收到来自" + clientEnd.ToString() + "消息：" + RecStr);
            Send("Server  received msg :" + RecStr, clientEnd);
        }
    }
}
