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

    #region CallbackFunction
    private void Start()
    {
        Managers.Sound.DefaultPlay();

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
        playerCtrl.isFirst = true;//TODO
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

    private IEnumerator SecondScore()
    {
        //TODO

        while (true)
        {
            Score += isFeverTime ? 30 : 5;
            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator GameStart()
    {
        Initialization(ProgressType.Game);

        Camera.main.DOOrthoSize(1.777778f, .5f).SetEase(Ease.InSine);
        yield return new WaitForSeconds(.4f);

        playerCtrl.Jump();
        playerCtrl.SetLevelOrHealth(startLevel);
        //obstancleSpawner.ObjectInstantiate();
        yield return new WaitForSeconds(.15f);

        StartCoroutine(SecondScore());
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
        Managers.Sound.FeverPlay();



        StartCoroutine(feverAnim.StartAnimation());
        yield return new WaitForSeconds(0.35f);
        playerCtrl.speed *= playerFeverSpeedMagnification;
        isFeverTime = true;
         
        backgroundCtrl.SetFever(true);
        var pos = playerCtrl.transform.position;//TODO
        pos.y = 0.1f;
        playerCtrl.transform.position = pos;
        var rig = playerCtrl.GetComponent<Rigidbody2D>();
        rig.constraints = RigidbodyConstraints2D.FreezeAll;

        for (float f = feverTime; f >= 0; f -= Time.deltaTime)
        {
            Managers.UI.imgHealth.fillAmount = f / feverTime;

            yield return null;
        }

        rig.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        StartCoroutine(feverAnim.StartAnimation());
        yield return new WaitForSeconds(0.35f);//TODO
        backgroundCtrl.SetFever(false);
        //TODO

        Managers.Sound.DefaultPlay();

        playerCtrl.speed /= playerFeverSpeedMagnification;

        //obstancleSpawner.objectRemove();
        //obstancleSpawner.ObjectInstantiate();

        playerCtrl.SetLevelOrHealth(maxLevel - 2);

        isFeverTime = false;
    }

    public void ThrowObject(GameObject obj)
    {
        Managers.Sound.Play(EffectSoundClip.Attack);

        score += 40;//TODO

        var rig = obj.AddComponent<Rigidbody2D>();

        if (rig is null) return;

        rig.velocity = Vector2.right * throwPower;
        rig.AddTorque(1000);
    }
    #endregion
}
