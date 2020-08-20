# UnityPackage

   - 常用插件打包
   - 插件包或脚本在不同分支，拉取相应分支即可

## CameraFilterPack_Blend2Camera_BOverlapA

   - Describe：Unity 相机特效，将相机2中渲染的图像覆盖叠加到相机1渲染的画面上。
   - Author：SmalBox
   - Blog：https://smalbox.top
   - GitHub：https://GitHub.com/SmalBox
   - Sketch：本脚本可以 **强制相机2渲染的画面覆盖在相机1上** ，忽略相机深度，越过Unity的默认深度的渲染机制。 **可用于XR项目中SDK不支持多相机叠加渲染问题。** (实际上此脚本相当于手动实现了Unity自带的画面叠加渲染效果)
   - Usage：
      - 将cs脚本挂载到相机1（主相机）上
      - 将相机2挂载到脚本的属性里
      - 相机2的ClearFlags设置为SolidColor，Background设置为黑色透明度为0
