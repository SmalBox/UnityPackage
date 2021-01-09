using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SmalBox.AutoUI;
using System;

public class GetTextForUrl : MonoBehaviour
{
    [Header("获文本取数据的URL")]
    public string requestUrl = "";

    // 提供一个数据处理回调delegate，注册这个delegate可以获取数据并处理这个数据
    public Action<string> DataProcessingCallBack;

    private string textStr = "";

    void Start()
    {
        StartCoroutine(GetText());
    }

    // 获取数据更新
    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get(requestUrl);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
            Debug.Log("网络错误！");
        else
        {
            Debug.Log("网络正常。");
            textStr = System.Text.Encoding.Default.GetString(www.downloadHandler.data);
            DataProcessingCallBack?.Invoke(textStr);
        }
    }
}
