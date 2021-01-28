using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimation : MonoBehaviour
{
    public Image image;//TODO

    public Sprite[] sprites;

    public float delay = 0.1f;
    public bool isLoop = false;
    public bool playOnAwake = false;

    private Coroutine coroutine;
    
    private void Awake()
    {
        if (playOnAwake)
            coroutine = StartCoroutine(CoroutineStartAnimation());
    }

    public void StartAnimation(Action action = null, int frame = 0)
    {
        if (coroutine != null) return;

        gameObject.SetActive(true);
        coroutine = StartCoroutine(CoroutineStartAnimation(action, frame));
    }
    
    public IEnumerator CoroutineStartAnimation(Action action = null, int frame = 0)
    {
        gameObject.SetActive(true);

        for (int i = 0; i < sprites.Length; i++)
        {
            image.sprite = sprites[i];

            if (i != 0 && i > frame)
            {
                action?.Invoke();
                action = null;
            }
            
            yield return new WaitForSeconds(delay);
        }

        if (isLoop)
            coroutine = StartCoroutine(CoroutineStartAnimation());
        else
            gameObject.SetActive(false);

        if (coroutine != null) coroutine = null;
        
        action?.Invoke();
    }
}
