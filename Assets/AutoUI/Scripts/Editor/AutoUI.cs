using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System;
using System.Threading;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor.Compilation;
using LitJson;

namespace SmalBox
{
    namespace AutoUI
    {
        public class AutoUI : EditorWindow
        {
            public GUISkin autoUISkin;
            public GameObject basePagePrefab;
            private Vector2 scrollViewVectorPages = Vector2.zero;
            private Vector2 scrollViewVectorBg = Vector2.zero;
            private float factScreenHeight = 410;
            string pageName = "NewPage";
            string pageNameNew = "NewName";
            string pageNameChange = "";
            static string pageClass =
@"using DG.Tweening.Plugins.Core.PathCore;
using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SmalBox.AutoUI;

public class ##ClassName## : PanelBase
{
    #region 生命周期
    public override void Init(params object[] args)
    {
        base.Init(args);
    }
    public override void OnShowAnim()
    {
        base.OnShowAnim();
        Transition.Show(skin);
    }
    public override void Update()
    {
        base.Update();
    }
    public override void OnClosingAnim()
    {
        base.OnClosingAnim();
        Transition.Hide(skin);
    }
    public override void OnClosed()
    {
        base.OnClosed();
    }
    #endregion

    #region 面板方法

    #endregion
}";

            [MenuItem("Window/AutoUI")]
            public static void ShowWindow()
            {
                EditorWindow.GetWindow(typeof(AutoUI));
                PlayerPrefs.SetString("PageName", "NewPage");
            }

