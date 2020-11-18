# 配置文件Config.txt 配置说明
## 定义规范
   - 配置文件以 ##属性名##:属性值 的方式配置。例如：
      - ##Time##:10
      - 表示 Time 这个属性值是 10
## 工程中使用说明
   - AutoUI 框架使用以下方法可读取属性
      ``` c#
      // 使用GetInfoForConfig方法获取属性
      string timeStr = AutoUIUtilities.GetInfoForConfig("Time");

      // 方法原型
      public static string GetInfoForConfig(string dataName)
      ```