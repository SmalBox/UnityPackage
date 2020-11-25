using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmalBox.AutoUI;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

/// <summary>
/// 获取本组件，注册SocketReceiveCallBack用来接收消息，用Send发送消息。
/// 注：本组件需要AutoUIUtilities.GetInfoForConfig来帮助获取ip端口配置文件。
/// </summary>
public class UDPClient : MonoBehaviour
{
    // 消息接收 CallBack, 注册这个回调来接收处理UDP客户端收到的信息
    public Action<string> SocketReceiveCallBack;

    private string hardwareServerIP;
    private string hardwareServerPort;
    private IPEndPoint hardwareServerIPEnd;

    private Socket clientSocket;

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
    private string SendStr;

    private byte[] recData = new byte[1024];
    private byte[] sendData = new byte[1024];

    private Thread clientThread;

    private void Awake()
    {
        InitHardwareNetConfig();
        InitUDPClient();
        #region 注册消息回调
        SocketReceiveCallBack = (string recStr) =>
        {
            Debug.Log("我是注册的接收消息回调，收到消息：" + recStr);
        };
        #endregion
    }
    // 初始化硬件网络参数
    public void InitHardwareNetConfig()
    {
        // 从ip
        hardwareServerIP = AutoUIUtilities.GetInfoForConfig("UDPServerIP");
        hardwareServerPort = AutoUIUtilities.GetInfoForConfig("UDPServerPort");
    }
    // 初始化网络对象
    public void InitUDPClient()
    {
        // 初始化硬件服务端ip地址
        hardwareServerIPEnd = new IPEndPoint(IPAddress.Parse(hardwareServerIP), System.Convert.ToInt32(hardwareServerPort));
        // 初始化客户端socket
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // 发送消息给硬件服务端，让硬件服务端知道我的IP和端口
        Send("InitMsg");

        // 开启udp客户端接收消息线程
        clientThread = new Thread(new ThreadStart(Receive));
        clientThread.Start();
    }

    // 发送消息方法
    public void Send(string sendStr)
    {
        sendData = new byte[1024];
        sendData = Encoding.ASCII.GetBytes(sendStr);
        clientSocket.SendTo(sendData, sendData.Length, SocketFlags.None, hardwareServerIPEnd);
    }
    private void Receive()
    {
        while (true)
        {
            recData = new byte[1024];
            EndPoint serverEnd = new IPEndPoint(IPAddress.Any, 0);
            var recLen = clientSocket.ReceiveFrom(recData, ref serverEnd);
            RecStr = Encoding.ASCII.GetString(recData, 0, recLen);
            Debug.Log("收到来自 "+ serverEnd.ToString() +"消息:" + RecStr);
        }
    }
}
