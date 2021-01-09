using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmalBox.AutoUI;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

public class UDPServer : MonoBehaviour
{
    private string clientIP;
    private string clientPort;

    private Socket serverSocket;
    // 消息接收 CallBack, 注册这个回调来接收处理UDP客户端收到的信息
    public Action<string> SocketReceiveCallBack;

    private string recStr;
    private string RecStr
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
    private byte[] recData = new byte[1024];
    private byte[] sendData = new byte[1024];

    private Thread serverThread;

    private void Awake()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.quitting += (() => serverThread.Abort());
#endif
        InitUDPServer();
    }
    private void InitUDPServer()
    {
        // 获取绑定的端口
        int serverPort = System.Convert.ToInt32(AutoUIUtilities.GetInfoForConfig("LocalUDPServerPort"));
        string severrIP = AutoUIUtilities.GetInfoForConfig("LocalUDPServerIP");
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        serverSocket.Bind(new IPEndPoint(IPAddress.Parse(severrIP), serverPort));

        serverThread = new Thread(new ThreadStart(Receive));
        serverThread.Start();

        // 开启协程检查监听服务是否还在，如果不在则重新启动这个监听，防止监听被kill掉。
        StartCoroutine(CheckThread());
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
            EndPoint clientEnd = new IPEndPoint(IPAddress.Any, System.Convert.ToInt32(AutoUIUtilities.GetInfoForConfig("UDPServerPort")));
            var recLen = serverSocket.ReceiveFrom(recData, ref clientEnd);
            RecStr = Encoding.ASCII.GetString(recData, 0, recLen);
            //Debug.Log("收到来自" + clientEnd.ToString() + "消息：" + RecStr);
            //Send("Server  received msg :" + RecStr, clientEnd);
        }
    }

    private IEnumerator CheckThread()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (serverThread == null)
            {
                serverThread = new Thread(new ThreadStart(Receive));
                serverThread.Start();
            }
        }
    }

    private void OnApplicationQuit()
    {
        serverThread.Abort();
    }
}