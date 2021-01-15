using DG.Tweening.Plugins.Core.PathCore;
using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SmalBox.AutoUI;

public class SecondMenu2Page : PanelBase
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
}