            private void OnGUI()
            {
                //GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
                //UnityEditor.Compilation.CompilationPipeline.assemblyCompilationFinished += CompilationPipeline_assemblyCompilationFinished;
                //GUI.skin = autoUISkin;
                //if (GUILayout.Button("测试按钮"))
                //{
                    //    //Debug.Log(typeof(NewPage).Assembly);
                    //    //Debug.Log(Assembly.GetExecutingAssembly());
                    //    //var pagePrefab = PrefabUtility.InstantiatePrefab(
                    //    //    Resources.Load("Prefabs/Pages/" + pageName)) as GameObject;
                    //    //Debug.Log(Assembly.Load("Assembly-CSharp").GetType(pageName));
                    //    //Debug.Log(Assembly.Load("Assembly-CSharp-Editor").GetType(pageName));
                    //    //FileNameRegCheck(pageName);
                    //    Debug.Log(AssetDatabase.GetAssetPath(basePagePrefab));
                //}
                if (Screen.height < factScreenHeight)
                    scrollViewVectorBg = GUI.BeginScrollView(
                        new Rect(0, 0, Screen.width, Screen.height), scrollViewVectorBg,
                        new Rect(0, 0, Screen.width * 0.9f, factScreenHeight),
                        false, true);
                else
                    scrollViewVectorBg = GUI.BeginScrollView(
                        new Rect(0, 0, Screen.width, Screen.height), scrollViewVectorBg,
                        new Rect(0, 0, Screen.width * 0.9f, Screen.height),
                        false, false);

                GUI.Box(new Rect(2, 2, Screen.width - 6, 129 - 2), "");
                GUILayout.Label("自动Page管理说明：", EditorStyles.boldLabel);
                GUILayout.Label("0. 先输入 Page名 （首字母大写驼峰格式，Page作为后缀）。", EditorStyles.helpBox);
                GUILayout.Label("1.1 点击 创建Page 按钮创建页面。", EditorStyles.helpBox);
                GUILayout.Label("1.2 等待脚本编译完成，将自动挂载Page脚本。\n(页面创建完成路径在ReSources/Prefabs/Pages/中)", EditorStyles.helpBox);
                GUILayout.Label("2.1 点击 删除Page 按钮 自动清除页面相关数据。\n(会清除页面预制体、脚本、路径配置文件)", EditorStyles.helpBox);

                //pageName = EditorGUILayout.TextField("Page名：", pageName);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Page名：");
                pageName = GUILayout.TextField(pageName);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("创建Page"))
                {
                    if (!FileNameRegCheck(pageName)) return;
                    // 更新页面配置文件
                    // 从SkinDict.json中读取配置文件
                    var skinDict = new Dictionary<string, string>();
                    string skinDictStr = Resources.Load<TextAsset>("Config/SkinDict").text;
                    JsonData data = JsonMapper.ToObject(skinDictStr);
                    // 添加配置文件到字典
                    for (int i = 0; i < data.Count; i++)
                    {
                        skinDict.Add(data[i][0].ToString(), data[i][1].ToString());
                    }
                    skinDict.Add(pageName, "Prefabs/Pages/" + pageName);
                    List<GlobalVar.PagesPathDict> pagePathDictList = new List<GlobalVar.PagesPathDict>();
                    foreach (var item in skinDict)
                    {
                        var pagePathDict = new GlobalVar.PagesPathDict();
                        pagePathDict.Name = item.Key;
                        pagePathDict.Path = item.Value;
                        pagePathDictList.Add(pagePathDict);
                    }
                    string newSkinDict = JsonMapper.ToJson(pagePathDictList);
                    File.WriteAllText(Application.dataPath + "/AutoUI/Resources/Config/SkinDict.json", newSkinDict);
                    Debug.Log("页面配置文件写入完成！" + Application.dataPath + "Assets/AutoUI/Resources/Config/SkinDict.json");
                    //AssetDatabase.Refresh();

                    // 从Prefabs的Pages中复制一个BasePage，然后用pageName重命名得到新page预制体
                    //AssetDatabase.CopyAsset(
                    //    "Assets/AutoUI/Resources/Prefabs/Pages/BasePage.prefab",
                    //    "Assets/AutoUI/Resources/Prefabs/Pages/" + pageName + ".prefab");
                    AssetDatabase.CopyAsset(
                        AssetDatabase.GetAssetPath(basePagePrefab),
                        "Assets/AutoUI/Resources/Prefabs/Pages/" + pageName + ".prefab");

                    Debug.Log("页面预制体创建完成！" + "Assets/AutoUI/Resources/Prefabs/Pages/" + pageName + ".prefab");
                    //AssetDatabase.Refresh();

                    // 用名字创建类
                    pageClass = pageClass.Replace("##ClassName##", pageName);
                    File.WriteAllText(Application.dataPath + "/AutoUI/Scripts/PanelScript/" + pageName + ".cs", pageClass);
                    Debug.Log("页面类创建完成！" + Application.dataPath + "/AutoUI/Scripts/PanelScript/" + pageName + ".cs");

                    PlayerPrefs.SetInt("StartMountCom", 1);
                    AssetDatabase.ImportAsset("Assets/AutoUI/Scripts/PanelScript/" + pageName + ".cs", ImportAssetOptions.ForceUpdate);

                    AssetDatabase.Refresh();
                    // 设置编译后回调中要执行的函数标志
                    PlayerPrefs.SetString("PageName", pageName);
                    Debug.Log("正在编译脚本,请稍等……");
                }
                //if (GUILayout.Button("2.挂载组件"))
                //{
                //    if (!FileNameRegCheck(pageName)) return;
                //    // 将创建的新类挂载到复制的新资产上
                //    // 加载创建的新page预制体
                //    var pagePrefab = PrefabUtility.InstantiatePrefab(
                //        Resources.Load("Prefabs/Pages/" + pageName)) as GameObject;

