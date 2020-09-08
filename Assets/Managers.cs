using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static TitleManager title;
    public static TitleManager Title { get => title; set => title = value; }

    private static GameManager game;
    public static GameManager Game { get => game; set => game = value; }

    private static SoundManager sound;
    public static SoundManager Sound { get => sound; set => sound = value; }

    private void Awake()
    {
        title = GetComponent<TitleManager>();
        sound = GetComponent<SoundManager>();

        DontDestroyOnLoad(this.gameObject);
    }
}
