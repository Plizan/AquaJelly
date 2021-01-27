using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimated : MonoBehaviour
{
    [SerializeField] private ProgressType progressType = ProgressType.None;

    private Vector2 beginningPosition;


    private void Awake()
    {
        beginningPosition = GetComponent<RectTransform>().anchoredPosition;

        //if (progressType == ProgressType.None)
        //    progressType = transform.parent.GetComponent<UIAnimated>()?.progressType ?? ProgressType.None;

        //if (progressType == ProgressType.None)
        //    gameObject.SetActive(false);
    }

    public void Initialization(ProgressType progressType)
    {
        if (progressType == ProgressType.None) return;
        gameObject.SetActive(progressType == this.progressType);
    }
}