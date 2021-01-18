using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class Managers : MonoBehaviour
{
    public static GameObject _gameObject;

    private static TitleManager title;
    public static TitleManager Title { get => title; set => title = value; }

    private static GameManager game;
    public static GameManager Game { get => game; set => game = value; }

    private static SoundManager sound;
    public static SoundManager Sound { get => sound; set => sound = value; }

    private static UIManager ui;
    public static UIManager UI { get => ui; set => ui = value; }

    private void Awake()
    {
        title = GetComponent<TitleManager>();
        sound = GetComponent<SoundManager>();
        game = GetComponent<GameManager>();
        ui = GetComponent<UIManager>();
    }
}

    
[CustomEditor(typeof(Managers)), CanEditMultipleObjects]
public class ManagersInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Set Player")) Managers.Game.SetPlayer();
        if (GUILayout.Button("Set UI")) Managers.UI.Initialization(Managers.Game.ProgressType);
        if (GUILayout.Button("Set Background")) Managers.Game.SetBackground();
        if (GUILayout.Button("Set Camera")) Managers.Game.SetCamera();
    }
}