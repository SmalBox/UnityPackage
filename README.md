# UnityPackage

   - 常用插件打包
   - 插件包或脚本在不同分支，拉取相应分支即可

## TimeControl

   - 控制软件使用期限

## CameraFilterPack_Blend2Camera_BOverlapA

   - 镜头特效，将相机2的画面覆盖混合到相机1的画面，越过Unity的相机深度渲染机制，强制覆盖混合相机2画面到相机1上

## AutoUI

   - 当前最新版本 **AutoUI-V0.0292**
   - **AutoUI框架** 来组织页面切换的通用逻辑。
   - 只需定制每个页面的业务逻辑内容，将页面的切换交给框架来处理。
   - 框架提供了 **页面**的**堆栈**组织方式。打开页面，各种返回页面功能。为每个页面提供了一系列**生命周期方法**提供使用。打开和返回页面时只需要一句代码即可顺利切换页面。
      - **代码示例：**
         ``` c#
         // 打开 页面 NewPage
         UIManager.instance.OpenPanel("NewPage");
         // 返回 上一页面
         UIManager.instance.ReturnPanel();
         ```
   - V0.0292更新内容：
      - 修复 实用工具 AutoUIUtilities 读取CSV 在打包后失效问题。
      - 新增NUGet管理外部添加的dll库极其依赖
   - V0.0291更新内容：
      - 更新框架方法：OpenPanel 和 ReturnPanel，添加打开或关闭时，可以控制打开或关闭几层页面（默认为1层，更新不影响之前的打开关闭页面的方法调用）。
      - 实测 实用工具 AutoUIUtilities 读取CSV 在打包后失效，待后续修复
   - V0.029更新内容：
      - 添加实用工具 AutoUIUtilities 读取CSV（Comma Separated Value 逗号分隔符文件）基本读取功能。
   - V0.028更新内容：
      - 优化创建、修改Page流程，将挂载流程自动化，只需点击创建、修改即可自动完成包括挂载的所有流程。
      - 添加UDPServer脚本组件
      - 修复UDPClient脚本组件中后开服务端导致的客户端报错：*远程主机强迫关闭了一个现有的连接* 问题
   - V0.027更新内容：
      - 修复AutoUI页面管理器的 修改page名 功能 中 修改的页面的差错控制。
      - 修复ReturnPanel中 返回任意级别菜单、返回跳转 中跳转到本层的判断。
   - V0.026更新内容：
      - 添加AutoUI页面管理器的 修改page名 功能。
   - V0.025更新内容：
      - 新增UDP客户端发送和接收消息脚本组件。
      - 修复 “AutoUIUtilities.GetInfoForConfig” 实用工具获取配置文件时index越界问题。
      - 设定MainScene中预设的Canvas的CanvasScaler组件属性固定为1920*1080分辨率
   - V0.024更新内容：
      - 添加VideoPlayer解决画面残留的UGUI视频组件
      - 更改视频滑动组件视频的全屏适配
   - V0.023更新内容：
      - 添加框架功能：跳转到栈的某层，打开某页面
      - 文档添加 “页面栈” 的介绍说明
      - 添加命名空间说明
   - V0.022更新内容：
      - 添加预设页-待机页
      - StandbyPage
         - **功能:** 包含循环播放视频，点击跳转页面接口，配置文件设置时间自动返回待机页
   - V0.021更新内容：
      - 添加实用工具类读取配置文件
      - 添加页面模板组件
