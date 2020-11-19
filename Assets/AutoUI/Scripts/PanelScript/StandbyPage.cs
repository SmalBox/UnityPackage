using DG.Tweening.Plugins.Core.PathCore;
using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandbyPage : PanelBase
{
    public MediaPlayer mediaPlayer;

    #region 生命周期
    public override void Init(params object[] args)
    {
        base.Init(args);
    }
    public override void OnShowing()
    {
        base.OnShowing();
        // 读取配置文件自动返回时间
        GlobalVar.instance.globalCountdown = System.Convert.ToInt32(AutoUIUtilities.GetInfoForConfig("AutoReturnTime"));

        // 打开待机页时关闭自动返回检测
        GlobalVar.instance.startAutoReturnStandbyCheck = false;
        Debug.Log("关闭自动返回检测");
        mediaPlayer.Play();
    }
    public override void OnShowAnim()
    {
        base.OnShowAnim();
        Transition.DirectShow(skin);
    }
    public override void Update()
    {
        base.Update();
    }
    public override void OnClosing()
    {
        base.OnClosing();
        // 读取配置文件自动返回时间
        GlobalVar.instance.globalCountdown = System.Convert.ToInt32(AutoUIUtilities.GetInfoForConfig("AutoReturnTime"));

        // 关闭面板过程中打开返回待机页检测
        GlobalVar.instance.startAutoReturnStandbyCheck = true;
        Debug.Log("打开自动返回检测");

        mediaPlayer.Control.Rewind();
    }
    public override void OnClosingAnim()
    {
        base.OnClosingAnim();
        Transition.DirectHide(skin);
    }
    public override void OnClosed()
    {
        base.OnClosed();
    }
    #endregion

    #region 面板方法
    public void ClickOpenMainMenu()
    {
        UIManager.instance.OpenPanel("MainMenuPage");
        mediaPlayer.Control.Rewind();
        mediaPlayer.Control.Pause();
    }
    #endregion
}