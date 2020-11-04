using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyCtrl : MonoBehaviour
{
    public float speed;
    public int score;

    private void Awake()
    {
        if (null == Managers.Game) return;
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
            if (null != Managers.Game)
            {
                //Managers.Game.Score += score;
                //Managers.Game.Health += score * 0.01f;
                Managers.Sound.Play(SoundClip.JELLY);
            }

            Destroy(this.gameObject);
        }
    }
}
