using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;
using System.IO;

namespace SmalBox.AutoUI
{

    public class UIManager : MonoBehaviour
    {
        // 单例
        public static UIManager instance;
        private UIManager() { }


        // Panel 父对象
        public GameObject uiPanel;

        // Tips 父对象
        public GameObject uiTips;

        // 存储皮肤面板路径信息
        public Dictionary<string, string> skinDict;

        // 已打开面板字典
        public static Dictionary<string, PanelBase> openedDict;

        // 面板堆栈
        public Stack<PanelBase> panelStack;


        //层级
        public enum PanelLayer
        {
            // 面板
            Panel = 0,
            // 提示
            Tips = 1,
        }

        [HideInInspector]
        // 层级对应坐标父对象
        public List<GameObject> panelLayerParentObj;

        public void InitSkinDict()
        {
            skinDict = new Dictionary<string, string>();

            // 从SkinDict.json中读取配置文件
            string skinDictStr = Resources.Load<TextAsset>("Config/SkinDict").text;
            JsonData data = JsonMapper.ToObject(skinDictStr);
            // 添加配置文件到字典
            for (int i = 0; i < data.Count; i++)
            {
                skinDict.Add(data[i][0].ToString(), data[i][1].ToString());
            }
        }
        public void InitPanelLayerParent()
        {
            panelLayerParentObj.Add(uiPanel);
            panelLayerParentObj.Add(uiTips);
        }


        private void Awake()
        {
            if (instance == null) instance = GetComponent<UIManager>();

            // 初始化皮肤面板路径信息
            InitSkinDict();

            // 初始化父层级坐标
            InitPanelLayerParent();

            // 初始化已打开面板字典
            openedDict = new Dictionary<string, PanelBase>();

            // 初始化面板堆栈
            panelStack = new Stack<PanelBase>();
        }

        #region 创建面板
        public PanelBase CreatePanel(string panelName, params object[] args)
        {
            // 如果面板已经打开，则直接返回
            string name = panelName;
            if (openedDict.ContainsKey(name))
                return null;
            // 根据参数初始化面板
            GameObject instantiatePanel = (GameObject)Instantiate(Resources.Load(skinDict[name]), panelLayerParentObj[0].transform);
            PanelBase panel = instantiatePanel.GetComponent(Assembly.Load("Assembly-CSharp").GetType(panelName)) as PanelBase;
            //Debug.Log("初始化当前面板到：" + panel.layer + "层级");
            instantiatePanel.transform.SetParent(panelLayerParentObj[(int)panel.layer].transform);

            panel.Init(args);
            // 加载皮肤
            panel.skinPath = skinDict[name];
            panel.skin = instantiatePanel;

            // 将面板加入已打开列表(已创建)
            openedDict.Add(name, panel);

            panel.OnShowing();
            return panel;
        }
        #endregion

        #region 显示面板
        public PanelBase ShowPanel(PanelBase panel)
        {
            panel.OnShowAnim();

            return panel;
        }
        #endregion

        #region 隐藏面板
        public void HidePanel(PanelBase panel)
        {
            panel.OnClosingAnim();
        }
        #endregion

        #region 关闭面板
        public void ClosePanel(string panelName)
        {
            // 面板没打开则直接返回
            string name = panelName;
            if (!openedDict.ContainsKey(name))
                return;
            PanelBase panel = openedDict[name];
            if (panel == null)
                return;
            // 先从字典中移除，然后再开启异步。防止在关闭前其他面板操作字典，导致错误。
            openedDict.Remove(name);
            StartCoroutine(ClosePlanelAnimAndDestroy(panel));
        }
        // 协程异步执行关闭操作，等待关闭动画结束再销毁
        private IEnumerator ClosePlanelAnimAndDestroy(PanelBase panel)
        {
            panel.OnClosing();
            panel.OnClosingAnim();
            yield return new WaitForSeconds(GlobalVar.animTimeOfClosePage);
            // 从打开字典中移出
            panel.OnClosed();
            GameObject.Destroy(panel.skin);
            Component.Destroy(panel);
        }
        #endregion

        #region 面板栈相关方法
        private void PushPanel(PanelBase panel)
        {
            panelStack.Push(panel);
        }
        #endregion