                //    // 添加组件
                //    if (!pagePrefab.GetComponent(System.Reflection.Assembly.Load("Assembly-CSharp").GetType(pageName)))
                //        pagePrefab.AddComponent(System.Reflection.Assembly.Load("Assembly-CSharp").GetType(pageName));
                //    // 保存修改
                //    PrefabUtility.SaveAsPrefabAsset(pagePrefab,
                //        "Assets/AutoUI/Resources/Prefabs/Pages/" + pageName + ".prefab");
                //    AssetDatabase.Refresh();
                //    DestroyImmediate(pagePrefab);
                //    Debug.Log("挂载完成");
                //}
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("删除Page"))
                {
                    if (!FileNameRegCheck(pageName)) return;
                    // 删除配置文件中数据
                    // 从SkinDict.json中读取配置文件
                    var skinDict = new Dictionary<string, string>();
                    string skinDictStr = Resources.Load<TextAsset>("Config/SkinDict").text;
                    JsonData data = JsonMapper.ToObject(skinDictStr);
                    // 添加配置文件到字典
                    for (int i = 0; i < data.Count; i++)
                    {
                        skinDict.Add(data[i][0].ToString(), data[i][1].ToString());
                    }
                    if (skinDict.ContainsKey(pageName))
                        skinDict.Remove(pageName);
                    List<GlobalVar.PagesPathDict> pagePathDictList = new List<GlobalVar.PagesPathDict>();
                    foreach (var item in skinDict)
                    {
                        var pagePathDict = new GlobalVar.PagesPathDict();
                        pagePathDict.Name = item.Key;
                        pagePathDict.Path = item.Value;
                        pagePathDictList.Add(pagePathDict);
                    }
                    string newSkinDict = JsonMapper.ToJson(pagePathDictList);
                    File.WriteAllText(Application.dataPath + "/AutoUI/Resources/Config/SkinDict.json", newSkinDict);
                    Debug.Log("页面配置文件清理完成！" + Application.dataPath + "Assets/AutoUI/Resources/Config/SkinDict.json");
                    AssetDatabase.Refresh();

                    // 删除预制体
                    if (File.Exists(Application.dataPath + "/AutoUI/Resources/Prefabs/Pages/" + pageName + ".prefab"))
                        File.Delete(Application.dataPath + "/AutoUI/Resources/Prefabs/Pages/" + pageName + ".prefab");
                    if (File.Exists(Application.dataPath + "/AutoUI/Resources/Prefabs/Pages/" + pageName + ".prefab.meta"))
                        File.Delete(Application.dataPath + "/AutoUI/Resources/Prefabs/Pages/" + pageName + ".prefab.meta");
                    Debug.Log("页面预制体清理完成！" + Application.dataPath + "/AutoUI/Resources/Prefabs/Pages/" + pageName + ".prefab");
                    AssetDatabase.Refresh();

                    // 删除脚本
                    if (File.Exists(Application.dataPath + "/AutoUI/Scripts/PanelScript/" + pageName + ".cs"))
                        File.Delete(Application.dataPath + "/AutoUI/Scripts/PanelScript/" + pageName + ".cs");
                    if (File.Exists(Application.dataPath + "/AutoUI/Scripts/PanelScript/" + pageName + ".cs.meta"))
                        File.Delete(Application.dataPath + "/AutoUI/Scripts/PanelScript/" + pageName + ".cs.meta");
                    Debug.Log("页面脚本清理完成！");
                    AssetDatabase.Refresh();
                    pageNameChange = "";
                }
                GUILayout.EndHorizontal();

                GUILayout.Label("当前Page如下:\n(点击点击页面按钮，自动填入Page名)", EditorStyles.boldLabel);
                var pagesPath = Directory.GetFiles(Application.dataPath + "/AutoUI/Resources/Prefabs/Pages/");
                if (Screen.height < factScreenHeight)
                    scrollViewVectorPages = GUI.BeginScrollView(
                        new Rect(0, 225, Screen.width * 0.965f, 100), scrollViewVectorPages,
                        new Rect(0, 225, Screen.width * 0.9f, pagesPath.Length * 10),
                        false, true);
                else
                    scrollViewVectorPages = GUI.BeginScrollView(
                        new Rect(0, 225, Screen.width, 100), scrollViewVectorPages,
                        new Rect(0, 225, Screen.width * 0.9f, pagesPath.Length * 10),
                        false, true);

