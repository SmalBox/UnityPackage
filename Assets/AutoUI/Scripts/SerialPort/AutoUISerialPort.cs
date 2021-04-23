using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using SmalBox.AutoUI;
/// <summary>
/// 串口通信组件，可在inspector中配置要连接的串口属性，在外部配置文件中修改串口号。
/// 提供串口接收消息的回调 ComReceiveCallBack , 当收到串口消息时此组件会自动解析命令
/// 解析到完整命令后会自动调用 ComReceiveCallBack 回调来执行处理命令。
/// 通过配置 CMD_LENGTH 配置命令长度。
/// </summary>
public class AutoUISerialPort : MonoBehaviour
{
    public SerialPort sp;
    public bool isConfigComName = false;
    public string comName = "COM1";
    public int baudRate = 9600;
    public Parity parity = Parity.None;
    public int dataBits = 8;
    public StopBits stopBits = StopBits.One;

    public int CMD_LENGTH = 9;

    public Action<String> ComReceiveCallBack;

    private Queue buffQueue;
    private List<byte> cmd;

    private void Awake()
    {
        sp = new SerialPort();
        sp.ReadTimeout = 400;
        sp.RtsEnable = true;
        //sp.DataReceived += new SerialDataReceivedEventHandler(PortDataReceived); // unity 无法触发这个回调。手动调用read读取数据
        //sp.ReceivedBytesThreshold = 1; // Unity 支持不全
        InitSP();
        OpenSP();
        buffQueue = new Queue();
        cmd = new List<byte>();
        ComReceiveCallBack = (string str) => { };
    }
    private void Update()
    {
        if (sp != null && sp.IsOpen && sp.BytesToRead > 0)
        {
            try
            {
                byte[] dataBytes = new byte[sp.BytesToRead];
                sp.Read(dataBytes, 0, sp.BytesToRead);
                foreach (var item in dataBytes)
                {
                    print("收到：" + item.ToString("X2"));
                    // 加入缓冲队列
                    buffQueue.Enqueue(item);
                }
                // 解析缓冲队列
                ParsedData();
            }
            catch (Exception ex)
            {
                print("输出错误：" + ex.ToString());
            }
        }
        #region 数据测试
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (sp != null && sp.IsOpen)
            {
                print("当前接受到的字符个数：" + sp.BytesToRead);
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            print("开始输出消息");
            if (sp != null && sp.IsOpen && sp.BytesToRead > 0)
            {
                try
                {
                    byte[] dataBytes = new byte[9];
                    sp.Read(dataBytes, 0, dataBytes.Length);
                    foreach (var item in dataBytes)
                    {
                        print(item.ToString("X2"));
                    }
                }
                catch (Exception ex)
                {
                    print("输出错误：" + ex.ToString());
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            //byte[] data = { 0x00, 0x5A, 0x58, 0x00, 0x07, 0x00, 0x00, 0x00, B9 };
            byte[] data = new byte[9];
            // 取字节的ascii数值 求和 ，转换成16进制，即使校验和。
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            print("数据队列长度：" + buffQueue.Count);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            print(sp.ReadLine());
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //print(sp.Read());
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            print(sp.ReadByte());
        }
        #endregion
    }

    private void InitSP()
    {
        if (isConfigComName)
        {
            sp.PortName = AutoUIUtilities.GetInfoForConfig("COMName");
        }else
        {
            sp.PortName = comName;
        }
        sp.BaudRate = 9600;
        sp.Parity = Parity.None;
        sp.DataBits = 8;
        sp.StopBits = StopBits.One;
    }
    private void OpenSP()
    {
        try
        {
            print("打开串口" + sp.PortName);
            sp.Open();
        }
        catch ( Exception ex)
        {
            print("打开出错，错误信息：" + ex.ToString());
            return;
        }
    }
    private void CloseSP()
    {
        try
        {
            print("关闭串口" + sp.PortName);
            sp.Close();
        }
        catch ( Exception ex)
        {
            print("关闭出错，错误信息：" + ex.ToString());
            return;
        }
    }
    
    // 解析数据
    private void ParsedData()
    {
        int emptyData;
        // 将缓冲区数据填充命令列表

        // 查看命令列表当前字节数，与命令长度对比计算有几个空位字节
        emptyData = CMD_LENGTH - cmd.Count;
        // 向空位填充字节，填充时通过缓冲区剩余个数控制数组别越界
        for (int i = 0; i < emptyData; i++)
        {
            if (buffQueue.Count > 0)
            {
                cmd.Add((byte)buffQueue.Dequeue());
            }
        }
        // 填充后再次查看字节数，当空余字节数为0时，计算校验和对比
        emptyData = CMD_LENGTH - cmd.Count;
        if (emptyData == 0)
        {
            if (cmd[CMD_LENGTH - 1] == CalculateChecksum()) // 当校验和正确时
            {
                // 对比成功则执行命令
                ExeCmd();
            } else
            {
                // 对比失败查看缓冲区数据是否为空，不为空则向后移动命令列表一个位置继续判断
                if (buffQueue.Count > 0)
                {
                    cmd.RemoveAt(0);
                    ParsedData();
                }
            }
        }
    }
    // 执行命令
    private void ExeCmd()
    {
        // 判断命令 列表长度 和 校验和 是否符合要求
        if (cmd.Count < CMD_LENGTH || cmd[CMD_LENGTH - 1] != CalculateChecksum())
            return;
        // 解析地址，根据地址判断哪个内容播放
        // 将命令字节列表转化成字符串对比
        string cmdStr = "";
        ComReceiveCallBack?.Invoke(cmdStr);
        // 命令执行完毕，清空命令列表
        cmd.Clear();
    }
    // 计算校验和
    private byte CalculateChecksum()
    {
        if (cmd.Count < CMD_LENGTH)
            return 0x00;
        byte sum = 0x00;
        for (int i = 0; i < CMD_LENGTH - 1; i++)
        {
            sum += cmd[i];
        }
        return sum;
    }

    private void PortDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        print("收到串口" + sp.PortName + "的消息：");
    }
    private void OnDestroy()
    {
        CloseSP();
    }
}
