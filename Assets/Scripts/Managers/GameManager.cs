using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ProgressType
{
    Lobby = 0,
    Game,

    None,
}

public class GameManager : MonoBehaviour
{
    #region Field Values
    [Header("Game Info")]
    [SerializeField] private ProgressType progressType;
    public ProgressType ProgressType { get => progressType; set { progressType = value; Managers.UI.Initialization(progressType); } }

    [SerializeField] private int score;
    public int Score { get => score; set { score = value; Managers.UI.txtScore.text = score.ToString(); } }

    [Header("Player Info")]
    public PlayerCtrl playerCtrl;
    [SerializeField] private int level;
    public int Level { get => level; set { level = value; playerCtrl.Level = level; } }
    public float maxHealth;

    [Header("Background Field")]
    public GameObject[] backgrounds;
    #endregion

    #region CallbackFunction
    private void Start()
    {
        ProgressType = progressType;
        beginningCameraPosition = Camera.main.transform.position;
        playerCtrl.Level = level;
    }

    Vector3 beginningCameraPosition;
    int backgroundIndex = 0;
    private void Update()
    {
        Vector3 temp = beginningCameraPosition;
        temp.x += playerCtrl.transform.position.x;
        Camera.main.transform.position = temp;

        //if (Camera.main.transform.position.x > 6.5f * (backgroundIndex + 1))
        //{
        //    var pos = backgrounds[backgroundIndex % 3].transform.position;
        //    pos.x += 6.5f * backgrounds.Length;
        //    backgrounds[backgroundIndex % 3].transform.position = pos;

        //    backgroundIndex++;
        //}
    }
    #endregion
}
