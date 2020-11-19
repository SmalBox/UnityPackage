# UnityPackage

   - 常用插件打包
   - 插件包或脚本在不同分支，拉取相应分支即可

## TimeControl

   - 控制软件使用期限

## CameraFilterPack_Blend2Camera_BOverlapA

   - 镜头特效，将相机2的画面覆盖混合到相机1的画面，越过Unity的相机深度渲染机制，强制覆盖混合相机2画面到相机1上

## AutoUI

   - 当前最新版本 **AutoUI-V0.022**
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
   - V0.022更新内容：
      - 添加预设页-待机页
      - StandbyPage
         - **功能:** 包含循环播放视频，点击跳转页面接口，配置文件设置时间自动返回待机页
   - V0.021更新内容：
      - 添加实用工具类读取配置文件
      - 添加页面模板组件