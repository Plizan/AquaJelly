using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Field")]
    [SerializeField] private GameObject panelLobby;
    [SerializeField] private GameObject panelGame;

    [Header("Lobby")]
    [SerializeField] private Image imgCombo;
    public int SetCombo
    {
        set
        {
            value = Mathf.Min(combos.Length - 1, value);
            imgCombo.gameObject.SetActive(value > 0);

            if (imgCombo.gameObject.activeSelf)
                imgCombo.sprite = combos[value];
        }
    }

    public Text txtHighScore;
    public Text txtNowScore;
    public Text txtStage;
    public Image imgHealth;

    [SerializeField] private Sprite[] combos;

    [Header("Game")]
    public Text txtScore;

    private List<UIAnimated> allUI = new List<UIAnimated>();

    private void Awake()
    {
        GetUIAnimateds(panelLobby).ForEach((i) => allUI.Add(i));
        GetUIAnimateds(panelGame).ForEach((i) => allUI.Add(i));
    }

    private List<UIAnimated> GetUIAnimateds(GameObject obj)
    {
        var tempList = new List<UIAnimated>();

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            tempList.Add(obj.transform.GetChild(i).GetComponent<UIAnimated>());
        }

        return tempList;
    }

    public void Initialization(ProgressType progressType)
    {
        allUI.ForEach((i) => i.Initialization(progressType));
    }
}