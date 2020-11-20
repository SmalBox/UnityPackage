using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace SmalBox.AutoUI
{
    public class PanelBase : MonoBehaviour
    {
        [NonSerialized]
        // 皮肤路径
        public string skinPath;
        [NonSerialized]
        // 皮肤
        public GameObject skin;
        // 层级
        public UIManager.PanelLayer layer = UIManager.PanelLayer.Panel;
        //面板参数
        public object[] args;

        #region 生命周期
        // 初始化
        public virtual void Init(params object[] args)
        {
            this.args = args;
        }
        // 面板开启前
        public virtual void OnShowing()
        {

        }
        // 面板开启动画
        public virtual void OnShowAnim()
        {

        }
        // 面板开启后
        public virtual void OnShowedAsync()
        {

        }
        // 帧更新
        public virtual void Update()
        {

        }
        // 关闭前
        public virtual void OnClosing()
        {

        }
        // 关闭动画
        public virtual void OnClosingAnim()
        {

        }
        // 关闭后
        public virtual void OnClosed()
        {

        }
        #endregion
    }
}