        #region 面板操作指令：打开面板、返回
        /// <summary>
        /// 打开面板
        /// </summary>
        /// <param name="panelName">面板名字</param>
        /// <param name="closeLastPanel">打开时是否关闭当前面板，默认：false</param>
        /// <param name="closeLastPanelNum">关闭当前面板数量，默认为1</param>
        /// <param name="args">扩展参数</param>
        public void OpenPanel(string panelName, bool closeLastPanel = false, int closeLastPanelNum = 1, params object[] args)
        {
            PanelBase panel = CreatePanel(panelName, args);
            // 新打开面板
            if (panel != null)
            {
                // 关闭栈顶面板
                if (panelStack.Count > 0)
                {
                    panelStack.Peek().OnClosing();
                    if (closeLastPanel)
                    {
                        var panelArray = panelStack.ToArray();
                        if (closeLastPanelNum > panelArray.Length)
                            closeLastPanelNum = panelArray.Length;
                        for (int i = 0; i < closeLastPanelNum; i++)
                        {
                            HidePanel(panelArray[i]);
                        }
                    }
                }
                ShowPanel(panel);
                // 新打开的面板入栈
                panelStack.Push(panel);
            }
        }
        /// <summary>
        /// 返回上一级
        /// </summary>
        /// <param name="showLastPanelAnim">是否开启关闭当前面板动画</param>
        /// <param name="showPanelNum">面板返回动画时，打开上几个面板的数量，默认打开上一个</param>
        public void ReturnPanel(bool showLastPanelAnim = false, int showPanelNum = 1)
        {
            if (panelStack.Count >= 2)
            {
                // 栈顶面板出栈，并关闭栈顶面板
                var topPanel = panelStack.Pop();
                ClosePanel(topPanel.GetType().Name);
                // 打开当前 栈顶面板
                panelStack.Peek().OnShowing();
                if (showLastPanelAnim)
                {
                    var panelArray = panelStack.ToArray();
                    if (showPanelNum > panelArray.Length)
                        showPanelNum = panelArray.Length;
                    for (int i = 0; i < showPanelNum; i++)
                    {
                        ShowPanel(panelArray[i]);
                    }
                }
            }
        }
        /// <summary>
        /// 返回任意级别菜单(从当前页面切换到栈指定的某层，销毁当前到层之间的栈记录)
        /// </summary>
        /// <param name="layer">返回面板堆栈的层数，层数从0开始</param>
        /// <param name="showLastPanelAnim">打开时是否关闭当前面板，默认：false</param>
        /// <param name="args"></param>
        public void ReturnPanel(int layer, bool showLastPanelAnim = false, params object[] args)
        {
            if (layer >= panelStack.Count - 1 || panelStack.Count < 2) return;
            // 栈顶面板出栈，并关闭栈顶面板
            var topPanel = panelStack.Pop();
            ClosePanel(topPanel.GetType().Name);
            while (panelStack.Count - 1 > layer)
            {
                topPanel = panelStack.Pop();
                ClosePanel(topPanel.GetType().Name);
            }
            // 打开当前栈顶面板
            topPanel = panelStack.Peek();
            topPanel.OnShowing();
            if (showLastPanelAnim)
            {
                ShowPanel(topPanel);
            }
        }
        /// <summary>
        /// 返回跳转(从当前页面切换到新页面，销毁到指定的栈层索引)
        /// </summary>
        /// <param name="layer">返回到某一层堆栈，打开新的页面</param>
        /// <param name="panelName">新打开面板的名字</param>
        /// <param name="args">扩展参数</param>
        public void ReturnPanel(int layer, string panelName, params object[] args)
        {
            if (layer >= panelStack.Count || panelStack.Count < 2) return;
            if (layer != panelStack.Count - 1)
            {
                // 栈顶面板出栈，并关闭栈顶面板
                var topPanel = panelStack.Pop();
                ClosePanel(topPanel.GetType().Name);
                while (panelStack.Count - 1 > layer)
                {
                    topPanel = panelStack.Pop();
                    ClosePanel(topPanel.GetType().Name);
                }
            }
            // 打开新面板
            var newPanel = CreatePanel(panelName);
            // 页面入栈
            panelStack.Push(newPanel);
            ShowPanel(newPanel);
        }
        /// <summary>
        /// 跳转到栈的某层，打开某页面
        /// </summary>
        /// <param name="layer">跳转的目标层</param>
        /// <param name="panelName">新打开的页面名</param>
        public void JumpToOpenPanel(int layer, string panelName)
        {
            if (layer >= panelStack.Count) return;
            while (panelStack.Count - 1 > layer)
            {
                ClosePanel(panelStack.Pop().GetType().Name);
            }
            // 打开新面板
            var newPanel = CreatePanel(panelName);
            // 页面入栈
            panelStack.Push(newPanel);
            ShowPanel(newPanel);
        }
        #endregion


        public static Type Typen(string typeName)
        {
            Type type = null;
            Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
            int assemblyArrayLength = assemblyArray.Length;
            for (int i = 0; i < assemblyArrayLength; ++i)
            {
                type = assemblyArray[i].GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }

            for (int i = 0; (i < assemblyArrayLength); ++i)
            {
                Type[] typeArray = assemblyArray[i].GetTypes();
                int typeArrayLength = typeArray.Length;
                for (int j = 0; j < typeArrayLength; ++j)
                {
                    if (typeArray[j].Name.Equals(typeName))
                    {
                        return typeArray[j];
                    }
                }
            }
            return type;
        }
    }
}
