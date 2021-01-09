using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using RenderHeads.Media.AVProVideo;

/// <summary>
/// 使用说明：
/// 1. 显示的内容放在Viewport下的Content中。
/// 1.1 SwipeSwitch组件属性中，通过ElementLength配置每个内容长度，ElementNum配置有几个内容。
/// 1.2 如果有视频勾选IsVideo可以在滑动到相应视频内容时播放视频。
/// 1.3 设置视频，在Content下添加或减少播放内容，将其下的AVProVideo配置视频路径，并将其按照顺序添加到
/// SwipeSwitch的LoadingVideoGroup中。
/// 2. 左右滑动提示按钮 NavigationBtn
/// 2.1 子对象中有两个Image，作为滑动提示，可以设置图片、做Animation动画。
/// 3. 当前进度导航 NavigationGroupH
/// 3.1 子对象中添加了Image，这个组件会根据内容当年前的索引给Image变色。这里的效果处理可以使用下面提供的
/// ContentIndexCallBack，注册这个delegate可以做相应的动作，具体可以看预设好的变色示例
/// </summary>
[RequireComponent(typeof(ScrollRect))]
public class SwipeSwitch : MonoBehaviour
{
    public GameObject content;
    public GameObject scrollbar;
    public GameObject scrollRect;
    [Space]
    public GameObject btn0; // 左导航标志
    public GameObject btn1; // 右导航标志
    [Space]
    public bool isVideo;
    public float elementLenth = 1920;
    public int elementNum = 3;
    public bool horizontal = true;
    public float snapAnimTime = 0.5f;
    [Space]
    public MediaPlayer[] loadingVideoGroup;
    private List<string> loadingVideoGroupPath;

