using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCtrl : MonoBehaviour
{
    public float speed;
    public float demage;

    private void Awake()
    {
        if (null == Managers.Game) return;
        //speed *= Managers.Game.backgroundSpeed;
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
                //Managers.Game.Health -= demage;
                Managers.Sound.Play(SoundForm.OBJECT);
            }
        }
    }
}
