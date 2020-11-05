using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float power = 1;
    [SerializeField] private int level;
    public int Level
    {
        get => level;
        set
        {
            if (level < value)
                Managers.Sound.Play(SoundClip.LEVELUP);
            else if (level > value)
                Managers.Sound.Play(SoundClip.LEVELDOWN);

            transform.DOScale(Vector3.one * value, Mathf.Abs(level - value));

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

    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && jumpCount < 2 && Managers.Game.ProgressType == ProgressType.Game)
        {
            Jump();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
            jumpCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.CompareTag("Obstacle"))
        {
            if (Managers.Game.isFeverTime)
                collider.transform.parent.gameObject.AddComponent<Rigidbody2D>().velocity = Vector2.up * power * 5;

            var parentTra = collider.transform.parent;
            var info = parentTra.GetComponent<ObstacleInfo>();

            if (level < info.level) return;

            Health -= info.damage;

            var layer = LayerMask.NameToLayer("NonCollider");
            parentTra.gameObject.layer = layer;

            foreach (Transform i in parentTra)
                if (i.CompareTag("Obstacle"))
                    i.gameObject.layer = layer;
        }

        else if (collider.CompareTag("Jelly"))
        {
            var info = collider.GetComponent<JellyInfo>();
            Managers.Game.Score += info.score;
            Health += info.heal;
        }

        else if (collider.CompareTag("SpawnPoint"))
            Managers.Game.obstancleSpawner.ObjectInstantiate();
    }

    public IEnumerator HealthUpdate()
    {
        float maxHealth = Managers.Game.maxHealth;
        float maxLevel = Managers.Game.maxLevel;

        while (true)
        {
            Health -= Time.deltaTime * 3;

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

                Debug.Log(Level);

                isWait = true;
            }

            if (Health <= 0) Managers.Game.GameStop();

            if (isWait) yield return new WaitForSeconds(1);

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

    int jumpCount = 0;
    public void Jump()
    {
        jumpCount++;
        rigidbody.velocity = Vector2.up * power;
    }
}
