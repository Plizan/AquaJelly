using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class BackgroundCtrl : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private float difference = 0.7f;

    [HideInInspector] public float speed = 1;

    private Sprite[,] sprites = null;
        
    private void Awake()
    {
        LoadBackground();
    }

    private void LoadBackground()
    {
        sprites = new Sprite[Managers.Game.maxLevel, 5];    

        for(int i = 0; i < Managers.Game.maxLevel; i++)
        {
            
        }
    }

    public void SetBackgoundLevel(int level)
    {
        
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
}
