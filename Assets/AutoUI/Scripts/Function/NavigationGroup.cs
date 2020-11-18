using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationGroup : MonoBehaviour
{
    public GameObject scrollVidew;

    private Color initColor;

    private void Awake()
    {
        initColor = transform.GetChild(0).GetComponent<Image>().color;
        scrollVidew.GetComponent<SwipeSwitch>().ContentIndexCallBack += SetNavigationHighlight;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // 根据索引设置导航点索引
    private void SetNavigationHighlight(int index)
    {
        InitAllNavigationHighlight();
        transform.GetChild(index).GetComponent<Image>().color =
            new Color(initColor.r, initColor.g, initColor.b, 1);
    }

    // 初始化所有颜色
    private void InitAllNavigationHighlight()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().color = initColor;
        }
    }
}
