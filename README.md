# AutoUI 框架说明

## 简介
   - **AutoUI框架**来组织**页面**切换的通用逻辑。
   - 只需定制每个**页面**的业务逻辑内容，将页面的切换交给框架来处理。
   - *当前版本:* **AutoUI-V0.0292**
      - 更新内容：
         - 修复 实用工具 AutoUIUtilities 读取CSV 在打包后失效问题。
   - *Author:* [SmalBox](https://smalbox.top),*GitHub:* [GitHub/SmalBox](https://github.com/smalbox)

## 功能
   - AutoUI 框架提供 页面管理工具 *(菜单栏的Window/AutoUI)* ，可自动创建页面（生成预制体、创建脚本、更新路径配置文件、挂载脚本），业务内容填充到页面当中，页面之间的切换用框架提供的打开和返回功能进行切换即可。
   - 框架为每个页面提供了以下生命周期方法：
      - public virtual void **Init(params object[] args)** // **初始化页面**
      - public virtual void **OnShowing()** // **打开面板前**
      - public virtual void **OnShowAnim()** // **面板开启动画**
      - public virtual void **OnShowedAsync()** // **面板开启后**
      - public virtual void **Update()** // **帧更新**
      - public virtual void **OnClosing()** // **关闭前**
      - public virtual void **OnClosingAnim()** // **关闭动画**
      - public virtual void **OnClosed()** // **关闭后**
   - 制作每个页面时，将业务代码写在以上生命周期方法中，在框架切换页面时，会自动调用这些方法，来正确切换页面。
   - 每个页面有 **层级** 属性，可以在 inspector 中调整面板层级。层级会让页面在动态生成时，生成在相应的层级父物体下，来实现UI层级遮挡。
   - AutoUI 框架中使用 **页面栈** 来组织保留页面。每打开一个页面会新建一个页面然后入栈。每当返回时会销毁栈顶页面来完成返回操作。
      - **页面栈** 从0层开始计算。
      - 一般来讲不要在业务逻辑里 **直接操作栈结构** ，不要破坏栈结构的组织方式随意跳转清理栈。
      - 在 *AutoUI/Scripts/UIManager.cs* 中添加新的方法来扩展框架功能，栈的操作可以在此进行封装扩展。

## 配置
   - 场景中创建空物体来添加以下脚本：
      - GlobalVar
         - 单例，用来存储全局公共变量
      - UImanager
         - 单例，用来提供AutoUI的主要功能（可在此对框架功能进行扩展）
   - 场景中新建canvas，在其下创建空子物体
      - UIpanel
      - TipPanel
      - 将以上空物体拖到UIManager的Inspector面板

## 功能及使用
   - **命名空间**
      ``` c#
      using SmalBox.AutoUI;
      ```
   - **打开面板**
      ``` c#
      /// <summary>
      /// 打开面板
      /// </summary>
      /// <param name="panelName">面板名字</param>
      /// <param name="closeLastPanel">打开时是否关闭当前面板，默认：false</param>
      /// <param name="closeLastPanelNum">关闭当前面板数量，默认为1</param>
      /// <param name="args">扩展参数</param>
      public void OpenPanel(string panelName, bool closeLastPanel = false, int closeLastPanelNum = 1, params object[] args)
      ```
   - **返回上一级**
      ``` c#
      /// <summary>
      /// 返回上一级
      /// </summary>
      /// <param name="showLastPanelAnim">是否开启关闭当前面板动画</param>
      /// <param name="showPanelNum">面板返回动画时，打开上几个面板的数量，默认打开上一个</param>
      public void ReturnPanel(bool showLastPanelAnim = false, int showPanelNum = 1)
      ```
   - **返回任意级别菜单**
      ``` c#
      /// <summary>
      /// 返回任意级别菜单(从当前页面切换到栈指定的某层，销毁当前到层之间的栈记录)
      /// </summary>
      /// <param name="layer">返回面板堆栈的层数，层数从0开始</param>
      /// <param name="showLastPanelAnim">打开时是否关闭当前面板，默认：false</param>
      /// <param name="args">扩展参数</param>
      public void ReturnPanel(int layer, bool showLastPanelAnim = false, params object[] args)
      ```
   - **返回跳转**
      ``` c#
      /// <summary>
      /// 返回跳转(从当前页面切换到新页面，销毁到指定的栈层索引)
      /// </summary>
      /// <param name="layer">返回到某一层堆栈，打开新的页面</param>
      /// <param name="panelName">新打开面板的名字</param>
      /// <param name="args">扩展参数</param>
      public void ReturnPanel(int layer, string panelName, params object[] args)
      ```
   - **跳转到栈的某层，打开某页面**
      ``` c#
      /// <summary>
      /// 跳转到栈的某层，打开某页面
      /// </summary>
      /// <param name="layer">跳转的目标层</param>
      /// <param name="panelName">新打开的页面名</param>
      public void JumpToOpenPanel(int layer, string panelName)
      ```

   - **使用例子：**
      ``` c#
      UIManager.instance.OpenPanel("NewPage");
      UIManager.instance.OpenPanel("NewPage", false);
      UIManager.instance.ReturnPanel();
      UIManager.instance.ReturnPanel(showLastPanelAnim:false);
      UIManager.instance.ReturnPanel(0, "RedPage");
      ```

## 内置页面内容模板组件
   - AutoUI 框架内提供一些已完成的常用页面组件，可在页面中直接使用。
   - 组件prefab在 AutoUI/Resources/Prefabs/PageComponents/ 中
      - **单级返回按钮**
         - BtnReturn
      - **主菜单快速切换**
         - MainMenuQuickBar
      - **读取本地数据的文本**
         - News
      - **滑动、点击切换图片、视频**
         - Scroll View Click H
         - Scroll View Click V
         - Scroll View Swipe H
            - 功能最完善，推荐使用
         - Scroll View Swipe V
      - **标记文本**
         - TagText
      - **视频内容组，播放过场视频后循环播放另一个视频**
         - VideoContentGroup
      - **VideoPlayer解决视频残留的UGUI组件**
         - VideoPlayer
   - **预设页**
      - 页面在 AutoUI/Resources/Prefabs/Pages/ 中
      - **待机页**
         - StandbyPage
            - **功能:** 包含循环播放视频，点击跳转页面接口，配置文件设置时间自动返回待机页
   - **网络组件**
      - 网络组件位于 AutoUI/Scripts/Network/ 中
      - **UDPClient**
         - 获取本组件，注册 **SocketReceiveCallBack** 用来接收消息，用 **Send** 发送消息。
         - *注：本组件需要实用工具中的AutoUIUtilities.GetInfoForConfig来帮助获取ip端口配置文件。*
         - 在 StreamingAssets/Config/Config.txt 中 的 **UDPServerIP**, **UDPServerPort** 属性填写对应的ip和端口号。在程序中获取此组件注册回调来接收消息，调用Send可以发送消息（消息是ASCII编码，不要输入此编码外的字符，例如不要传输中文，使用ASCII表中的字符）。

## 高级功能
   - **创建页面管理工具**
      - 更改默认创建页面预制体基类
         - 默认创建工具会将 **BasePage** 预制体作为模板基类，创建新的页面。
         - 当已经创建好的页面想要加入到框架的页面当中时，则需要在 *（Assets/AutoUI/Scripts/Editor/AutoUI.cs）* 脚本的Inspector面板中替换 **BasePagePrefab**，即可在创建时将其作为源预制体页面添加到框架中。
   - **页面切换的动画设置与自定义**
      - 可以在页面的生命周期的 OnShowAnim() 和 OnClosingAnim() 中定义关闭的动画。
      - 内置了三种切换方式：
         - 直接切换无过度
         - 渐变切换
         - 缩放切换
      - 使用方式：
         ``` c#
         // 渐变过度
         Transition.Show(gameObject);
         Transition.Hide(gameObject);

         // 缩放过度
         Transition.ShowZoom(gameObject);
         Transition.HideZoom(gameObject);

         // 直接切换
         Transition.DirectShow(gameObject);
         Transition.DirectHide(gameObject);
         ```
      - 方法原型如下：
         ``` c#
         public static void Show(GameObject gameObject, float showTime = GlobalVar.animTimeOfOpenClosePage){}
         public static void Hide(GameObject gameObject, float hideTime = GlobalVar.animTimeOfClosePage){}

         public static void ShowZoom(GameObject gameObject){}
         public static void HideZoom(GameObject gameObject){}

         public static void DirectShow(GameObject gameObject){}
         public static void DirectHide(GameObject gameObject){}
         ```
   - **实用工具类：AutoUIUtilities**
      - **读取配置文件属性**
         ``` c#
         // 使用GetInfoForConfig方法获取属性
         string timeStr = AutoUIUtilities.GetInfoForConfig("Time");

         // 方法原型
         public static string GetInfoForConfig(string dataName)
         ```
         - 配置文件在StreamingAssets/Config/Config.txt中
         - 配置文件以 ##属性名##:属性值 的方式配置。例如：
            - ##Time##:10
            - 表示 Time 这个属性值是 1
      - **读取CSV文件**
         ``` c#
         // 示例：

         // 获取Test.csv中1行2列的数据
         string csvData = AutoUIUtilities.GetCSVInfo("/Config/Test.csv", 1, 2);

         // 获取Test.csv中所有的数据，存储到List二维表中
         List<List<string>> testCSVTable;
         AutoUIUtilities.GetCSVInfoToList("/Config/Test.csv", out testCSVTable);


         // 方法原型:

         /// <summary>
         /// 解析原始数据，返回指定行列的数据
         /// </summary>
         /// <param name="path">相对StreamingAssets的路径以斜杠开头</param>
         /// <param name="row">指定行</param>
         /// <param name="column">指定列</param>
         /// <returns></returns>
         public static string GetCSVInfo(string path, int row, int column)

         /// <summary>
         /// 解析原始数据，返回指定行列的数据
         /// </summary>
         /// <param name="path">相对StreamingAssets的路径以斜杠开头</param>
         /// <param name="row">指定行</param>
         /// <param name="column">指定列</param>
         /// <returns></returns>
         public static string GetCSVInfo(string path, int row, int column)
         ```
         - *注：本解析CSV文件使用的是[CSVHelper](https://joshclose.github.io/CsvHelper/)解析库，[CSVHelper GitHub](https://github.com/JoshClose/CsvHelper)*。对应的库文件在 Assets/Plugins/CSVHelper。使用VisuaStudio的NuGet工具可以安装此库（库官方给出的方法）。