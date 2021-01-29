using System;
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
    public ProgressType ProgressType
    {
        get => progressType;
        set
        {
            progressType = value;
            Managers.UI.Initialization(progressType);
        }
    }

    public float backgroundSpeed;
    public int stageCombo;
    public int maxStageLevel;
    public int stageLevel;
    public float endXLength;

    public int secondScore;

    private bool isPlaying;
    
    [SerializeField] private int score;
    [SerializeField] private int highScore;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            highScore = Mathf.Max(score, highScore);
            Managers.UI.txtScore.text = score.ToString();
        }
    }
    [SerializeField] private int spawnTime;

    [SerializeField] private float feverTime;
    public float invincibilityTime;

    [SerializeField] private float throwPower;

    public bool isFeverTime = false;

    [Header("Player Info")]
    public int maxLevel = 4;
    public float maxHealth;

    [SerializeField] private int startLevel;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerFeverSpeedMagnification;
    [SerializeField] private int playerJumpCount;
    [SerializeField] private float playerJumpPower;
    [SerializeField] private float playerDotDamage;

    [Header("Camera Info")]
    [Range(-3, 3)] public float cameraXValue = 0f;
    private Coroutine coroutineCameraFollow;
    //public float cameraSpeed = 1f;

    [Header("Player Field")]
    public PlayerCtrl playerCtrl;
    public Sprite feverSprite;

    [Header("Camera Field")]
    public CameraCtrl cameraCtrl;

    [Header("Background Field")]
    public BackgroundCtrl backgroundCtrl;

    [Header("Spanwer Field")]
    public ObjectSpawner obstancleSpawner;

    [Header("Fever Field")]
    [SerializeField] private SpriteAnimation feverAnim;

    #endregion

    #region Function

    private void Awake()
    {
        DataLoad();

        if (isPlaying)
        {
            stageLevel = 1;
            Score = 0;
        }
    }

    private void Start()
    {
        Managers.Sound.DefaultPlay();

        Initialization(ProgressType);
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
                break;
            }
            case ProgressType.Lobby:
            {
                Managers.UI.SetCombo = stageLevel - 2;
                Managers.UI.txtHighScore.text = $"HIGH SCORE : {highScore}";
                Managers.UI.txtNowScore.text = $"SCORE : {score}";
                Managers.UI.txtStage.text = $"STAGE : {stageLevel}";
                Managers.Map.SetFloors(stageLevel);
                Managers.Map.DeleteLevel();
                break;
            }
        }

        DataSave();
    }

    public void SetPlayer()
    {
        playerCtrl.Level = startLevel;
        playerCtrl.speed = playerSpeed;
        playerCtrl.power = playerJumpPower;
        playerCtrl.maxJumpCount = playerJumpCount;
        playerCtrl.dotDamage = playerDotDamage;
        playerCtrl.isFirst = true; //TODO
    }

    public void SetCamera()
    {
        cameraCtrl.xValue = cameraXValue;
    }

    public void SetBackground()
    {
        backgroundCtrl.speed = backgroundSpeed;
    }

    private bool isClear = false; //TODO
    private void GameUpdate()
    {
        if (!isClear && playerCtrl.transform.position.x > endXLength) StartCoroutine(GameClear());
    }

    public void OnPlay()
    {
        StartCoroutine(GameStart());
    }

    private IEnumerator SecondScore()
    {
        //TODO

        while (true)
        {
            Score += secondScore + stageLevel;
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator GameStart()
    {
        Initialization(ProgressType.Game);

        Camera.main.DOOrthoSize(1.777778f, .5f).SetEase(Ease.InSine);
        yield return new WaitForSeconds(.4f);

        isPlaying = true;
        playerCtrl.isJump = true;
        playerCtrl.SetLevelOrHealth(startLevel);
        //obstancleSpawner.ObjectInstantiate();
        Managers.Map.Init();
        
        yield return new WaitForSeconds(.15f);

        StartCoroutine(SecondScore());
        backgroundCtrl.speed = backgroundSpeed;
        StartCoroutine(backgroundCtrl.Scroll());

        coroutineCameraFollow = StartCoroutine(cameraCtrl.CameraFollow(playerCtrl.transform));

        
        playerCtrl.Initialization(ProgressType);
        //StartCoroutine(obstancleSpawner.ObjectSpawn(spawnTime));
    }

    IEnumerator GameClear()
    {
        isClear = true;
        
        StopCoroutine(coroutineCameraFollow);

        yield return new WaitForSeconds(1);

        stageLevel++;
        
        GameStop(false);
    }

    public void GameStop(bool isScoreClear)
    {
        feverAnim.StartAnimation(() =>
        {
            isPlaying = false;

            StopAllCoroutines();
            obstancleSpawner.objectRemove();
            
            EndFever();

            if (isScoreClear)
            {
                Score = 0;
                stageLevel = 1;
            }
            Initialization(ProgressType.Lobby);
            playerCtrl.Initialization(ProgressType);
            
            // Managers.Map.SetFloors(stageLevel);
            
            isClear = false;
        }, 6);
    }

    public void FeverMode()
    {
        Managers.Sound.FeverPlay();

        feverAnim.StartAnimation(() =>
        {
            StartCoroutine(FeverMode());
        }, 6);
        
        IEnumerator FeverMode()
        {
            playerCtrl.speed *= playerFeverSpeedMagnification;
            isFeverTime = true;

            backgroundCtrl.SetFever(true);
            var pos = playerCtrl.transform.position; //TODO
            pos.y = 0.1f;
            playerCtrl.transform.position = pos;
            playerCtrl.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            
            for (float f = feverTime; f >= 0; f -= Time.deltaTime)
            {
                Managers.UI.imgHealth.fillAmount = f / feverTime;

                yield return null;
            }
            
            feverAnim.StartAnimation(() =>
            {
                EndFever();
            }, 6);
        }
    }

    private void EndFever()
    {
        playerCtrl.GetComponent<Rigidbody2D>().constraints =  RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        
        backgroundCtrl.SetFever(false);
        //TODO

        Managers.Sound.DefaultPlay();

        playerCtrl.speed /= playerFeverSpeedMagnification;
        //TODO

        //obstancleSpawner.objectRemove();
        //obstancleSpawner.ObjectInstantiate();

        playerCtrl.SetLevelOrHealth(maxLevel - 2);

        isFeverTime = false;
    }
    
    public void ThrowObject(GameObject obj)
    {
        Managers.Sound.Play(EffectSoundClip.Attack);

        Score += 100 + (stageLevel * stageCombo);

        var rig = obj.AddComponent<Rigidbody2D>();

        if (rig is null) return;

        //rig.velocity = Vector2.right * throwPower;
        //rig.AddTorque(1000);
        rig.constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    public void DataSave()
    {
        PlayerPrefs.SetInt("StageLevel", stageLevel);
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.SetInt("Score", Score);
        PlayerPrefs.SetInt("IsPlaying", isPlaying ? 1 : 0);

        PlayerPrefs.Save();
    }

    public void DataLoad()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        
        stageLevel = PlayerPrefs.GetInt("StageLevel", 1);
        Score = PlayerPrefs.GetInt("Score", 0);
        
        isPlaying = PlayerPrefs.GetInt("IsPlaying", 0) == 1;
    }

    private void OnApplicationQuit()
    {
        DataSave();
    }

    #endregion

}