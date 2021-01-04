using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Text;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.UI;

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
                if (data.Length > dataName.Length + 5)
                {
                    if ("##" + dataName + "##:" == data.Substring(0, dataName.Length + 5))
                    {
                        return data.Substring(dataName.Length + 5);
                    }
                }
            }
            return null;
        }

        #region 解析CSV文件方法（引用CSVHelper.dll）
        /// <summary>
        /// 解析原始数据，返回指定行列的数据
        /// </summary>
        /// <param name="path">相对StreamingAssets的路径以斜杠开头</param>
        /// <param name="row">指定行</param>
        /// <param name="column">指定列</param>
        /// <returns></returns>
        public static string GetCSVInfo(string path, int row, int column)
        {
            using (var reader = new StreamReader(Application.streamingAssetsPath + path, Encoding.GetEncoding("GBK")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = false;
                csv.Read();
                for (int i = 0; i < row - 1; i++)
                {
                    if (!csv.Read())
                    {
                        Debug.LogWarning("获取csv文件数据越界");
                        return null;
                    }
                }
                var columnCount = csv.Context.Record.Length;
                if (column - 1 < 0 || column > columnCount ||
                    row - 1 < 0)
                {
                    Debug.LogWarning("获取csv文件数据越界");
                    return null;
                }
                var records = csv.GetField(column - 1);
                return records;
            }
        }
        /// <summary>
        /// 读取csv文件到List表中。
        /// </summary>
        /// <param name="path">相对StreamingAssets的路径以斜杠开头</param>
        /// <param name="csvTable">List二维表对象，读取后将赋值给此二位列表</param>
        /// <param name="openLog">是否在读取时打开log输出，默认为：false</param>
        public static void GetCSVInfoToList(string path, out List<List<string>> csvTable, GameObject msg, bool openLog = false)
        {
            csvTable = new List<List<string>>();
            msg.GetComponent<Text>().text = "csvhelper文件读取前：" + Application.streamingAssetsPath + path;
            // 读取csv文件存入csvTable中
            //using (var reader = new StreamReader(Application.streamingAssetsPath + path, Encoding.GetEncoding("GBK")))
            using (var reader = new StreamReader(Application.streamingAssetsPath + path, Encoding.GetEncoding("UTF-8")))
            {
                msg.GetComponent<Text>().text = "csvhelper读取前";
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    msg.GetComponent<Text>().text = "csvhelper读取中";
                    csv.Configuration.HasHeaderRecord = false;
                    int index = 0;
                    while (csv.Read())
                    {
                        List<string> tempList = new List<string>();
                        var columnCount = csv.Context.Record.Length;
                        for (int i = 0; i < columnCount; i++)
                        {
                            tempList.Add(csv.GetField<string>(i));
                            if (openLog)
                                Debug.Log("行：" + index + ", 列：" + i + ", 数据：" + csv.GetField<string>(i));
                        }
                        csvTable.Add(tempList);
                        index++;
                    }
                }
            }
        }
        #endregion
    }
}
