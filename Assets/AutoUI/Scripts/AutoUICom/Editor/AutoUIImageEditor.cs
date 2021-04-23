using UnityEngine;
using UnityEditor;
using SFB;

[CustomEditor(typeof(AutoUIImage))]
public class AutoUIImageEditor : Editor
{
    AutoUIImage image;
    private void OnEnable()
    {
        image = (AutoUIImage)target;
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();

        image.sprite = (Sprite)EditorGUILayout.ObjectField("SourceImage", image.sprite, typeof(Sprite), true);
        image.color = EditorGUILayout.ColorField("Color", image.color);
        image.material = (Material)EditorGUILayout.ObjectField("Material", image.material, typeof(Material), true);
        image.raycastTarget = EditorGUILayout.Toggle("RaycastTarget", image.raycastTarget);

        EditorGUILayout.BeginHorizontal();
        image.streamingAssetsImgPath = EditorGUILayout.TextField("StreamingAssetsImagtPath", image.streamingAssetsImgPath);
        if (GUILayout.Button("选择图片"))
        {
            string[] selectedImg = StandaloneFileBrowser.OpenFilePanel("选择外部配置图片", Application.streamingAssetsPath, "png", false);
            if (selectedImg.Length > 0)
            {
                string fullName = selectedImg[0].Replace("\\", "/").Replace(Application.streamingAssetsPath, "");
                image.streamingAssetsImgPath = fullName;
                // 读取图片
                if (image.streamingAssetsImgPath != "")
                {
                    Texture2D tx = new Texture2D(0, 0);
                    tx.LoadImage(
                        System.IO.File.ReadAllBytes(Application.streamingAssetsPath + image.streamingAssetsImgPath)
                        );
                    Sprite mySprite = Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height), Vector2.zero);
                    image.sprite = mySprite;
                }
            }
	        GUIUtility.ExitGUI();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
}
