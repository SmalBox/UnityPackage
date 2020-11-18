using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVar : MonoBehaviour
{
    public static GlobalVar instance;
    private GlobalVar() { }
    private void Awake()
    {
        if (instance == null) instance = GetComponent<GlobalVar>();
    }

    [HideInInspector]
    // 屏幕点击次数
    public int screenClickTimes = 0;
    [HideInInspector]
    // 全局倒计时时间
    public float globalCountdown = 0;
    [HideInInspector]
    public bool startAutoReturnStandbyCheck = false;

    [HideInInspector]
    // 视频控制器
    public GameObject videoDisplay;

    // 打开和关闭动画时间
    public const float animTimeOfOpenClosePage = 1f;
    public const float animTimeOfClosePage = 0.5f;


    #region 数据结构
    // AutoUIPagesPathDict
    public class PagesPathDict
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
    #endregion

}
