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

    private void Awake()
    {
        if (playOnAwake)
            StartCoroutine(StartAnimation());
    }

    public IEnumerator StartAnimation()
    {
        gameObject.SetActive(true);

        foreach (var i in sprites)
        {
            image.sprite = i;
            yield return new WaitForSeconds(delay);
        }

        if (isLoop)
            StartCoroutine(StartAnimation());
        else
            gameObject.SetActive(false);

    }
}