    // 内容事件回调
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
            if (btn0 != null && btn1 != null)
            {
                // 打开两个按钮
                btn0.SetActive(true);
                btn1.SetActive(true);
                if (contentIndex == 0)
                {
                    // 关闭Btn0
                    btn0.SetActive(false);
                }
                if (contentIndex == elementNum - 1)
                {
                    // 关闭Btn1
                    btn1.SetActive(false);
                }
            }
            ContentIndexCallBack?.Invoke(contentIndex);
        }
    }

    private int contentIndex;
    private Vector2 pos;
    private float startDis;
    private float startBarValue;
    private bool canDrag;
    private bool canDragEnd;


    private void Start()
    {
        // 根据loadingVideoGroup初始化loadingVideoGroupPath
        loadingVideoGroupPath = new List<string>();
        foreach (var item in loadingVideoGroup)
        {
            loadingVideoGroupPath.Add(item.m_VideoPath);
        }
        // 根据方向调整控件
        if (horizontal)
        {
            scrollRect.GetComponent<ScrollRect>().vertical = false;
            scrollRect.GetComponent<ScrollRect>().horizontal = true;
            startDis = content.transform.position.x;
        }
        else
        {
            scrollRect.GetComponent<ScrollRect>().vertical = true;
            scrollRect.GetComponent<ScrollRect>().horizontal = false;
            startDis = content.transform.position.y;
        }

        ContentIndex = 0;

        canDrag = true;
        canDragEnd = true;
        pos = new Vector2();
        startBarValue = scrollbar.GetComponent<Scrollbar>().value;

        if (isVideo)
        {
            // 打开第一个开始播放
            PlayIndexVideo(ContentIndex);
        }
    }
    public void StartDrag()
    {
        if (canDrag)
        {
            if (horizontal)
            {
                //startDis = content.transform.position.x;
                startDis = content.GetComponent<RectTransform>().localPosition.x;
                scrollRect.GetComponent<ScrollRect>().horizontal = true;
            }
            else
            {
                startDis = content.GetComponent<RectTransform>().localPosition.y;
                scrollRect.GetComponent<ScrollRect>().vertical = true;
            }

            canDrag = false;
            pos = Input.mousePosition;
            startBarValue = scrollbar.GetComponent<Scrollbar>().value;
        }
    }
    public void EndDrag()
    {
        if (canDragEnd && !canDrag)
        {
            if (horizontal)
            {
                if (Input.mousePosition.x - pos.x > 0 && startBarValue > 0.1f)
                {
                    // 向右滑动
                    scrollRect.GetComponent<ScrollRect>().horizontal = false;
                    content.transform.DOLocalMoveX(startDis + elementLenth, snapAnimTime)
                        .OnStart(() =>
                        {
                            canDragEnd = false;
                        })
                        .OnComplete(() =>
                        {
                            ContentIndex--;
                            if (isVideo)
                            {
                                // 停止其他视频
                                StopAllVideo();
                                // 播放当前视频
                                PlayIndexVideo(ContentIndex);
                            }
                            canDragEnd = true;
                            canDrag = true;
                        });
                }
                else if (Input.mousePosition.x - pos.x < 0 && startBarValue < 0.9f)
                {
                    // 向左滑动
                    scrollRect.GetComponent<ScrollRect>().horizontal = false;
                    content.transform.DOLocalMoveX(startDis - elementLenth, snapAnimTime)
                        .OnStart(() =>
                        {
                            canDragEnd = false;
                        })
                        .OnComplete(() =>
                        {
                            ContentIndex++;
                            if (isVideo)
                            {
                                // 停止其他视频
                                StopAllVideo();
                                // 播放当前视频
                                PlayIndexVideo(ContentIndex);
                            }
                            canDragEnd = true;
                            canDrag = true;
                        });
                }
                else
                {
                    canDrag = true;
                }
            }
            else
            {
                if (Input.mousePosition.y - pos.y > 0 && startBarValue != 0)
                {
                    // 向上滑动
                    scrollRect.GetComponent<ScrollRect>().vertical = false;
                    content.transform.DOLocalMoveY(startDis + elementLenth, snapAnimTime)
                        .OnStart(() =>
                        {
                            canDragEnd = false;
                        })
                        .OnComplete(() =>
                        {
                            ContentIndex++;
                            if (isVideo)
                            {
                                // 停止其他视频
                                StopAllVideo();
                                // 播放当前视频
                                PlayIndexVideo(ContentIndex);
                            }
                            canDragEnd = true;
                            canDrag = true;
                        });
                }
                else if (Input.mousePosition.y - pos.y < 0 && startBarValue != 1)
                {
                    // 向下滑动
                    scrollRect.GetComponent<ScrollRect>().vertical = false;
                    content.transform.DOLocalMoveY(startDis - elementLenth, snapAnimTime)
                        .OnStart(() =>
                        {
                            canDragEnd = false;
                        })
                        .OnComplete(() =>
                        {
                            ContentIndex--;
                            if (isVideo)
                            {
                                // 停止其他视频
                                StopAllVideo();
                                // 播放当前视频
                                PlayIndexVideo(ContentIndex);
                            }
                            canDragEnd = true;
                            canDrag = true;
                        });
                }
                else
                {
                    canDrag = true;
                }

            }
        }
    }

    // 停止所有视频
    private void StopAllVideo()
    {
        for (int i = 1; i < content.GetComponentsInChildren<Transform>().Length; i++)
        {
            if (content.GetComponentsInChildren<Transform>()[i].GetComponent<MediaPlayer>())
            {
                content.GetComponentsInChildren<Transform>()[i].GetComponent<MediaPlayer>().CloseVideo();
            }
        }
    }
    // 播放指定索引视频
    private void PlayIndexVideo(int index)
    {
        if (content.transform.GetChild(index).GetComponentInChildren<MediaPlayer>())
        {
            content.transform.GetChild(index).GetComponentInChildren<MediaPlayer>()
            .OpenVideoFromFile(
                MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder,
                loadingVideoGroupPath[index]);
        }
    }
}