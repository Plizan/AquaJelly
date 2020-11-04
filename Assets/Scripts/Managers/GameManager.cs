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
    [SerializeField] private int spawnTime;

    [Header("Player Info")]
    public int startLevel;
    public int maxLevel = 4;
    
    public float maxHealth;
    [Header("Player Field")]
    public PlayerCtrl playerCtrl;

    [Header("Camera Field")]
    [SerializeField] private CameraCtrl cameraCtrl;

    [Header("Background Field")]
    public BackgroundCtrl backgroundCtrl;

    [Header("Spanwer Field")]
    [SerializeField] private ObjectSpawner obstancleSpawner;
    #endregion

    #region CallbackFunction
    private void Start()
    {
        ProgressType = progressType;

        if (ProgressType == ProgressType.Game) GameStart();
    }

    private void Update()
    {
        if (progressType == ProgressType.Game) GameUpdate();
    }

    private void GameUpdate()
    {

    }

    public void OnPlay()
    {
        StartCoroutine(GameStart());
    }

    private IEnumerator GameStart()
    {
        ProgressType = ProgressType.Game;
        playerCtrl.Jump();

        playerCtrl.Level = startLevel;
        playerCtrl.Health = maxHealth / (maxLevel - 1) * startLevel;

        yield return new WaitForSeconds(.15f);
        backgroundCtrl.speed = speed;
        StartCoroutine(backgroundCtrl.Scroll());
        StartCoroutine(cameraCtrl.CameraFollow(playerCtrl.transform));
        StartCoroutine(playerCtrl.Movement());
        StartCoroutine(playerCtrl.HealthUpdate());
        StartCoroutine(obstancleSpawner.ObjectSpawn(spawnTime));
    }

    public void GameStop()
    {
        ProgressType = ProgressType.Lobby;

        StopAllCoroutines();
        obstancleSpawner.objectRemove();
    }
    #endregion
}
