using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

/*
 * 使用说明：
 * 1. 将脚本添加到Text文本组件
 * 2. 在Inspector面板中配置 FileAllName （文件带后缀的全称）
 *    例如：“config.txt”
 * 3. 脚本会在打包PC版后的StreamingAssets中生成文本内容的配置文件
 *    脚本会在第一次打开程序时自动生成配置文件
 */

/// <summary>
/// 使用外部配置文件的文本内容扩展
/// </summary>
public class TextExConfigSA : MonoBehaviour
{
    /// <summary>
    /// 带文件后缀的文件名
    /// </summary>
    public string fileAllName;

    private string result;

    // Start is called before the first frame update
    void Start()
    {
        result = "";
        StartCoroutine(LoadConfig());
    }
    private void Update()
    {
        if (result != "")
        {
            Debug.Log(result);
            GetComponent<Text>().text = result;
        }
    }

    private IEnumerator LoadConfig()
    {
        //string path = "file://" + Application.persistentDataPath + "/" + fileAllName;
        string path = Application.streamingAssetsPath + "/" + fileAllName;
        Debug.Log(path);

        // 先写入一个文件
        if (!File.Exists(path))
        {
            //FileStream fs = new FileStream(Application.persistentDataPath + "/" + fileAllName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            FileStream fs = new FileStream(Application.streamingAssetsPath + "/" + fileAllName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("初始化文件：" + fileAllName);
            sw.WriteLine("请进行文件配置。");
            sw.Close();
        }

        while (true)
        {
            // 再读取文件
            //WWW www = new WWW(path);
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(path);
            yield return www.SendWebRequest();
            result = www.downloadHandler.text;
            yield return new WaitForSeconds(5);
        }
    }
}
