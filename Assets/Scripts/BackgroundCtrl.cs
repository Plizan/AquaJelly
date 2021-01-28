using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class BackgroundCtrl : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;

    public Material[] GetMatarials { get
    {
        var mat = new Material[renderers.Length];
        
        for (int i = 0; i < mat.Length; i++)
        {
            mat[i] = renderers[i].material;
        }

        return mat;
    }}
    
    [SerializeField] private float difference = 0.7f;

    [HideInInspector] public float speed = 1;

    private Sprite[,] sprites = null;

    [SerializeField] private GameObject feverObj;

    private void Awake()
    {
        LoadBackground();
    }

    private void LoadBackground()
    {//TODO
        sprites = new Sprite[Managers.Game.maxLevel, 5];

        for (int i = 0; i < Managers.Game.maxLevel; i++)
        {

        }
    }

    public void SetBackgoundLevel(int level)
    {

    }

    public void SetFever(bool isFever)
    {
        feverObj.SetActive(isFever);
    }

    public IEnumerator Scroll()
    {
        while (true)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.mainTextureOffset -= new Vector2(Time.deltaTime * (i + 1) * speed * difference, 0f);
            }

            yield return null;
        }
    }

    public static Texture ConvertSpriteToTexture(Sprite sprite)
    {
        try
        {
            if (sprite.rect.width != sprite.texture.width)
            {
                int x = Mathf.FloorToInt(sprite.textureRect.x);
                int y = Mathf.FloorToInt(sprite.textureRect.y);
                int width = Mathf.FloorToInt(sprite.textureRect.width);
                int height = Mathf.FloorToInt(sprite.textureRect.height);

                Texture2D newText = new Texture2D(width, height);
                Color[] newColors = sprite.texture.GetPixels(x, y, width, height);

                newText.SetPixels(newColors);
                newText.Apply();
                return newText;
            }
            else
                return sprite.texture;
        }
        catch
        {
            return sprite.texture;
        }
    }
}
