using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerCtrl : MonoBehaviour
{
    [HideInInspector] public float speed = 20;
    [HideInInspector] public float power = 1;
    [HideInInspector] public float maxJumpCount = 1;
    [HideInInspector] public float dotDamage = 3;
    [HideInInspector] public bool isInvincibility = false;

    private int level;

    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Sprite[] runSprites;
    [SerializeField] private Sprite[] jumpSprites;
    [SerializeField] private Sprite[] feverSprites;

    [SerializeField] private float animationDelay;

    [SerializeField] private float[] levelSize;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public int Level
    {
        get => level;
        set
        {
            Managers.Sound.Play(EffectSoundClip.SizeChange);

            Mathf.Min(Managers.Game.maxLevel, value);

            transform.DOScale(Vector3.one * levelSize[value], Mathf.Abs(level - value));

            level = value;
        }
    }
    [SerializeField] private float health;
    public float Health
    {
        get => health;
        set
        {
            health = value;
            Managers.UI.imgHealth.fillAmount = health / Managers.Game.maxHealth;
        }
    }

    public void SetLevelOrHealth(int level)
    {
        Level = level;
        Health = Managers.Game.maxHealth / (Managers.Game.maxLevel - 1) * level;
    }

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool isJump = false;
    
    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !isJump && jumpCount < maxJumpCount && Managers.Game.ProgressType == ProgressType.Game && !Managers.Game.isFeverTime)
        {
            isJump = true;
        }
    }

    //bool isCollision;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            jumpCount = 0;
            //isCollision = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.collider.CompareTag("Floor"))
        //isCollision = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isInvincibility && collider.CompareTag("Obstacle"))
        {
            var tra = collider.transform.parent;
            var info = tra.GetComponent<ObstacleInfo>();

            if (info is null) return;

            if (Managers.Game.isFeverTime || level > info.maxLevel)
            {
                Managers.Game.ThrowObject(tra.gameObject);
                return;
            }

            if (level < info.minLevel) return;

            Managers.Sound.Play(EffectSoundClip.Hit);
            Health -= info.damage;

            //StartCoroutine(Invincibility());
            StartCoroutine(Damage());
        }
        else if (collider.CompareTag("Jelly"))
        {
            Managers.Sound.Play(EffectSoundClip.Item);

            var info = collider.GetComponent<JellyInfo>();
            Managers.Game.Score += info.score + Managers.Game.stageLevel;
            Health += info.heal;


            for (int i = 0; i < Managers.Pool.jellyEffectPool.Count; i++)
            {
                if (!Managers.Pool.jellyEffectPool[i].activeSelf)
                {
                    Managers.Pool.jellyEffectPool[i].transform.position = transform.position;
                    Managers.Pool.jellyEffectPool[i].SetActive(true);
                    break;
                }
            }

        }

        else if (collider.CompareTag("SpawnPoint"))
            //Managers.Game.obstancleSpawner.ObjectInstantiate();
        {

        }
        else if (collider.CompareTag("ThrowObstacleSpawn"))
        {
            for (int i = 0; i <Managers.Pool.warn.Length; i++)
            {
                if (!Managers.Pool.warn[i].activeSelf)
                {
                    float y = collider.GetComponent<ThrowSpawn>().throwObstacle.transform.position.y * 310 + 35;
                    Debug.Log(y);
                    Managers.Pool.warn[i].GetComponent<RectTransform>().anchoredPosition = Vector2.right * -50 + Vector2.up * y;

                    Managers.Pool.warn[i].SetActive(true);
                    break;
                }
            }
        }
    }

    public Coroutine _animation;

    public void Initialization(ProgressType progressType)
    {
        void Init()
        {
            Managers.Game.cameraCtrl.transform.position = new Vector3(0, 0, -100);
            _rigidbody.velocity = Vector2.zero;
            transform.position = new Vector3(-0.0529999994f, -1.22099996f, -1f);
            spriteRenderer.color = Color.white;
            spriteRenderer.sprite = runSprites[0];
            transform.localScale = Vector3.one;
        }
        
        switch (progressType)
        {
            case ProgressType.Lobby:
                StopAllCoroutines();
                Init();
                break;
            case ProgressType.Game:
                Init();
                StartCoroutine(Movement());
                _animation = StartCoroutine(Animation());
                StartCoroutine(HealthUpdate());
                break;
            case ProgressType.None:
                break;
        }
    }

    WaitForSeconds damageDelay = new WaitForSeconds(0.1f);
    SpriteRenderer spriteRenderer;
    IEnumerator Damage()
    {
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = Color.red;
            yield return damageDelay;
            spriteRenderer.color = Color.white;
            yield return damageDelay;
        }
    }

    public IEnumerator Invincibility()
    {
        isInvincibility = true;

        bool isEnable = true;
        var spriteRenderer = GetComponent<SpriteRenderer>();

        for (float f = 0; f < Managers.Game.invincibilityTime; f += Time.deltaTime)
        {
            spriteRenderer.enabled = !isEnable;
            yield return new WaitForSeconds(.1f);
        }

        spriteRenderer.enabled = true;
        isInvincibility = false;
    }

    public IEnumerator HealthUpdate()
    {
        float maxHealth = Managers.Game.maxHealth;
        float maxLevel = Managers.Game.maxLevel;

        while (true)
        {
            if (!Managers.Game.isFeverTime)
                Health -= Time.deltaTime * dotDamage;
            else
                while (Managers.Game.isFeverTime)
                    yield return null;
            
            //bool isWait = false;

            if (Level > 0 && Health < maxHealth / (maxLevel - 1) * (Level - 1))
            {
                Level--;
                //isWait = true;
            }
            else if (Level < maxLevel && Health > maxHealth / (maxLevel - 1) * Level)
            {
                Level++;

                if (Level == maxLevel) Managers.Game.FeverMode();

                //isWait = true;
            }

            if (Health <= 0)
            {
                Managers.Game.stageLevel = 1;
                Managers.Game.Score = 0;
                Managers.Game.GameStop();
                yield break;
            }

            //if (isWait) yield return new WaitForSeconds(1);

            yield return null;
        }
    }

    public IEnumerator Movement()
    {
        while (true)
        {
            //Vector3 pos = transform.position + transform.right * Time.deltaTime * speed;
            //rigidbody.MovePosition(pos);
            transform.Translate(Vector2.right * Time.deltaTime * speed);

            yield return null;
        }
    }

    public IEnumerator Animation()
    {
        uint i = 0;

        while (true)
        {
            var value = ++i % (Managers.Game.isFeverTime ? feverSprites.Length : runSprites.Length);
            _spriteRenderer.sprite = Managers.Game.isFeverTime ? feverSprites[value] : runSprites[value];

            if (!Managers.Game.isFeverTime && /*isCollision &&*/ value == 1)
                _rigidbody.velocity += Vector2.up * power / 3;

            if(isJump)
                yield return StartCoroutine(JumpAnimation());

            yield return new WaitForSeconds(animationDelay);
        }
    }

    public IEnumerator JumpAnimation()
    {
        isJump = false;
        Jump();
        
        for (int i = 0; i < jumpSprites.Length; i++)
        {
            _spriteRenderer.sprite = jumpSprites[i];
            yield return new WaitForSeconds(animationDelay);
        }
    }

    int jumpCount = 0;
    public bool isFirst = true;

    public void Jump()
    {
        if (!isFirst)
            Managers.Sound.Play(EffectSoundClip.Jump);

        isFirst = false;
        jumpCount++;
        _rigidbody.velocity += Vector2.up * power;
        Debug.Log("jump");
    }
}