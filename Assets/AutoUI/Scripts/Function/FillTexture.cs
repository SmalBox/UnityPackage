/*
 * 打开关闭对象时，自动挂载和卸载RenderTexture，解决videoPlayer的视频残留问题。
 * renderingWidth、renderingHeight 渲染宽、高分辨率。小于实际分辨率则画面质量下降，高于则浪费内存。
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

namespace SmalBox.AutoUI
{
    [RequireComponent(typeof(RawImage))]
    [RequireComponent(typeof(VideoPlayer))]
    public class FillTexture : MonoBehaviour
    {
        public int renderingWidth = 1920;
        public int renderingHeight = 1080;

        private RenderTexture temp;
        private void OnEnable()
        {
            // 生成临时 RenderTexture
            temp = RenderTexture.GetTemporary(
                renderingWidth,
                renderingHeight,
                0,
                RenderTextureFormat.ARGB32,
                RenderTextureReadWrite.sRGB);
            // 设置纹理到RawImage和Video上
            gameObject.GetComponent<VideoPlayer>().targetTexture = temp;
            gameObject.GetComponent<RawImage>().texture = temp;

            Debug.Log(gameObject);
        }

        private void OnDisable()
        {
            // 销毁临时 RenderTexture
            temp = null;
            // 取消设置纹理到RawImage和Video上
            //gameObject.GetComponent<VideoPlayer>().targetTexture = null;
            gameObject.GetComponent<RawImage>().texture = null;
        }
    }
}
