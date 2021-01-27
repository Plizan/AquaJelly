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

    public void StartAnimation()
    {
        if (coroutine != null) return;

        gameObject.SetActive(true);
        coroutine = StartCoroutine(CoroutineStartAnimation());
    }
    
    public IEnumerator CoroutineStartAnimation()
    {
        gameObject.SetActive(true);

        foreach (var i in sprites)
        {
            image.sprite = i;
            yield return new WaitForSeconds(delay);
        }

        if (isLoop)
            coroutine = StartCoroutine(CoroutineStartAnimation());
        else
            gameObject.SetActive(false);

        if (coroutine != null) coroutine = null;
    }
}