                //GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
                GUI.Box(new Rect(0, 0, Screen.width, factScreenHeight + pagesPath.Length * 10), "");
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                for (int i = 0; i < pagesPath.Length; i++)
                {
                    if (Path.GetExtension(pagesPath[i]) == ".prefab")
                    {
                        if (Path.GetFileNameWithoutExtension(pagesPath[i]) != "BasePage")
                        {
                            if (GUILayout.Button(Path.GetFileName(pagesPath[i])))
                            {
                                pageName = Path.GetFileNameWithoutExtension(pagesPath[i]);
                                pageNameChange = Path.GetFileNameWithoutExtension(pagesPath[i]);
                            }
                        }
                    }
                }
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                var scriptPath = Directory.GetFiles(Application.dataPath + "/AutoUI/Scripts/PanelScript/");
                for (int i = 0; i < scriptPath.Length; i++)
                {
                    if (Path.GetExtension(scriptPath[i]) == ".cs")
                    {
                        if (Path.GetFileNameWithoutExtension(scriptPath[i]) != "PanelBase")
                        {
                            GUILayout.Label(Path.GetFileName(scriptPath[i]));
                        }
                    }
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUI.EndScrollView();

                #region 修改Page名字信息
                GUILayout.BeginArea(new Rect(0, factScreenHeight-80, Screen.width, 64));
                GUI.Box(new Rect(0, 0, Screen.width, 60), "");
                GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                        GUILayout.BeginHorizontal();
                            GUILayout.Label("从当前Page框选择Page〖");
                            GUILayout.Label(pageNameChange);
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                            GUILayout.Label("新Page名:");
                            pageNameNew = GUILayout.TextField(pageNameNew);
                        GUILayout.EndHorizontal();
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical();
                        GUILayout.Label("〗修改此Page名");
                        if (GUILayout.Button("修改Page名"))
                        {
                            if (!FileNameRegCheck(pageNameNew)) return;
                            if (pageNameChange == "")
                            {
                                Debug.LogWarning("【请点击要更改的页面！】");
                                return;
                            }
                            // 修改page名，为挂载组件做准备
                            pageName = pageNameNew;

                            // 修改配置文件信息
                            #region 从SkinDict.json中读取配置文件
                            var skinDict = new Dictionary<string, string>();
                            string skinDictStr = Resources.Load<TextAsset>("Config/SkinDict").text;
                            JsonData data = JsonMapper.ToObject(skinDictStr);
                            // 添加配置文件到字典
                            for (int i = 0; i < data.Count; i++)
                            {
                                skinDict.Add(data[i][0].ToString(), data[i][1].ToString());
                            }
                            #endregion
                            #region 输入合法检测
                    if (!skinDict.ContainsKey(pageNameChange))
                            {
                                Debug.LogWarning("【页面不存在，请重新选择页面！】");
                                return;
                            }
                            if (skinDict.ContainsKey(pageNameNew))
                            {
                                Debug.LogWarning("【新页面名已存在，请输入不同的页面名！】");
                                return;
                            }
                    #endregion
                            #region 修改配置文件
                            skinDict.Remove(pageNameChange);
                            skinDict.Add(pageNameNew, "Prefabs/Pages/" + pageNameNew);
                            #endregion
                            #region 写回配置文件
                    List<GlobalVar.PagesPathDict> pagePathDictList = new List<GlobalVar.PagesPathDict>();
                            foreach (var item in skinDict)
                            {
                                var pagePathDict = new GlobalVar.PagesPathDict();
                                pagePathDict.Name = item.Key;
                                pagePathDict.Path = item.Value;
                                pagePathDictList.Add(pagePathDict);
                            }
                            string newSkinDict = JsonMapper.ToJson(pagePathDictList);
                            File.WriteAllText(Application.dataPath + "/AutoUI/Resources/Config/SkinDict.json", newSkinDict);
                            #endregion
                            Debug.Log("页面配置文件写入完成！" + Application.dataPath + "Assets/AutoUI/Resources/Config/SkinDict.json");
                            AssetDatabase.Refresh();

                            // 修改预制体名
                            AssetDatabase.RenameAsset(
                                "Assets/AutoUI/Resources/Prefabs/Pages/" + pageNameChange + ".prefab",
                                pageNameNew + ".prefab");
                            Debug.Log("页面预制体修改完成！" + "Assets/AutoUI/Resources/Prefabs/Pages/" + pageNameNew + ".prefab");
                            AssetDatabase.Refresh();

                            // 修改脚本名
                            string pageClassOld = File.ReadAllText(Application.dataPath + "/AutoUI/Scripts/PanelScript/" + pageNameChange + ".cs");
                            string pageClassNew = pageClassOld.Replace("public class " + pageNameChange, "public class " + pageNameNew);
                            File.WriteAllText(Application.dataPath + "/AutoUI/Scripts/PanelScript/" + pageNameChange + ".cs", pageClassNew);
                            AssetDatabase.RenameAsset("Assets/AutoUI/Scripts/PanelScript/" + pageNameChange + ".cs",
                                pageNameNew + ".cs");
                            Debug.Log("页面类修改完成！" + Application.dataPath + "/AutoUI/Scripts/PanelScript/" + pageNameNew + ".cs");
                            pageNameChange = "";
                            AssetDatabase.Refresh();
                        }
                    GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.Label("☛使用说明：点击Page滚动栏里的page选中要改名的page，输入新名字，点击修改Page名。", EditorStyles.helpBox);
                GUILayout.EndArea();
                #endregion

                GUI.EndScrollView();
            }

