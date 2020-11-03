using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class BackgroundCtrl : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private float difference = 0.7f;

    [HideInInspector] public float speed = 1;

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
