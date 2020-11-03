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

    public float speed;

    [SerializeField] private int score;
    public int Score { get => score; set { score = value; Managers.UI.txtScore.text = score.ToString(); } }

    [Header("Camera Field")]
    [SerializeField] private CameraCtrl cameraCtrl;

    [Header("Player Info")]
    public PlayerCtrl playerCtrl;
    [SerializeField] private int level;
    [SerializeField] private int maxLevel = 4;
    public int Level { get => level; set { level = value; playerCtrl.Level = level; } }
    public float maxHealth;

    [Header("Background Field")]
    public BackgroundCtrl backgroundCtrl;
    #endregion

    #region CallbackFunction
    private void Start()
    {
        ProgressType = progressType;
        playerCtrl.Level = level;
        playerCtrl.Health = maxHealth / (maxLevel - 1) * level;
    }

    private void Update()
    {
        if (progressType == ProgressType.Game) GameUpdate();
    }

    private void GameUpdate()
    {
        if (Level > 1 && playerCtrl.Health < maxHealth / (maxLevel - 1) * (Level - 1))
            Level--;
        else if (Level < maxLevel && playerCtrl.Health > maxHealth / (maxLevel - 1) * Level)
            Level++;

        playerCtrl.Health -= Time.deltaTime * 3;

        Score++;

        if (playerCtrl.Health <= 0) GameStop();
    }

    public void OnPlay()
    {
        StartCoroutine(GameStart());
    }

    private IEnumerator GameStart()
    {
        ProgressType = ProgressType.Game;
        playerCtrl.Jump();

        yield return new WaitForSeconds(.15f);
        backgroundCtrl.speed = speed;
        StartCoroutine(backgroundCtrl.Scroll());
        StartCoroutine(cameraCtrl.CameraFollow(playerCtrl.transform));
        StartCoroutine(playerCtrl.Movement());
    }

    private void GameStop()
    {
        ProgressType = ProgressType.Lobby;

        StopAllCoroutines();
    }
    #endregion
}
