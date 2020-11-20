using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SmalBox.AutoUI
{
    public class AutoUIUtilities : MonoBehaviour
    {
        /// <summary>
        /// 读取StreamingAssets/Config/Config.txt中的配置文件。
        /// 配置文件以 ##属性名##:属性值 的方式配置。
        /// 此方法将用 属性名 来获取属性值，并以string的方式返回。
        /// 如果没有匹配的属性则返回 null，可以此判断是否取值成功。
        /// </summary>
        /// <param name="dataName">属性名字符串</param>
        /// <returns></returns>
        public static string GetInfoForConfig(string dataName)
        {
            foreach (string data in File.ReadLines(Application.streamingAssetsPath + "/Config/Config.txt"))
            {
                if (data.Length > dataName.Length)
                {
                    if ("##" + dataName + "##:" == data.Substring(0, dataName.Length + 5))
                    {
                        return data.Substring(dataName.Length + 5);
                    }
                }
            }
            return null;
        }
    }
}
