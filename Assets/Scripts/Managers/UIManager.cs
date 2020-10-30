using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Field")]
    [SerializeField] private GameObject panelLobby;
    [SerializeField] private GameObject panelGame;

    public Text txtScore;
    public Image imgHealth;

    public void Initialization(ProgressType progressType)
    {
        for (int i = 0; i < panelLobby.transform.childCount; i++)
        {
            var animated = panelLobby.transform.GetChild(i).GetComponent<UIAnimated>();

            if (animated is null) panelLobby.transform.GetChild(i).gameObject.SetActive(false);
            else animated.Initialization(progressType);
        }
    }
}
