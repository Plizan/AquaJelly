using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyCtrl : MonoBehaviour
{
    public float speed;
    public int score;

    private void Awake()
    {
        speed *= Managers.Game.backgroundSpeed;
    }

    private void Start()
    {

    }

    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Managers.Game.score += score;
            Managers.Sound.Play(SoundForm.JELLY);
            Destroy(this);
        }
    }
}
