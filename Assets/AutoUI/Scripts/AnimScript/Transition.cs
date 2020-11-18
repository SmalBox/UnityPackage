using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Transition : MonoBehaviour
{
    // 渐变动画
    public static void Show(GameObject gameObject, float showTime = GlobalVar.animTimeOfOpenClosePage)
    {
        if (gameObject.GetComponent<CanvasGroup>() == null)
        {
            gameObject.AddComponent<CanvasGroup>();
        }
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        gameObject.SetActive(true);
        DOTween.To(() => gameObject.GetComponent<CanvasGroup>().alpha,
            x => gameObject.GetComponent<CanvasGroup>().alpha = x,
            1, showTime);
    }

    public static void Hide(GameObject gameObject, float hideTime = GlobalVar.animTimeOfClosePage)
    {
        if (gameObject.GetComponent<CanvasGroup>() == null)
        {
            gameObject.AddComponent<CanvasGroup>();
        }
        //gameObject.GetComponent<CanvasGroup>().alpha = 1;
        DOTween.To(() => gameObject.GetComponent<CanvasGroup>().alpha,
            x => gameObject.GetComponent<CanvasGroup>().alpha = x,
            0, hideTime).OnComplete(delegate
            {
                gameObject.SetActive(false);
            });
    }

    // 缩放动画
    public static void ShowZoom(GameObject gameObject)
    {
        if (gameObject.GetComponent<RectTransform>() == null)
            gameObject.AddComponent<RectTransform>();
        gameObject.GetComponent<RectTransform>().localScale = Vector3.zero;
        gameObject.SetActive(true);
        gameObject.transform.DOScale(Vector3.one, GlobalVar.animTimeOfOpenClosePage);
    }

    public static void HideZoom(GameObject gameObject)
    {
        if (gameObject.GetComponent<RectTransform>() == null)
            gameObject.AddComponent<RectTransform>();
        gameObject.transform.DOScale(Vector3.zero, GlobalVar.animTimeOfOpenClosePage).OnComplete(delegate
        {
            gameObject.SetActive(false);
        });
    }

    // 直接切换
    public static void DirectShow(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
    public static void DirectHide(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

}
