using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [HideInInspector] public float speed = 20;
    [HideInInspector] public float power = 1;
    [HideInInspector] public float maxJumpCount = 1;
    [HideInInspector] public float dotDamage = 3;
    [HideInInspector] public bool isInvincibility = false;

    private int level;

    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Sprite fever;
    [SerializeField] private float animationDelay;

    [SerializeField] private float[] levelSize;

    public int Level
    {
        get => level;
        set
        {
            Managers.Sound.Play(EffectSoundClip.SizeChange);

            Mathf.Min(Managers.Game.maxLevel, value);

            transform.DOScale(Vector3.one * levelSize[value - 1], Mathf.Abs(level - value));

            level = value;
        }
    }
    [SerializeField] private float health;
    public float Health { get => health; set { health = value; Managers.UI.imgHealth.fillAmount = health / Managers.Game.maxHealth; } }

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

    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && jumpCount < maxJumpCount && Managers.Game.ProgressType == ProgressType.Game && !Managers.Game.isFeverTime)
        {
            Jump();
        }
    }

    bool isCollision;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            jumpCount = 0;
            isCollision = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
            isCollision = false;
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
        }
        else if (collider.CompareTag("Jelly"))
        {
            Managers.Sound.Play(EffectSoundClip.Item);

            var info = collider.GetComponent<JellyInfo>();
            Managers.Game.Score += info.score;
            Health += info.heal;
        }

        else if (collider.CompareTag("SpawnPoint"))
        //Managers.Game.obstancleSpawner.ObjectInstantiate();
        {
            Managers.Game.GameStop();
        }
    }

    public Coroutine _animation;
    public void Initialization(ProgressType progressType)
    {
        switch (progressType)
        {
            case ProgressType.Lobby:
                StopAllCoroutines();
                break;
            case ProgressType.Game:
                StartCoroutine(Movement());
                _animation = StartCoroutine(Animation());
                StartCoroutine(HealthUpdate());
                break;
            case ProgressType.None:
                break;
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

            bool isWait = false;

            if (Level > 0 && Health < maxHealth / (maxLevel - 1) * (Level - 1))
            {
                Level--;
                isWait = true;
            }
            else if (Level < maxLevel && Health > maxHealth / (maxLevel - 1) * Level)
            {
                Level++;

                if (Level == maxLevel) yield return StartCoroutine(Managers.Game.FeverMode());

                isWait = true;
            }

            if (Health <= 0) Managers.Game.GameStop();

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
            var value = ++i % sprites.Length;
            _spriteRenderer.sprite = Managers.Game.isFeverTime ? fever : sprites[value];

            if (!Managers.Game.isFeverTime && /*isCollision &&*/ value == 1)
                _rigidbody.velocity += Vector2.up * power / 3;

            yield return new WaitForSeconds(animationDelay);
        }
    }

    int jumpCount = 0;
    public bool isFirst = true;
    public void Jump()
    {
        if (!isFirst)
            Managers.Sound.Play(isFirst ? EffectSoundClip.GameStart : EffectSoundClip.Jump);

        isFirst = false;
        jumpCount++;
        _rigidbody.velocity += Vector2.up * power;
    }
}
