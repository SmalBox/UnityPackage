using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("AutoUI/AutoUIImage")]
public class AutoUIImage : Image
{
    public string streamingAssetsImgPath;
    protected override void OnEnable()
    {
        base.OnEnable();
        // 读取图片
        if (streamingAssetsImgPath != null && streamingAssetsImgPath != "")
        {
            Texture2D tx = new Texture2D(0, 0);
            tx.LoadImage(
                System.IO.File.ReadAllBytes(Application.streamingAssetsPath + streamingAssetsImgPath)
                );
            Sprite mySprite = Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height), Vector2.zero);
            sprite = mySprite;
        }
    }
}
