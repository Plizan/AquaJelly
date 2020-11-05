using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

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

    [SerializeField] private int feverTime;

    public bool isFeverTime = false;

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
    public ObjectSpawner obstancleSpawner;

    [Header("Fever Field")]
    [SerializeField] private GameObject backFeverObj;
    [SerializeField] private GameObject frontFeverObj;
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

        playerCtrl.SetLevelOrHealth(startLevel);

        yield return new WaitForSeconds(.15f);
        backgroundCtrl.speed = speed;
        StartCoroutine(backgroundCtrl.Scroll());
        StartCoroutine(cameraCtrl.CameraFollow(playerCtrl.transform));
        StartCoroutine(playerCtrl.Movement());
        StartCoroutine(playerCtrl.HealthUpdate());
        //StartCoroutine(obstancleSpawner.ObjectSpawn(spawnTime));
    }

    public void GameStop()
    {
        ProgressType = ProgressType.Lobby;

        StopAllCoroutines();
        obstancleSpawner.objectRemove();
    }

    public IEnumerator FeverMode()
    {
        isFeverTime = true;

        backFeverObj.SetActive(true);
        frontFeverObj.SetActive(true);

        backFeverObj.transform.DOShakePosition(feverTime);
        frontFeverObj.transform.DOShakePosition(feverTime);

        for (float f = feverTime; f >= 0; f -= Time.deltaTime)
        {
            Managers.UI.imgHealth.fillAmount = f / feverTime;

            yield return null;
        }

        backFeverObj.SetActive(false);
        frontFeverObj.SetActive(false);

        obstancleSpawner.objectRemove();
        obstancleSpawner.ObjectInstantiate();

        playerCtrl.SetLevelOrHealth(maxLevel - 2);

        isFeverTime = false;
    }
    #endregion
}
