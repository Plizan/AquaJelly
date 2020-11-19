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

    public float backgroundSpeed;

    [SerializeField] private int score;
    public int Score { get => score; set { score = value; Managers.UI.txtScore.text = score.ToString(); } }
    [SerializeField] private int spawnTime;

    [SerializeField] private int feverTime;

    public bool isFeverTime = false;

    [Header("Player Info")]
    public int maxLevel = 4;
    public float maxHealth;

    [SerializeField] private int startLevel;
    [SerializeField] private float playerSpeed;
    [SerializeField] private int playerJumpCount;
    [SerializeField] private float playerJumpPower;
    [SerializeField] private float playerDotDamage;

    [Header("Camera Info")]
    [Range(-3, 3)] public float cameraXValue = 0f;
    //public float cameraSpeed = 1f;

    [Header("Player Field")]
    public PlayerCtrl playerCtrl;

    [Header("Camera Field")]
    public CameraCtrl cameraCtrl;

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

    private void Initialization(ProgressType progressType)
    {
        ProgressType = progressType;

        switch (progressType)
        {
            case ProgressType.Game:
                {
                    SetPlayer();
                    SetCamera();
                    SetBackground();
                    return;
                }
            case ProgressType.Lobby:
                {
                    return;
                }
        }
    }

    public void SetPlayer()
    {
        playerCtrl.Level = startLevel;
        playerCtrl.speed = playerSpeed;
        playerCtrl.power = playerJumpPower;
        playerCtrl.maxJumpCount = playerJumpCount;
        playerCtrl.dotDamage = playerDotDamage;
    }

    public void SetCamera()
    {
        cameraCtrl.xValue = cameraXValue;
    }

    public void SetBackground()
    {
        backgroundCtrl.speed = backgroundSpeed;
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
        Initialization(ProgressType.Game);

        Camera.main.DOOrthoSize(1.777778f, .5f).SetEase(Ease.InSine);
        yield return new WaitForSeconds(.4f);

        playerCtrl.Jump();
        playerCtrl.SetLevelOrHealth(startLevel);
        obstancleSpawner.ObjectInstantiate();

        yield return new WaitForSeconds(.15f);

        backgroundCtrl.speed = backgroundSpeed;
        StartCoroutine(backgroundCtrl.Scroll());
        StartCoroutine(cameraCtrl.CameraFollow(playerCtrl.transform));
        playerCtrl.Initialization(ProgressType);
        //StartCoroutine(obstancleSpawner.ObjectSpawn(spawnTime));
    }

    public void GameStop()
    {
        ProgressType = ProgressType.Lobby;

        playerCtrl.Initialization(ProgressType);

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
