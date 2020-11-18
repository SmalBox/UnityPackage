# AutoUI 框架说明
## 简介
   - **AutoUI框架**来组织**页面**切换的通用逻辑。
   - 只需定制每个**页面**的业务逻辑内容，将页面的切换交给框架来处理。

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
   - **打开面板**
      ``` c#
      /// <summary>
      /// 打开面板
      /// </summary>
      /// <param name="panelName">面板名字</param>
      /// <param name="closeLastPanel">打开时是否关闭当前面板，默认：false</param>
      /// <param name="args">扩展参数</param>
      public void OpenPanel(string panelName, bool closeLastPanel = false, params object[] args)
      ```
   - **返回上一级**
      ``` c#
      /// <summary>
      /// 返回上一级
      /// </summary>
      /// <param name="showLastPanelAnim">是否开启关闭当前面板动画</param>
      public void ReturnPanel(bool showLastPanelAnim = false)
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

   - **使用例子：**
      ``` c#
      UIManager.instance.OpenPanel("NewPage");
      UIManager.instance.OpenPanel("NewPage", false);
      UIManager.instance.ReturnPanel();
      UIManager.instance.ReturnPanel(showLastPanelAnim:false);
      UIManager.instance.ReturnPanel(0, "RedPage");
      ```

## 高级功能
   - **创建页面管理工具**
      - 更改默认创建页面预制体基类
         - 默认创建工具会将 **BasePage** 预制体作为模板基类，创建新的页面。
         - 当已经创建好的页面想要加入到框架的页面当中时，则需要在 *（Assets/AutoUI/Scripts/Editor/AutoUI.cs）* 脚本的Inspector面板中替换 **BasePagePrefab**，即可在创建时将其作为源预制体页面添加到框架中。