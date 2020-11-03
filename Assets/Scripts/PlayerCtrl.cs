﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float power = 1;
    [SerializeField] private int level;
    public int Level { get => level; set { transform.DOScale(Vector3.one * value, Mathf.Abs(level - value)); level = Mathf.Min(value, 4); } }
    [SerializeField] private float health;
    public float Health { get => health; set { health = Mathf.Min(value, Managers.Game.maxHealth); Managers.UI.imgHealth.fillAmount = health / Managers.Game.maxHealth; } }

    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
            jumpCount = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && jumpCount < 2 && Managers.Game.ProgressType == ProgressType.Game)
        {
            Jump();
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
