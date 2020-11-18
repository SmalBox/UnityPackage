using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using RenderHeads.Media.AVProVideo;

[RequireComponent(typeof(ScrollRect))]
public class ClickSwitch : MonoBehaviour
{
    public GameObject content;
    [Space]
    public GameObject navigationBtn;
    public GameObject Btn0;  // 对应左、上
    public GameObject Btn1;  // 对应右、下
    [Space]
    public bool isVideo;
    public float elementLength = 1920;
    public int elementNum = 3;
    public bool horizontal = true;
    public float snapAnimTime = 0.5f;

    public Action<int> ContentIndexCallBack;
    [HideInInspector]
    public int ContentIndex
    {
        get
        {
            return contentIndex;
        }
        set
        {
            contentIndex = value;
            // 打开两个按钮
            Btn0.SetActive(true);
            Btn1.SetActive(true);
            if (contentIndex == 0)
            {
                // 关闭Btn0
                Btn0.SetActive(false);
            }
            if (contentIndex == elementNum - 1)
            {
                // 关闭Btn1
                Btn1.SetActive(false);
            }
            ContentIndexCallBack?.Invoke(contentIndex);
        }
    }

    private int contentIndex;


    private void Start()
    {
        ContentIndex = 0;
        PlayCurrVideo();
    }

    public void Btn0Event()
    {
        if (horizontal)
        {
            // 开始滑动
            float aimPos = content.GetComponent<RectTransform>().localPosition.x + elementLength;
            content.transform.DOLocalMoveX(aimPos, snapAnimTime)
                .OnComplete(()=>
                {
                    // 设置导航索引
                    ContentIndex--;
                    PlayCurrVideo();
                    Btn0.GetComponent<Button>().interactable = true;
                });
        }else
        {
            // 开始滑动
            float aimPos = content.GetComponent<RectTransform>().localPosition.y - elementLength;
            content.transform.DOLocalMoveY(aimPos, snapAnimTime)
                .OnComplete(()=>
                {
                    // 设置导航索引
                    ContentIndex--;
                    PlayCurrVideo();
                    Btn0.GetComponent<Button>().interactable = true;
                });
        }
    }
    public void Btn1Event()
    {
        if (horizontal)
        {
            // 开始滑动
            float aimPos = content.GetComponent<RectTransform>().localPosition.x - elementLength;
            content.transform.DOLocalMoveX(aimPos, snapAnimTime)
                .OnComplete(()=>
                {
                    // 设置导航索引
                    ContentIndex++;
                    PlayCurrVideo();
                    // 滑动结束后打开按钮可点击
                    Btn1.GetComponent<Button>().interactable = true;
                });
        }else
        {
            // 开始滑动
            float aimPos = content.GetComponent<RectTransform>().localPosition.y + elementLength;
            content.transform.DOLocalMoveY(aimPos, snapAnimTime)
                .OnComplete(()=>
                {
                    // 设置导航索引
                    ContentIndex++;
                    PlayCurrVideo();
                    // 滑动结束后打开按钮可点击
                    Btn1.GetComponent<Button>().interactable = true;
                });
        }
    }

    // 停止所有视频
    private void StopAllVideo()
    {
        for (int i = 1; i < content.GetComponentsInChildren<Transform>().Length; i++)
        {
            if (content.GetComponentsInChildren<Transform>()[i].GetComponent<MediaPlayer>())
            {
                content.GetComponentsInChildren<Transform>()[i].GetComponent<MediaPlayer>().Rewind(true);
            }
        }
    }
    // 播放指定索引视频
    private void PlayIndexVideo(int index)
    {
        Debug.Log("播放: " + index);
        if (content.transform.GetChild(index).GetComponentInChildren<MediaPlayer>())
        {
            content.transform.GetChild(index).GetComponentInChildren<MediaPlayer>().Play();
        }
    }

    // 播放当前视频
    private void PlayCurrVideo()
    {
        if (isVideo)
        {
            // 停止其他视频
            StopAllVideo();
            // 播放当前视频
            PlayIndexVideo(ContentIndex);
            // 滑动结束后打开按钮可点击
        }
    }

}