            // 文件名输入文本检测
            private bool FileNameRegCheck(string fileName)
            {
                if (fileName == "")
                {
                    Debug.LogWarning("输入Page名");
                    return false;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(fileName, @"^\b[A-Z][0-9a-zA-Z]+Page$"))
                {
                    Debug.LogWarning("名字输入不符合规范！");
                    return false;
                }
                return true;
            }
            private void CompilationPipeline_assemblyCompilationFinished(string arg1, UnityEditor.Compilation.CompilerMessage[] arg2)
            {
                Debug.Log(arg1);
            }
            
            // 删除组件**
            private void DeleteComponent()
            {
                // 加载创建的新page预制体
                var pagePrefab = PrefabUtility.InstantiatePrefab(
                    Resources.Load("Prefabs/Pages/" + pageNameNew)) as GameObject;

                // 删除组件
                if (pagePrefab.GetComponent(System.Reflection.Assembly.Load("Assembly-CSharp").GetType(pageNameChange)))
                    DestroyImmediate(pagePrefab.GetComponent(System.Reflection.Assembly.Load("Assembly-CSharp").GetType(pageNameChange)));
                // 保存修改
                PrefabUtility.SaveAsPrefabAsset(pagePrefab,
                    "Assets/AutoUI/Resources/Prefabs/Pages/" + pageNameNew + ".prefab");
                AssetDatabase.Refresh();
                DestroyImmediate(pagePrefab);
            }


            // 挂载组件
            [UnityEditor.Callbacks.DidReloadScripts]
            public static void AllScriptReloaded()
            {
                MountCom();
            }
            public static void MountCom()
            {
                if (PlayerPrefs.GetInt("StartMountCom") > 0)
                {
                    PlayerPrefs.SetInt("StartMountCom", 0);
                    Debug.Log("脚本编译完成，开始挂载脚本。");
                    var pagePrefab = PrefabUtility.InstantiatePrefab(
                        Resources.Load("Prefabs/Pages/" + PlayerPrefs.GetString("PageName"))) as GameObject;
                    // 添加组件
                    if (!pagePrefab.GetComponent(System.Reflection.Assembly.Load("Assembly-CSharp").GetType(PlayerPrefs.GetString("PageName"))))
                       pagePrefab.AddComponent(System.Reflection.Assembly.Load("Assembly-CSharp").GetType(PlayerPrefs.GetString("PageName")));
                    // 保存修改
                    PrefabUtility.SaveAsPrefabAsset(pagePrefab,
                        "Assets/AutoUI/Resources/Prefabs/Pages/" + PlayerPrefs.GetString("PageName") + ".prefab");
                    AssetDatabase.Refresh();
                    DestroyImmediate(pagePrefab);
                    Debug.Log("挂载完成，" + PlayerPrefs.GetString("PageName") + "创建完毕！");
                }
            }
        }
    }
}
