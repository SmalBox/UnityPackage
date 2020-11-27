using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using SmalBox.AutoUI;

public class SceneManage : MonoBehaviour
{
    private void Awake()
    {
        // 初始化点击次数
        GlobalVar.instance.screenClickTimes = 0;
        // 初始化返回待机页检测
        GlobalVar.instance.startAutoReturnStandbyCheck = false;
        // 读取配置文件自动返回时间
        GlobalVar.instance.globalCountdown = System.Convert.ToInt32(AutoUIUtilities.GetInfoForConfig("AutoReturnTime"));
    }
    private void Start()
    {
        // 打开待机页
        UIManager.instance.OpenPanel("StandbyPage");
    }
    private void Update()
    {
        if (GlobalVar.instance.startAutoReturnStandbyCheck)
        {
            // 全局检测 倒计时返回
            if (Input.GetMouseButton(0))
            {
                // 有点击则重置时间
                GlobalVar.instance.globalCountdown = System.Convert.ToInt32(AutoUIUtilities.GetInfoForConfig("AutoReturnTime"));
            }
            if (GlobalVar.instance.globalCountdown > 0)
            {
                // 倒计时
                GlobalVar.instance.globalCountdown -= Time.deltaTime;
                Debug.Log("剩余时间：" + GlobalVar.instance.globalCountdown);
            }else
            {
                // 返回待机页
                UIManager.instance.ReturnPanel(0);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<UDPClient>().Send("I'm Client");
        }
        
    }

}
